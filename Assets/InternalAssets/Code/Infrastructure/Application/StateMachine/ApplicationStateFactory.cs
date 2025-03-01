using Zenject;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine
{
    
    public class ApplicationStateFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public ApplicationStateFactory(DiContainer container)
        {
            _container = container;
        }

        public T CreateState<T>() where T : ApplicationState
        {
            return _container.Resolve<T>();
        }
    }
}