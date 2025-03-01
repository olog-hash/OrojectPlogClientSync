using System;
using LiteNetLib.Utils;
using ModestTree;
using ProjectOlog.Code.Engine.StateMachines.Interactables;
using ProjectOlog.Code.Features.Objects.Interactables.StateMachines.MusicPlayer.States;
using UnityEngine;

namespace ProjectOlog.Code.Features.Objects.Interactables.StateMachines.MusicPlayer
{
    public class MusicPlayerStateMachine : InteractionObjectStateManager
    {
        protected override Type EnumType => typeof(EMusicPlayerState);
        
        public enum EMusicPlayerState
        {
            Default,
            Playing,
        }
        
        [SerializeField] private MusicPlayerContext _context;
        private MusicPlayerNetworker _networker;
        
        
        public override void Init()
        {
            ValidateConstraints();
            
            InteractionObjectName = "Music Box";
            _networker = new MusicPlayerNetworker(this, _context);
            
            InitializeStates();
            RegisterNetworker(_networker);
        }

        private void ValidateConstraints()
        {
            Assert.IsNotNull(_context.AudioSource, "Audio source is not assigned.");
            Assert.IsNotNull(_context.TextPanelView, "Text panel is not assigned.");
        }
        
        private void InitializeStates()
        {
            // Add states to inherited stateManager. States - dictionary and set Initial State
            States.Add(EMusicPlayerState.Default, new DefaultState(_networker, _context, EMusicPlayerState.Default));
            States.Add(EMusicPlayerState.Playing, new PlayingState(_networker, _context, EMusicPlayerState.Playing));
            
            // Set default state
            CurrentState = States[EMusicPlayerState.Default];
        }
        
        public override void SetSnapshotData(NetDataPackage dataPackage)
        {
            var stateKey = (EMusicPlayerState)dataPackage.GetShort();
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