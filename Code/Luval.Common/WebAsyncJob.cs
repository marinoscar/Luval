using System;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Luval.Common
{
    public class WebAsyncJob : IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _shuttingDown;
        private readonly Action _job;

        public Object Target { get; private set; }

        public WebAsyncJob(Action job, object target)
        {
            _job = job;
            Target = target;
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }

        public void ExecuteAsync()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }
                Task.Factory.StartNew(_job);
            }
        }
    }
}
