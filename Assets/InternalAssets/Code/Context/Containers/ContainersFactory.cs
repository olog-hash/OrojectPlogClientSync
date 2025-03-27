using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zenject;

namespace ProjectOlog.Code.DataStorage.Core
{
    public class ContainersFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public ContainersFactory(DiContainer container)
        {
            _container = container;
        }

        public T GetProjectContainer<T>() where T : IProjectContainer
        {
            return _container.Resolve<T>();
        }
        
        public T GetSceneContainer<T>() where T : ISceneContainer
        {
            return _container.Resolve<T>();
        }
        
        public List<IProjectContainer> GetAllProjectContainers()
        {
            var containerType = typeof(IProjectContainer);
            var assembly = Assembly.GetAssembly(containerType);

            var containerTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && containerType.IsAssignableFrom(t));

            var containers = new List<IProjectContainer>();

            foreach (var type in containerTypes)
            {
                var worker = (IProjectContainer)_container.Resolve(type);
                containers.Add(worker);
            }

            return containers;
        }
        
        public List<ISceneContainer> GetAllSceneContainers()
        {
            var containerType = typeof(ISceneContainer);
            var assembly = Assembly.GetAssembly(containerType);

            var containerTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && containerType.IsAssignableFrom(t));

            var containers = new List<ISceneContainer>();

            foreach (var type in containerTypes)
            {
                var worker = (ISceneContainer)_container.Resolve(type);
                containers.Add(worker);
            }

            return containers;
        }
    }
}