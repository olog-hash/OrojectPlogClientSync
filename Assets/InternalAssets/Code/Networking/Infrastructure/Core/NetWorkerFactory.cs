using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zenject;

namespace ProjectOlog.Code.Networking.Infrastructure.Core
{
    public class NetWorkerFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public NetWorkerFactory(DiContainer container)
        {
            _container = container;
        }

        public T GetNetWorker<T>() where T : NetWorkerClient
        {
            return _container.Resolve<T>();
        }
        
        
        public List<NetWorkerClient> GetAllNetWorkers()
        {
            var netWorkerType = typeof(NetWorkerClient);
            var assembly = Assembly.GetAssembly(netWorkerType);

            var netWorkerTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && netWorkerType.IsAssignableFrom(t));

            var workers = new List<NetWorkerClient>();

            foreach (var type in netWorkerTypes)
            {
                var worker = (NetWorkerClient)_container.Resolve(type);
                workers.Add(worker);
            }

            return workers;
        }
        
    }
}