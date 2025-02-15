using System;
using LiteNetLib.Utils;
using ModestTree;
using ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.SimpleDoor.States;
using ProjectOlog.Code.Game.StateMachines.Interactables;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.SimpleDoor
{
    public class SimpleDoorStateMachine : InteractionObjectStateManager
    {
        protected override Type EnumType => typeof(ESimpleDoorState);
        
        public enum ESimpleDoorState
        {
            Closed,
            Opening,
            Opened,
            Closing
        }

        [SerializeField] private SimpleDoorContext _context;
        private SimpleDoorNetworker _networker;

        public override void Init()
        {
            ValidateConstraints();

            InteractionObjectName = "ДВЕРЬ";
            _networker = new SimpleDoorNetworker(this, _context);
            
            InitializeStates();
            RegisterNetworker(_networker);
        }

        private void ValidateConstraints()
        {
            Assert.IsNotNull(_context.PivotTransform, "Pivot transform is not assigned.");
        }
        
        private void InitializeStates()
        {
            // Add states to inherited stateManager. States - dictionary and set Initial State
            States.Add(ESimpleDoorState.Closed, new ClosedState(_networker, _context, ESimpleDoorState.Closed));
            States.Add(ESimpleDoorState.Closing, new ClosingState(_networker, _context, ESimpleDoorState.Closing));
            States.Add(ESimpleDoorState.Opened, new OpenedState(_networker, _context, ESimpleDoorState.Opened));
            States.Add(ESimpleDoorState.Opening, new OpeningState(_networker, _context, ESimpleDoorState.Opening));
            
            // Set default state
            CurrentState = States[ESimpleDoorState.Closed];
        }
        
        public override void SetSnapshotData(NetDataPackage dataPackage)
        {
            var stateKey = (ESimpleDoorState)dataPackage.GetShort();
            var objectData = dataPackage.GetPackage();
            
            _context.Deserialize(objectData);
            CurrentState = States[stateKey];
            CurrentState.EnterState();
        }

        public override NetDataPackage GetSnapshotData()
        {
            short currentStateKey = Convert.ToInt16(CurrentState.StateKey);   
            
            return new NetDataPackage(currentStateKey, _context.GetPackage());
        }
    }
}