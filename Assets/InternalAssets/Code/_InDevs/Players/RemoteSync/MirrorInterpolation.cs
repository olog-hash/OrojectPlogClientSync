using System.Collections.Generic;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Networking.Client;
using ProjectOlog.Code.Networking.Libs.MirrorInterpolation;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.RemoteSync
{
    public class MirrorInterpolation : MonoBehaviour
    {
        [Header("Toggle")]
        public bool interpolate = true;

        // snapshot interpolation settings
        [Header("Snapshot Interpolation")]
        public SnapshotInterpolationSettings snapshotSettings =
            new SnapshotInterpolationSettings();

        // runtime settings
        public double bufferTime => NetworkTime.TickInterval * snapshotSettings.bufferTimeMultiplier;

        // <servertime, snaps>
        public SortedList<double, Snapshot3D> snapshots = new SortedList<double, Snapshot3D>();

        // for smooth interpolation, we need to interpolate along server time.
        // any other time (arrival on client, client local time, etc.) is not
        // going to give smooth results.
        double localTimeline;

        // catchup / slowdown adjustments are applied to timescale,
        // to be adjusted in every update instead of when receiving messages.
        double localTimescale = 1;

        // we use EMA to average the last second worth of snapshot time diffs.
        // manually averaging the last second worth of values with a for loop
        // would be the same, but a moving average is faster because we only
        // ever add one value.
        ExponentialMovingAverage driftEma;
        ExponentialMovingAverage deliveryTimeEma; // average delivery time (standard deviation gives average jitter)
        

        [Header("Simulation")]
        bool lowFpsMode;
        double accumulatedDeltaTime;

        void Awake()
        {
            localTimeline -= 0.1f;
            
            // show vsync reminder. too easy to forget.
            Debug.Log("Reminder: Snapshot interpolation is smoothest & easiest to debug with Vsync off.");

            // initialize EMA with 'emaDuration' seconds worth of history.
            // 1 second holds 'sendRate' worth of values.
            // multiplied by emaDuration gives n-seconds.
            driftEma = new ExponentialMovingAverage(NetworkTime.TickRate * snapshotSettings.driftEmaDuration);
            deliveryTimeEma = new ExponentialMovingAverage(NetworkTime.TickRate * snapshotSettings.deliveryTimeEmaDuration);
        }

        // add snapshot & initialize client interpolation time if needed
        public void OnMessage(Snapshot3D snap)
        {
            // set local timestamp (= when it was received on our end)
            // Unity 2019 doesn't have Time.timeAsDouble yet
            snap.localTime = NetworkTime.localTime;

            // (optional) dynamic adjustment
            if (snapshotSettings.dynamicAdjustment)
            {
                // set bufferTime on the fly.
                // shows in inspector for easier debugging :)
                snapshotSettings.bufferTimeMultiplier = SnapshotInterpolation.DynamicAdjustment(
                    NetworkTime.TickInterval,
                    deliveryTimeEma.StandardDeviation,
                    snapshotSettings.dynamicAdjustmentTolerance
                );
            }
            
            //Debug.Log($"Snapshot: Time={snap.remoteTime:F3}, Pos={snap.Position}, Buffer={snapshots.Count}");
            // insert into the buffer & initialize / adjust / catchup
            SnapshotInterpolation.InsertAndAdjust(
                snapshots,
                snapshotSettings.bufferLimit,
                snap,
                ref localTimeline,
                ref localTimescale,
                NetworkTime.TickInterval,
                bufferTime,
                snapshotSettings.catchupSpeed,
                snapshotSettings.slowdownSpeed,
                ref driftEma,
                snapshotSettings.catchupNegativeThreshold,
                snapshotSettings.catchupPositiveThreshold,
                ref deliveryTimeEma);
        }

        public void UpdatePosition(float deltaTime, ref CharacterBodyLogger characterBodyLogger)
        {
            // accumulated delta allows us to simulate correct low fps + deltaTime
            // if necessary in client low fps mode.
            accumulatedDeltaTime += deltaTime;

            // only while we have snapshots.
            // timeline starts when the first snapshot arrives.
            if (snapshots.Count > 0)
            {
                // snapshot interpolation
                if (interpolate)
                {
                    // step
                    SnapshotInterpolation.Step(
                        snapshots,
                        // accumulate delta is Time.unscaledDeltaTime normally.
                        // and sum of past 10 delta's in low fps mode.
                        accumulatedDeltaTime, // accumulatedDeltaTimeДа.
                        ref localTimeline,
                        localTimescale,
                        out Snapshot3D fromSnapshot,
                        out Snapshot3D toSnapshot,
                        out double t);

                    // interpolate & apply
                    Snapshot3D computed = Snapshot3D.Interpolate(fromSnapshot, toSnapshot, t);
                    
                    // Плавное применение позиции и вращения
                    transform.position = computed.Position;
                    transform.rotation = computed.Rotation;

                    // Плавное применение ViewPitchDegrees
                    characterBodyLogger.ViewPitchDegrees = computed.ViewPitchDegrees;

                    // Применяем состояние с проверкой на резкие изменения
                    if (characterBodyLogger.CharacterBodyState != computed.CharacterBodyState)
                    {
                        characterBodyLogger.CharacterBodyState = computed.CharacterBodyState;
                    }

                    // IsGrounded обновляем с небольшой задержкой для избежания дрожания
                    if (characterBodyLogger.IsGrounded != computed.IsGrounded)
                    {
                        // Добавляем небольшую задержку при изменении состояния заземления
                        // чтобы избежать частого переключения
                        if (t > 0.7f || t < 0.3f)
                        {
                            characterBodyLogger.IsGrounded = computed.IsGrounded;
                        }
                    }

                    characterBodyLogger.MoveDirection = CalculateMoveVector(fromSnapshot.Position, toSnapshot.Position, toSnapshot.Rotation);

                    //Debug.Log($"Interpolation: t={t:F3}, From={fromSnapshot.Position}, To={toSnapshot.Position}");
                }
                // apply raw
                else
                {
                    Snapshot3D snap = snapshots.Values[0];
                    transform.position = snap.Position;
                    transform.rotation = snap.Rotation;
                    characterBodyLogger.ViewPitchDegrees = snap.ViewPitchDegrees;
                    characterBodyLogger.IsGrounded = snap.IsGrounded;
                    characterBodyLogger.CharacterBodyState = snap.CharacterBodyState;
                    snapshots.RemoveAt(0);
                }
            }

            // reset simulation helpers
            accumulatedDeltaTime = 0;
        }

        /// <summary>
        /// Принудительная установка положения и вращения
        /// </summary>
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            List<double> keysToDelete = new List<double>();
            
            foreach (KeyValuePair<double, Snapshot3D> snapshot in snapshots)
            {
                if (Vector3.Distance(snapshot.Value.Position, position) > 0.05)
                {
                    keysToDelete.Add(snapshot.Key);
                }
                else
                {
                    break;
                }
            }
            
            foreach (var key in keysToDelete)
            {
                snapshots.Remove(key);
            }

            transform.position = position;
            transform.rotation = rotation;
        }
        
        private Vector2 CalculateMoveVector(Vector3 previousPosition, Vector3 currentPosition, Quaternion currentRotation)
        {
            float deltaX = currentPosition.x - previousPosition.x;
            float deltaZ = currentPosition.z - previousPosition.z;

            Vector2 inputMoveDirection = new Vector2(deltaX, deltaZ);
            
            float rotationAngle = currentRotation.eulerAngles.y * Mathf.Deg2Rad;
            
            float sin = Mathf.Sin(rotationAngle);
            float cos = Mathf.Cos(rotationAngle);
            
            Vector2 rotatedMoveDirection = new Vector2(
                inputMoveDirection.x * cos - inputMoveDirection.y * sin,
                inputMoveDirection.x * sin + inputMoveDirection.y * cos
            );
            
            rotatedMoveDirection.x = -rotatedMoveDirection.x;

            return rotatedMoveDirection;
        }
    }
}