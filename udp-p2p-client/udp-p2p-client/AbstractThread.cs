using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace udp_p2p_client
{
    public abstract class AbstractThread
    {
        private Thread _thread;

        protected AbstractThread()
        {
            _thread = new Thread(new ThreadStart(this.Run));
        }

        // Thread methods / properties
        public void Start() => _thread.Start();
        public void Join() => _thread.Join();
        public bool IsAlive => _thread.IsAlive;

        // Override in base class
        public abstract void Run();
    }

    


}
