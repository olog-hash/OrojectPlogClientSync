using ProjectOlog.Code.UI.Core;
using Zenject;

namespace ProjectOlog.Code.UI.HUD
{
    public class HUDFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public HUDFactory(DiContainer container)
        {
            _container = container;
        }

        public T ResolveViewModel<T>() where T : BaseViewModel
        {
            return _container.Resolve<T>();
        }
    }
}