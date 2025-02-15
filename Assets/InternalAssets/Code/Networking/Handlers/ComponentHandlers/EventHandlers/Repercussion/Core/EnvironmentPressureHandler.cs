using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Repercussion.Core;
using ProjectOlog.Code.Mechanics.Repercussion.Core.Pressures;
using ProjectOlog.Code.Networking.Profiles.Entities;

namespace ProjectOlog.Code.Networking.Handlers.ComponentHandlers.EventHandlers.Repercussion.Core
{
    public sealed class EnvironmentPressureHandler : IComponentSerializer<EnvironmentPressure>
    {
        private NetworkEntitiesContainer _entitiesContainer;

        public EnvironmentPressureHandler(NetworkEntitiesContainer entitiesContainer)
        {
            _entitiesContainer = entitiesContainer;
        }
        
        public void Serialize(EnvironmentPressure component, out NetDataPackage dataPackage)
        {
            dataPackage = new NetDataPackage((ushort)component.EnvironmentType);
        }

        public void Deserialize(NetDataPackage dataPackage, out EnvironmentPressure component)
        {
            component = new EnvironmentPressure
            {
                EnvironmentType = (EEnvironmentType)dataPackage.GetUShort()
            };
        }
    }
}