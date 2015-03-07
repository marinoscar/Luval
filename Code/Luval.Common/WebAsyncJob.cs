using System;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Luval.Common
{
    public class WebAsyncJob : IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _shuttingDown;

        public WebAsyncJob()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }

        public void Execute(Task task)
        {
            if (task == null) return;
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }

                if(task.Status == TaskStatus.Created)
                    task.Start();
            }
        }
    }
}
