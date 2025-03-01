using LiteNetLib.Utils;
using ProjectOlog.Code.Engine.StateMachines.Interactables;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Features.Objects.Interactables.StateMachines.MusicPlayer.States
{
    public sealed class PlayingState : MusicPlayerState, IInteractableState
    {
        private float _maxMusicClipLength = 0;
        
        public PlayingState(MusicPlayerNetworker networker, MusicPlayerContext context, MusicPlayerStateMachine.EMusicPlayerState key) : base(networker, context, key)
        {
            InteractionStateName = "ОСТАНОВИТЬ";
        }

        public override void EnterState()
        {
            _maxMusicClipLength = 0;
            
            if (Context.AllMusicClips.Count == 0)
            {
                Debug.LogError("There are no clips!");
                SetNextState(MusicPlayerStateMachine.EMusicPlayerState.Default);
                return;
            }

            var currentClip = Context.AllMusicClips[Context.CurrentMusicClip];
            
            _maxMusicClipLength = currentClip.length;
            PlayAudioClip(currentClip, Context.CurrentMusicParameter);
            
            Context.CurrentMusicClip = (Context.CurrentMusicClip + 1) % Context.AllMusicClips.Count;
        }

        public override void ExitState()
        {
            _maxMusicClipLength = 0;
            
            Context.CurrentMusicParameter = 0;
            Context.AudioSource.Stop();
            Context.TextPanelView.Hide();
        }

        public override void UpdateState(float deltaTime)
        {
            if (_maxMusicClipLength == 0) return;
            
            if (Context.CurrentMusicParameter >= _maxMusicClipLength)
            {
                //SetNextState(MusicPlayerStateMachine.EMusicPlayerState.Default);
            }
            else
            {
                Context.CurrentMusicParameter += deltaTime;
            }
        }

        public void Interact(EntityProvider entityProvider, NetDataPackage detail)
        {
            //SetNextState(MusicPlayerStateMachine.EMusicPlayerState.Default);
        }

        private void PlayAudioClip(AudioClip clip, float time)
        {
            Context.AudioSource.clip = clip;
            Context.AudioSource.time = time;
            Context.AudioSource.Play();

            Context.TextPanelView.WriteText(clip.name);
        }
    }
}