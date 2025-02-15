using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Repercussion.Core;
using ProjectOlog.Code.Mechanics.Repercussion.Core.Pressures;
using ProjectOlog.Code.Networking.Handlers.ComponentHandlers.AdvancedDeserializers.Core;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Handlers.ComponentHandlers.AdvancedDeserializers.Deserializators
{
    public class PressureDeserializer
    {
        private readonly GenericComponentDeserializer _deserializer;

        public PressureDeserializer(ComponentSerializator componentSerializator)
        {
            _deserializer = new GenericComponentDeserializer(componentSerializator);
        
            // Register handlers
            _deserializer.RegisterHandler<EPressureType, EnvironmentPressure>(EPressureType.EnvironmentPressure);
            _deserializer.RegisterHandler<EPressureType, ObjectPressure>(EPressureType.ObjectPressure);
            _deserializer.RegisterHandler<EPressureType, WeaponPressure>(EPressureType.WeaponPressure);
        }

        public bool TryDeserializeToEntity(EPressureType pressureType, NetDataPackage dataPackage, Entity entity)
        {
            return _deserializer.TryDeserializeToEntity(pressureType, dataPackage, entity);
        }
    }
}