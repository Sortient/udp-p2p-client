using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace udp_p2p_client
{
    public class RemoteNode
    {
        public string ip;
        public int port;
        public RemoteNode(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
    }
}
