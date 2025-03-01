using System;
using LiteNetLib.Utils;
using ModestTree;
using ProjectOlog.Code.Engine.StateMachines.Interactables;
using ProjectOlog.Code.Features.Objects.Interactables.StateMachines.KitPack.States;
using UnityEngine;

namespace ProjectOlog.Code.Features.Objects.Interactables.StateMachines.KitPack
{
    public class KitPackStateMachine : InteractionObjectStateManager
    {
        protected override Type EnumType => typeof(EKitPackState);
        
        public enum EKitPackState
        {
            Default,
            CoolDown,
        }
        
        [SerializeField] private KitPackContext _context;
        private KitPackNetworker _networker;
        
        public override void Init()
        {
            ValidateConstraints();
            
            _networker = new KitPackNetworker(this, _context);
            
            InitializeStates();
            RegisterNetworker(_networker);
        }

        private void ValidateConstraints()
        {
            Assert.IsNotNull(_context.KitPackModel, "Game object model is not assigned.");
        }
        
        private void InitializeStates()
        {
            // Add states to inherited stateManager. States - dictionary and set Initial State
            States.Add(EKitPackState.Default, new DefaultState(_context, EKitPackState.Default));
            States.Add(EKitPackState.CoolDown, new CoolDownState(_context, EKitPackState.CoolDown));
            
            // Set default state
            CurrentState = States[EKitPackState.Default];
        }
        
        public override void SetSnapshotData(NetDataPackage dataPackage)
        {
            var stateKey = (EKitPackState)dataPackage.GetShort();
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