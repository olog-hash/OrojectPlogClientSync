using System.Collections.Generic;
using System.Linq;
using ProjectOlog.Code._InDevs.Players.RemoteSync;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Networking.Client;
using ProjectOlog.Code.Networking.Libs.MirrorInterpolation;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Snapshot
{
    public class RemoteObjectInterpolation : MonoBehaviour
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
        public SortedList<double, RemoteObjectInterpolationSnapshot> snapshots = new SortedList<double, RemoteObjectInterpolationSnapshot>();

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
        public void OnMessage(RemoteObjectInterpolationSnapshot snap)
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

        public void UpdatePosition(float deltaTime)
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
                        out RemoteObjectInterpolationSnapshot fromSnapshot,
                        out RemoteObjectInterpolationSnapshot toSnapshot,
                        out double t);

                    // interpolate & apply
                    var computed = RemoteObjectInterpolationSnapshot.Interpolate(fromSnapshot, toSnapshot, t);
                    
                    // Плавное применение позиции и вращения
                    transform.position = computed.Position;
                    transform.rotation = computed.Rotation;
                    transform.localScale = computed.Scale;
                }
                // apply raw
                else
                {
                    var snap = snapshots.Values[0];
                    transform.position = snap.Position;
                    transform.rotation = snap.Rotation;
                    transform.localScale = snap.Scale;
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
            
            foreach (KeyValuePair<double, RemoteObjectInterpolationSnapshot> snapshot in snapshots)
            {
                if (Vector3.Distance(snapshot.Value.Position, position) > 0.005)
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

        public void SetPositionAndRotationClear(Vector3 position, Quaternion rotation)
        {
            if (snapshots.Count > 0)
            {
                var lastSnapshot = snapshots.Last();
                var lastSnapshotKey = lastSnapshot.Key;
                var lastSnapshotValue = lastSnapshot.Value;
                
                lastSnapshotValue.Position = position;
                lastSnapshotValue.Rotation = rotation;
                
                snapshots.Clear();
                
                snapshots.Add(lastSnapshotKey, lastSnapshotValue);
            }
            
            transform.position = position;
            transform.rotation = rotation;
        }
        
    }
}