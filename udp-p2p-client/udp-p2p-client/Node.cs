using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace udp_p2p_client
{
    internal class Node
    {
        public UdpClient client = null;
        public int port;
        public NetworkListener nlistener = null;
        public NodeGUI nodeGUI = null;
        public List<RemoteNode> nodes = new List<RemoteNode>();
        public string nickname;
        public string localIP;
        public List<Tuple<DateTime, string>> messageHistory;

        public void go(NodeGUI gui,int port, string localIP, string remoteIP, int remotePort, string nickname)
        {
            this.port = port;
            this.client= new UdpClient(port);
            this.nodeGUI = gui;
            this.nickname = nickname;
            this.localIP = localIP;
            this.nodeGUI.label1.Text = this.nickname + " listening on: " 
                + this.localIP + ":" + this.port;
            nlistener = new NetworkListener(this);
            RemoteNode initialNode = new RemoteNode(remoteIP, remotePort);
            nodes.Add(initialNode);
            this.nodeGUI.AddToKnownNodesList(initialNode);
            nlistener.Start();
        }

        public class NetworkListener : AbstractThread
        {
            int port;
            Node node = null;
            
            public NetworkListener(Node n)
            {
                this.node = n;
                port = node.port;
            }

            delegate void SetTextCallback(string text);
            public void AppendText(string text)
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                // taken from stackoverflow
                if (this.node.nodeGUI.txtOutput.InvokeRequired)
                {
                    this.node.nodeGUI.Invoke(new MethodInvoker(delegate { this.node.nodeGUI.txtOutput.Text += Environment.NewLine + text; }));
                }
                else
                {
                    this.node.nodeGUI.txtOutput.Text += Environment.NewLine + text;
                }
            }
            public override void Run()
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                try
                {
                    byte[] receive = new byte[65535];
                    while(true)
                    {
                        receive = node.client.Receive(ref ipEndPoint);
                        string data = Encoding.ASCII.GetString(receive);
                        string remoteIP = ipEndPoint.Address.ToString();
                        string port = ipEndPoint.Port.ToString();
                        string[] cmd = data.Split(' ');

                        if (cmd[0] == "/share_nodes")
                        {
                            string nodeList = cmd[1];
                            if (nodeList != "")
                            {
                                string[] nodeProperties = nodeList.Split(',');
                                List<RemoteNode> nodesToAdd = new List<RemoteNode>();
                                for (int i = 0; i < nodeProperties.Length / 2; i += 2)
                                {
                                    nodesToAdd.Add(new RemoteNode(nodeProperties[i],
                                        Convert.ToInt32(nodeProperties[i + 1])));
                                }
                                foreach (RemoteNode newNode in nodesToAdd)
                                {
                                    if (!node.IsNodeKnown(newNode.ip, newNode.port))
                                    {
                                        node.AddToKnownNodes(newNode.ip, newNode.port);
                                        node.client.Send(Encoding.ASCII.GetBytes("/share_nodes " + node.NodeList()),
                                            ("/share_nodes " + node.NodeList()).Length, newNode.ip, newNode.port);
                                    }
                                }
                            }
                        }
                        else if (cmd[0] == "/request_nodes")
                        {
                            byte[] nodes = Encoding.ASCII.GetBytes("/share_nodes " + node.NodeList());
                            node.AddToKnownNodes(ipEndPoint.Address.ToString(), ipEndPoint.Port);
                            node.client.Send(nodes, nodes.Length, ipEndPoint.Address.ToString(), ipEndPoint.Port);
                        }
                        else if (cmd[0] == "/ping")
                        {
                            node.client.Send(Encoding.ASCII.GetBytes("/pong "), ("/pong ").Length,
                                ipEndPoint.Address.ToString(), ipEndPoint.Port);
                        }
                        else
                        {
                            //DateTime time = Convert.ToDateTime(cmd[0]);
                            //Tuple<DateTime, string> tuple = Tuple.Create(time, cmd[0]);
                            //node.messageHistory.Add(tuple);
                            // node.nodeGUI.messageTone.Play();
                            node.nodeGUI.AppendText(node.Receive(data, remoteIP, port));
                        }

                        if (!node.IsNodeKnown(ipEndPoint.Address.ToString(), ipEndPoint.Port))
                        {
                            node.AddToKnownNodes(ipEndPoint.Address.ToString(), ipEndPoint.Port);
                            byte[] nodes = Encoding.ASCII.GetBytes("/share_nodes " + node.NodeList());
                            node.client.Send(nodes,nodes.Length, ipEndPoint.Address.ToString(), ipEndPoint.Port);

                            foreach(RemoteNode rn in node.nodes)
                            {
                                node.client.Send(nodes, nodes.Length, rn.ip, rn.port);
                            }
                            node.client.Send(Encoding.ASCII.GetBytes("/request_nodes "), ("/request_nodes ").Length, ipEndPoint.Address.ToString(), ipEndPoint.Port);
                        }
                        
                        receive = new byte[65535];
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            
            private void SetText(string text)
            {

            }

            public void ParseMessage(string[] cmd, IPEndPoint ipEndPoint)
            {
                if (cmd[0] == "/share_nodes")
                {
                    string nodeList = cmd[1];
                    if (nodeList != "")
                    {
                        string[] nodeProperties = nodeList.Split(',');
                        List<RemoteNode> nodesToAdd = new List<RemoteNode>();
                        for (int i = 0; i < nodeProperties.Length / 2; i += 2)
                        {
                            nodesToAdd.Add(new RemoteNode(nodeProperties[i],
                                Convert.ToInt32(nodeProperties[i + 1])));
                        }
                        foreach (RemoteNode newNode in nodesToAdd)
                        {
                            if (!node.IsNodeKnown(newNode.ip, newNode.port))
                            {
                                node.AddToKnownNodes(newNode.ip, newNode.port);
                                node.client.Send(Encoding.ASCII.GetBytes("/share_nodes " + node.NodeList()),
                                    ("/share_nodes " + node.NodeList()).Length, newNode.ip, newNode.port);
                            }
                        }
                    }
                }
                else if (cmd[0] == "/request_nodes")
                {
                    byte[] nodes = Encoding.ASCII.GetBytes("/share_nodes " + node.NodeList());
                    node.AddToKnownNodes(ipEndPoint.Address.ToString(), ipEndPoint.Port);
                    node.client.Send(nodes, nodes.Length, ipEndPoint.Address.ToString(), ipEndPoint.Port);
                }
                else if (cmd[0] == "/ping")
                {
                    node.client.Send(Encoding.ASCII.GetBytes("/pong "), ("/pong ").Length,
                        ipEndPoint.Address.ToString(), ipEndPoint.Port);
                }
                else
                {
                    //DateTime time = Convert.ToDateTime(cmd[0]);
                    //Tuple<DateTime, string> tuple = Tuple.Create(time, cmd[0]);
                    //node.messageHistory.Add(tuple);
                    //node.nodeGUI.AppendText(node.Receive(data, remoteIP, port));
                }
            }
        }
        public string Receive(string line, string remoteIP, string port)
        {
            string output = line;
            return output;
        }

        public bool IsNodeKnown(string ip, int port)
        {
            foreach (RemoteNode rn in this.nodes.ToList())
            {
                if (rn.ip == ip && rn.port == port)
                {
                    return true;
                    
                }
            }
            return false;
        }

        public void AddToKnownNodes(string ip, int port)
        {
            RemoteNode nodeToAdd = new RemoteNode(ip, port);
            this.nodes.Add(nodeToAdd);
            this.nodeGUI.AddToKnownNodesList(nodeToAdd);
        }

        public void AddToMessageHistory(string message)
        {

        }

        public string NodeList()
        {
            string list = "";
            foreach (RemoteNode rn in this.nodes.ToList())
            {
                list += rn.ip + ',' + rn.port + ',';
            }
            return list;
        }


        public void Send(string msg)
        {
            try
            {
                // msg = this.nickname + " says: " + msg + '`';
                byte[] bytes = null;
                string[] cmd = msg.Split(' ');
                if (cmd[0] == "/share_nodes")
                {
                    foreach(RemoteNode rn1 in this.nodes)
                    {
                        foreach (RemoteNode rn2 in this.nodes)
                        {
                            if (rn1 != rn2)
                            {
                                msg += rn2.ip + "," + rn2.port +",";
                                bytes = Encoding.ASCII.GetBytes(msg);
                                this.client.Send(bytes, bytes.Length, rn1.ip, rn1.port);
                            }
                        }
                    }
                }
                else if (cmd[0] == "ping")
                {
                    foreach (RemoteNode rn in this.nodes)
                    {
                        bytes = Encoding.ASCII.GetBytes("/ping ");
                        this.client.Send(bytes, bytes.Length, rn.ip, rn.port);
                    }
                }
                else
                {
                    this.nodeGUI.txtOutput.AppendText(Environment.NewLine + "You said: " + msg);
                    msg = nickname + " says: " + msg; //DateTime.Now.ToString() + " " + nickname + " says: " + msg;
                    bytes = Encoding.ASCII.GetBytes(msg);
                    foreach (RemoteNode rn in this.nodes)
                    {
                        
                        if (rn.ip == this.localIP && rn.port == this.port)
                        {
                            // do nothing
                        }
                        else
                        {                            
                            IPEndPoint ipAddress = new IPEndPoint(IPAddress.Parse(rn.ip), rn.port);
                            this.client.Send(bytes, bytes.Length, rn.ip, rn.port);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void ListenForMessages()
        { 
        }

        public void SortMessages()
        {
            this.messageHistory.Sort();
            nodeGUI.txtOutput.Clear();
            foreach (Tuple<DateTime, string> item in messageHistory)
            {
                nodeGUI.txtOutput.Text += Environment.NewLine + item.Item2;
            }
        }
    }
}
