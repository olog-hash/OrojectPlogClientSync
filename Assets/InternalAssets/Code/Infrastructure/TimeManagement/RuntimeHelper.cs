using System.Collections.Generic;
using ProjectOlog.Code.Infrastructure.TimeManagement.Interfaces;
using ProjectOlog.Code.Network.Client;
using UnityEngine;

namespace ProjectOlog.Code.Infrastructure.TimeManagement
{
    public class RuntimeHelper : MonoBehaviour
    {
        public static uint CurrentFixedStep = 0;
        public static float LastFixedUpdateTimeStep = 0;
        public static float LastFixedUpdateElapsedTime = 0;

        private List<IFixedUpdate> _fixedUpdateNotes = new List<IFixedUpdate>();
        private List<ITickUpdate> _tickUpdateNotes = new List<ITickUpdate>();
        private List<IUpdate> _updateNotes = new List<IUpdate>();
        private List<ILateUpdate> _lateUpdateNotes = new List<ILateUpdate>();
        private ITickUpdate _tickLateUpdate;

        private TickRateHandler _tickRateHandler;


        public RuntimeHelper()
        {
            Initialize();
        }

        public void Initialize()
        {
            _fixedUpdateNotes.Clear();
            _tickUpdateNotes.Clear();
            _lateUpdateNotes.Clear();
            _updateNotes.Clear();

            _tickLateUpdate = null;
            
            NetworkTime.OnTickRateWasChanged += OnTickRateWasChanged;
        }

        private void OnTickRateWasChanged()
        {
            _tickRateHandler = new TickRateHandler(TickUpdate, NetworkTime.TickInterval);
        }

        public void RegisterFixedUpdate(IFixedUpdate updatable, bool fromStart = false)
        {
            if (!_fixedUpdateNotes.Contains(updatable))
            {
                if (fromStart)
                {
                    _fixedUpdateNotes.Insert(0, updatable);
                }
                else
                {
                    _fixedUpdateNotes.Add(updatable);
                }
            }
        }

        public void DeregisterFixedUpdate(IFixedUpdate updatable)
        {
            if (_fixedUpdateNotes.Contains(updatable))
            {
                _fixedUpdateNotes.Remove(updatable);
            }
        }

        public void RegisterTickUpdate(ITickUpdate updatable, bool fromStart = false)
        {
            if (!_tickUpdateNotes.Contains(updatable))
            {
                if (fromStart)
                {
                    _tickUpdateNotes.Insert(0, updatable);
                }
                else
                {
                    _tickUpdateNotes.Add(updatable);
                }
            }
        }

        public void DeregisterTickUpdate(ITickUpdate updatable)
        {
            if (_tickUpdateNotes.Contains(updatable))
            {
                _tickUpdateNotes.Remove(updatable);
            }
        }

        public void RegisterTickLateUpdate(ITickUpdate updatable)
        {
            _tickLateUpdate = updatable;
        }

        public void DeregisterTickLateUpdate(ITickUpdate updatable)
        {
            _tickLateUpdate = null;
        }

        public void RegisterUpdate(IUpdate updatable, bool fromStart = false)
        {
            if (!_updateNotes.Contains(updatable))
            {
                if (fromStart)
                {
                    _updateNotes.Insert(0, updatable);
                }
                else
                {
                    _updateNotes.Add(updatable);
                }
            }
        }

        public void DeregisterUpdate(IUpdate updatable)
        {
            if (_updateNotes.Contains(updatable))
            {
                _updateNotes.Remove(updatable);
            }
        }

        public void RegisterLateUpdate(ILateUpdate updatable, bool fromStart = false)
        {
            if (!_lateUpdateNotes.Contains(updatable))
            {
                if (fromStart)
                {
                    _lateUpdateNotes.Insert(0, updatable);
                }
                else
                {
                    _lateUpdateNotes.Add(updatable);
                }
            }
        }

        public void DeregisterLateUpdate(ILateUpdate updatable)
        {
            if (_lateUpdateNotes.Contains(updatable))
            {
                _lateUpdateNotes.Remove(updatable);
            }
        }


        private void FixedUpdate()
        {
            CurrentFixedStep++;
            LastFixedUpdateElapsedTime = Time.time;
            LastFixedUpdateTimeStep = Time.fixedDeltaTime;

            for (int i = 0; i < _fixedUpdateNotes.Count; i++)
            {
                _fixedUpdateNotes[i].OnFixedUpdate(Time.fixedDeltaTime);
            }
        }

        public void TickUpdate()
        {
            NetworkTime.UpdateLocalTick();

            for (int i = 0; i < _tickUpdateNotes.Count; i++)
            {
                _tickUpdateNotes[i].OnTickUpdate(NetworkTime.TickInterval);
            }

            _tickLateUpdate?.OnTickUpdate(NetworkTime.TickInterval);
        }

        private void Update()
        {
            _tickRateHandler?.Update(Time.deltaTime);

            for (int i = 0; i < _updateNotes.Count; i++)
            {
                _updateNotes[i].OnUpdate(Time.deltaTime);
            }
        }

        private void LateUpdate()
        {
            for (int i = 0; i < _lateUpdateNotes.Count; i++)
            {
                _lateUpdateNotes[i].OnLateUpdate(Time.deltaTime);
            }
        }
    }
}