using LiteNetLib.Utils;
using Scellecs.Morpeh.Providers;

namespace ProjectOlog.Code.Game.StateMachines.Interactables.Networking
{
    public abstract class ObjectNetworker
    {
        private IObjectNetworkerContainer _objectNetworkerContainer;
        
        public void Init(IObjectNetworkerContainer objectNetworkerContainer)
        {
            _objectNetworkerContainer = objectNetworkerContainer;
        }

        public virtual bool IsAvaliableToExecute(EntityProvider entityProvider)
        {
            return entityProvider is not null;
        }

        private void SendTo(string methodName, NetDataPackage sourceDataPackage)
        {
            // Клиент пока не может отправлять свой слепок обьектов на сервер.
        }
    }
}