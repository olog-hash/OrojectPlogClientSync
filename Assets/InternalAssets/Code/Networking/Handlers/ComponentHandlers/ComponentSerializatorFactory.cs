using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Scellecs.Morpeh;
using Zenject;

namespace ProjectOlog.Code.Networking.Handlers.ComponentHandlers
{
    public sealed class ComponentSerializatorFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public ComponentSerializatorFactory(DiContainer container)
        {
            _container = container;
        }

        public IComponentSerializer<T> GetComponentHandler<T>() where T : struct, IComponent
        {
            return _container.Resolve<IComponentSerializer<T>>();
        }
        
        public List<object> GetAllComponentHandlers()
        {
            var serializerInterface = typeof(IComponentSerializer<>);
            var assembly = Assembly.GetExecutingAssembly();

            var serializerTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == serializerInterface));

            var serializers = new List<object>();

            foreach (var type in serializerTypes)
            {
                var componentType = type.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == serializerInterface)
                    .GetGenericArguments()[0];

                var serializer = _container.Resolve(type);
                serializers.Add(serializer);
            }

            return serializers;
        }
    }
}