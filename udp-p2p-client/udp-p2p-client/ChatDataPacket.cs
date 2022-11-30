using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace udp_p2p_client
{
    [Serializable]
    public class ChatDataPacket
    {
        public long messageID;
        public string nickname;
        public string ip;
        public int port;
        public string message;
        public string timestamp;
        Random r = new Random();

        public ChatDataPacket(string nickname, string ip, int port, string message, DateTime timestamp)
        {
            //string id = port.ToString() + r.Next(9000).ToString();
            //id.Replace(@".", string.Empty);
            this.messageID = port + r.Next(9000);
            this.nickname = nickname;
            this.ip = ip;
            this.port = port;
            this .message = message;
            this.timestamp = timestamp.ToString();
        }

        public ChatDataPacket(byte[] data)
        {
            string[] strings = Encoding.ASCII.GetString(data).Split('`');
            this.nickname = strings[0];
            this.ip = strings[1];
            this.port = Convert.ToInt32(strings[2]);
            this.message = strings[3];
            this.timestamp = strings[4];
        }
    }
}
