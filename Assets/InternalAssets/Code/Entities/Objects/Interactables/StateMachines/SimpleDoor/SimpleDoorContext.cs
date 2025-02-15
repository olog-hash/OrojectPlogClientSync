using System;
using LiteNetLib.Utils;
using ProjectOlog.Code.Game.Core;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.SimpleDoor
{
    [Serializable]
    public class SimpleDoorContext : INetPackageSerializable
    {
        [Header("Tools")]
        [SerializeField] private Transform _pivotTransform;
        [SerializeField] private InterpolationProvider _interpolationProvider;
        [SerializeField] private AudioSource _audioSource;
        
        [Header("Assets")]
        public AudioClip OpenDoorClip;
        public AudioClip CloseDoorClip;

        public SimpleDoorContext()
        {
            
        }

        public Transform PivotTransform => _pivotTransform;
        public InterpolationProvider InterpolationProvider => _interpolationProvider;
        public AudioSource AudioSource => _audioSource;
        
        
        // ======
        
        [Header("Data")]
        public int CurrentActionTick;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(CurrentActionTick);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            CurrentActionTick = dataPackage.GetInt();
        }
    }
}