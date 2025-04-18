﻿using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Infrastructure.Core.Batch;
using ProjectOlog.Code.Networking.Profiles.Entities;
using ProjectOlog.Code.Networking.Profiles.Users;
using Zenject;

namespace ProjectOlog.Code.Networking.Infrastructure.Core
{
    public abstract class NetWorkerClient
    {
        public string Name { get; private set; }
        
        protected NetTransportProvider _transportProvider;
        protected NetworkUsersContainer _usersContainer;
        protected NetworkEntitiesContainer _entitiesContainer;

        public NetWorkerClient()
        {
            Name = this.GetType().Name;
        }

        [Inject]
        public void Construct(NetTransportProvider transportProvider, NetworkUsersContainer usersContainer, NetworkEntitiesContainer entitiesContainer)
        {
            _transportProvider = transportProvider;
            _usersContainer = usersContainer;
            _entitiesContainer = entitiesContainer;
        }
        
        public virtual void Initialize()
        {
            
        }
        
        public virtual void OnLogicUpdate()
        {
            
        }

        public virtual bool IsAvaliableToExecute(NetPeer peer)
        {
            return true;
        }

        #region Senders

        protected void SendTo(string methodName, NetDataPackage sourceDataPackage, DeliveryMethod deliveryMethod)
        {
            //var dataPackage = new NetDataPackage(Name, methodName, sourceDataPackage);
            //_clientSender.Send(_clientSender.WritePacket(dataPackage.GetData()), deliveryMethod);
            
            var header = new PackageHeader()
            {
                NetWorkerName = Name,
                MethodName = methodName,
                
                DeliveryMethod = deliveryMethod,
            };
            
            _transportProvider.EnqueuePackage(header, sourceDataPackage);
        }

        #endregion
    }
}