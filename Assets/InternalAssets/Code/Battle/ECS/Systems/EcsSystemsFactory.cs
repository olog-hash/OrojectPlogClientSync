using Scellecs.Morpeh;
using Zenject;

namespace ProjectOlog.Code.Battle.ECS.Systems
{
    public class EcsSystemsFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public EcsSystemsFactory(DiContainer container)
        {
            _container = container;
        }

        public T CreateSystem<T>() where T : class, ISystem
        {
            return _container.Resolve<T>();
        }

        public bool CreateSystem<T>(SystemsGroup systemsGroup, bool flag = true) where T : class, ISystem
        {
            return systemsGroup.AddSystem(CreateSystem<T>(), flag);
        }
        
        public bool CreateFeature<T>(SystemsGroup systemsGroup, bool flag = true) where T : FeatureSystemsBlock, new()
        {
            var feature = new T();
            
            feature.Execute(systemsGroup, this);
            return true;
        }
    }
}