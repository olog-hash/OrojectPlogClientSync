using ProjectOlog.Code.Infrastructure.Logging;
using ProjectOlog.Code.Infrastructure.Logging.Mapping;
using UnityEngine;
using Zenject;
using Console = ProjectOlog.Code.Infrastructure.Logging.Console;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class ConsoleInstaller: MonoInstaller
    {
        [SerializeField]
        private Console _console;
        
        [SerializeField]
        private BindManager _bindManager;
        
        public override void InstallBindings()
        {
            CommandManager.Reset();
         
            var bindManagerInstance = Container.InstantiatePrefabForComponent<BindManager>(_bindManager);
            Container.Bind<BindManager>().FromInstance(bindManagerInstance).AsCached();
            
            BindManager.Reset();
            
            Container.BindInterfacesAndSelfTo<Console>().FromInstance(_console).AsSingle().NonLazy();
            
            _console.Initialize();
        }
    }
}