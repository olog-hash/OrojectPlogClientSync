using System;
using LiteNetLib.Utils;
using UnityEngine;

namespace ProjectOlog.Code.Features.Objects.Interactables.StateMachines.KitPack
{
    [Serializable]
    public class KitPackContext : INetPackageSerializable
    {
        [SerializeField] private AbstractModuleAction _actionModule;
        [SerializeField] private GameObject _kitPackModel;
        [SerializeField] private Collider _triggerCollider;
        
        public KitPackContext()
        {
            
        }

        public AbstractModuleAction ActionModule => _actionModule;
        public GameObject KitPackModel => _kitPackModel;
        public Collider TriggerCollider => _triggerCollider;
        
        // ====
        [Header("Data")] 
        public int CoolDownTime = 15;
        public int CurrentTime = 0;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage();
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            
        }
    }
}