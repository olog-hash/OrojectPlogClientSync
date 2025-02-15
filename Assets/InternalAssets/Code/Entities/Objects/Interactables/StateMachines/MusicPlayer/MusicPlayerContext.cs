using System;
using System.Collections.Generic;
using LiteNetLib.Utils;
using ProjectOlog.Code.Game.DiegeticInterface.LookPanel;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.MusicPlayer
{
    [Serializable]
    public class MusicPlayerContext : INetPackageSerializable
    {
        [Header("Tools")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SimpleTextPanelView _textPanelView;
        
        [Header("Assets")]
        public List<AudioClip> AllMusicClips = new List<AudioClip>();

        public MusicPlayerContext()
        {
            
        }
        
        public AudioSource AudioSource => _audioSource;
        public SimpleTextPanelView TextPanelView => _textPanelView;
        
        // ===
        [Header("Data")] 
        public int CurrentMusicClip;
        public float CurrentMusicParameter;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(CurrentMusicClip, CurrentMusicParameter);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            CurrentMusicClip = dataPackage.GetInt();
            CurrentMusicParameter = dataPackage.GetFloat();
        }
    }
}