using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace udp_p2p_client
{
    internal class Node
    {
        public bool debug = false;
        public bool broadcast = false;
        public string messageToSend = "";
        string externalString;
        public UdpClient client = null;
        public WebClient externalClient = new WebClient();
        public int port;
        public NetworkListener nlistener = null;
        public NodeGUI nodeGUI = null;
        public List<RemoteNode> nodes = new List<RemoteNode>();
        public List<RemoteNode> temp_list = null;
        public string nickname;
        public string localIP;
        public IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
        ImageConverter ic = new ImageConverter();
        System.Drawing.Image imageToSend = Properties.Resources.P2P1;

        public List<Tuple<DateTime, long, string, string>> messageHistory = new List<Tuple<DateTime, long, string, string>>();

        public void go(NodeGUI gui, int port, string localIP, string remoteIP, int remotePort, string nickname)
        {
            this.port = port;
            this.client = new UdpClient(port);
            this.nodeGUI = gui;
            this.nickname = nickname;
            this.localIP = localIP;
            this.nodeGUI.label1.Text = this.nickname + " listening on: "
                + this.localIP + ":" + this.port;
            nlistener = new NetworkListener(this);
            RemoteNode initialNode = new RemoteNode(remoteIP, remotePort);
            nodes.Add(initialNode);
            this.nodeGUI.AddToKnownNodesList(initialNode);   
            if (broadcast)
            {
                client.EnableBroadcast = true;
                Introduce("255.255.255.255", remotePort);
            }
            else
            {
                Introduce(initialNode.ip, initialNode.port);
            }
            //client.Client.ReceiveTimeout = 1000;
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
                    
                    while (true)
                    {
                        if (node.debug) { node.nodeGUI.AppendText("Waiting on packet..."); }
                        receive = node.client.Receive(ref ipEndPoint);
                        if (node.debug)
                        {
                            node.nodeGUI.AppendText("Packet received.");
                            node.nodeGUI.AppendText(ipEndPoint.ToString());
                        }

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
                                for (int i = 0; i < nodeProperties.Length - 1; i += 2)
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
                            if (!node.IsNodeKnown(ipEndPoint.Address.ToString(), ipEndPoint.Port))
                            {
                                node.AddToKnownNodes(ipEndPoint.Address.ToString(), ipEndPoint.Port);
                            }
                            node.client.Send(nodes, nodes.Length, ipEndPoint.Address.ToString(), ipEndPoint.Port);
                        }
                        else if (cmd[0] == "/ping")
                        {
                            node.client.Send(Encoding.ASCII.GetBytes("/pong "), ("/pong ").Length,
                                ipEndPoint.Address.ToString(), ipEndPoint.Port);
                        }
                        else if (cmd[0] == "/hello")
                        {
                            if (!node.IsNodeKnown(ipEndPoint.Address.ToString(), ipEndPoint.Port))
                            {
                                node.AddToKnownNodes(ipEndPoint.Address.ToString(), ipEndPoint.Port);
                            }
                        }
                        else if (cmd[0] == "/request_history")
                        {
                            string history = "/send_history ";
                            foreach (Tuple<DateTime, long, string, string> item in node.messageHistory)
                            {
                                history = "/send_history ";
                                history += item.Item1.ToString() + ',' + item.Item2.ToString() + ',' 
                                    + item.Item3 + ',' + item.Item4;
                                node.client.Send(Encoding.ASCII.GetBytes(history), history.Length,
                                ipEndPoint.Address.ToString(), ipEndPoint.Port);
                            }
                            history = "/history_completed";
                            node.client.Send(Encoding.ASCII.GetBytes(history), history.Length,
                                ipEndPoint.Address.ToString(), ipEndPoint.Port);
                        }
                        else if (cmd[0] == "/send_history")
                        {
                            string[] strings = (data.Remove(0, "/send_history ".Length)).Split(',');
                            node.messageHistory.Add(new Tuple<DateTime, long, string, string>(Convert.ToDateTime(strings[0]),
                                (long)Convert.ToDouble(strings[1]), strings[2], strings[3]));
                            
                        }
                        else if (cmd[0] == "/history_completed")
                        {
                            List<Tuple<DateTime, long, string, string>> temp = new List<Tuple<DateTime, long, string, string>>();
                            temp = node.messageHistory.Distinct().ToList();
                            node.messageHistory = temp;
                            node.SortMessages();
                            // node.nodeGUI.AppendText("You're all caught up!");
                        }
                        else if (cmd[0] == "/send_image")
                        {
                            node.ReceiveImage(Encoding.ASCII.GetBytes(cmd[1]));
                        }
                        else
                        {
                            //DateTime time = Convert.ToDateTime(cmd[0]);
                            //Tuple<DateTime, string> tuple = Tuple.Create(time, cmd[0]);
                            //node.messageHistory.Add(tuple);
                            // node.nodeGUI.messageTone.Play();
                            ChatDataPacket cdp = new ChatDataPacket(receive);
                            //node.nodeGUI.AppendText(node.Receive(data, remoteIP, port));
                            node.nodeGUI.AppendText(cdp.nickname + " says: " + cdp.message);
                            node.AddToMessageHistory(cdp);
                        }

                        if (!node.IsNodeKnown(ipEndPoint.Address.ToString(), ipEndPoint.Port))
                        {
                            // node.RequestHistory(ipEndPoint.Address.ToString(), ipEndPoint.Port);

                            this.node.Introduce(ipEndPoint.Address.ToString(), ipEndPoint.Port);
                            node.AddToKnownNodes(ipEndPoint.Address.ToString(), ipEndPoint.Port);
                            byte[] nodes = Encoding.ASCII.GetBytes("/share_nodes " + node.NodeList());
                            node.client.Send(nodes, nodes.Length, ipEndPoint.Address.ToString(), ipEndPoint.Port);

                            foreach (RemoteNode rn in node.nodes)
                            {
                                node.client.Send(nodes, nodes.Length, rn.ip, rn.port);
                            }

                            node.client.Send(Encoding.ASCII.GetBytes("/request_nodes "),
                                ("/request_nodes ").Length, ipEndPoint.Address.ToString(), ipEndPoint.Port);
                        }
                        this.node.ReorderList();
                        receive = new byte[65535];
                    }
                }

                catch (SocketException e)
                {
                    // MessageBox.Show(e.StackTrace);
                    // node.nodeGUI.AppendText(ipEndPoint.ToString());
                    // node.nodeGUI.AppendText("A node disconnected.");
                    ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    Run();
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

        public void ReorderList()
        {
            temp_list = new List<RemoteNode>();

            foreach (RemoteNode rn in nodes)
            {
                if (!temp_list.Contains(rn))
                {
                    temp_list.Add(rn);
                }
            }
            nodes = temp_list;
            temp_list = null;
            nodeGUI.ClearKnownNodesList();

            foreach (RemoteNode rn in nodes)
            {
                this.nodeGUI.AddToKnownNodesList(rn);
            }
        }

        public void AddToMessageHistory(ChatDataPacket packet)
        {
            bool isInHistory = false;/*
            for (int i = 0; i < messageHistory.Count; i++)
            {
                if (messageHistory[i].Item2 == packet.messageID)
                {
                    isInHistory = true;
                }
            }
            */
            if (!isInHistory)
            {
                Tuple<DateTime, long, string, string> itemToAdd;
                try
                {
                    itemToAdd = new Tuple<DateTime, long, string, string>(Convert.ToDateTime(packet.timestamp), packet.messageID,
                        packet.message, packet.nickname);
                    this.messageHistory.Add(itemToAdd);
                }
                catch(FormatException e)
                {
                    nodeGUI.AppendText("An unreadable message was received.");
                }
                
            }
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
                ChatDataPacket packet = new ChatDataPacket(this.nickname, this.localIP, this.port, msg, DateTime.Now);
                if (cmd[0] == "/share_nodes")
                {
                    foreach (RemoteNode rn1 in this.nodes)
                    {
                        foreach (RemoteNode rn2 in this.nodes)
                        {
                            if (rn1 != rn2)
                            {
                                msg += rn2.ip + "," + rn2.port + ",";
                                bytes = Encoding.ASCII.GetBytes(msg);
                                this.client.Send(bytes, bytes.Length, rn1.ip, rn1.port);
                            }
                        }
                    }
                }
                else if (cmd[0] == "/ping")
                {
                    foreach (RemoteNode rn in this.nodes)
                    {
                        bytes = Encoding.ASCII.GetBytes("/ping ");
                        this.client.Send(bytes, bytes.Length, rn.ip, rn.port);
                    }
                }
                else if (cmd[0] == "/request_history")
                {
                    foreach (RemoteNode rn in this.nodes)
                    {
                        bytes = Encoding.ASCII.GetBytes("/request_history ");
                        this.client.Send(bytes, bytes.Length, rn.ip, rn.port);
                    }
                }
                else
                {
                    this.nodeGUI.txtOutput.AppendText(Environment.NewLine + "You said: " + msg);
                    //msg = nickname + " says: " + msg; //DateTime.Now.ToString() + " " + nickname + " says: " + msg;
                    //bytes = Encoding.ASCII.GetBytes(packet.nickname + ",")
                    msg = this.nickname + "`" + this.localIP + "`" + this.port + "`" + msg + "`" + DateTime.Now.ToString();
                    bytes = Encoding.ASCII.GetBytes(msg);
                    foreach (RemoteNode rn in this.nodes)
                    {
                        if (rn.ip == this.localIP && rn.port == this.port)
                        {
                            // do nothing
                        }
                        else
                        {
                            //IPEndPoint ipAddress = new IPEndPoint(IPAddress.Parse(rn.ip), rn.port);
                            this.client.Send(bytes, bytes.Length, rn.ip, rn.port);

                        }
                    }
                    Tuple<DateTime, string> historyItem = new Tuple<DateTime, string>(DateTime.Now, msg);
                    this.AddToMessageHistory(packet);
                    // lstMessageHistory.Add(historyItem);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            this.ReorderList();
        }

        public void Send(string msg, string ip, int port)
        {
            try
            {
                // msg = this.nickname + " says: " + msg + '`';
                byte[] bytes = null;
                string[] cmd = msg.Split(' ');
                ChatDataPacket packet = new ChatDataPacket(this.nickname, this.localIP, this.port, msg, DateTime.Now);
                if (cmd[0] == "/request_history")
                {
                    bytes = Encoding.ASCII.GetBytes("/request_history ");
                    this.client.Send(bytes, bytes.Length, ip, port);
                }

                /*else
                {
                    this.nodeGUI.txtOutput.AppendText(Environment.NewLine + "You said: " + msg);
                    //msg = nickname + " says: " + msg; //DateTime.Now.ToString() + " " + nickname + " says: " + msg;
                    //bytes = Encoding.ASCII.GetBytes(packet.nickname + ",")
                    msg = this.nickname + "`" + this.localIP + "`" + this.port + "`" + msg + "`" + DateTime.Now.ToString();
                    bytes = Encoding.ASCII.GetBytes(msg);
                    foreach (RemoteNode rn in this.nodes)
                    {
                        if (rn.ip == this.localIP && rn.port == this.port)
                        {
                            // do nothing
                        }
                        else
                        {
                            //IPEndPoint ipAddress = new IPEndPoint(IPAddress.Parse(rn.ip), rn.port);
                            this.client.Send(bytes, bytes.Length, rn.ip, rn.port);

                        }
                    }
                    Tuple<DateTime, string> historyItem = new Tuple<DateTime, string>(DateTime.Now, msg);
                    this.AddToMessageHistory(packet);
                    // lstMessageHistory.Add(historyItem);
                }*/
            }
            catch (Exception e)
            {
                throw;
            }

            this.ReorderList();
        }

        public void RequestHistory(string remoteIP, int remotePort)
        {
            byte[] bytes = Encoding.ASCII.GetBytes("/request_history");
            this.client.Send(bytes, bytes.Length, remoteIP, remotePort);
        }
        public void ListenForMessages()
        {
        }

        public void Introduce(string remoteIP, int remotePort)
        {
            byte[] bytes = Encoding.ASCII.GetBytes("/hello");
            this.client.Send(bytes, bytes.Length, remoteIP, remotePort);
            bytes = Encoding.ASCII.GetBytes("/request_nodes");
            this.client.Send(bytes, bytes.Length, remoteIP, remotePort);
            bytes = Encoding.ASCII.GetBytes("/request_history ");
            this.client.Send(bytes, bytes.Length, remoteIP, remotePort);
        }


        public void SortMessages()
        {
            this.messageHistory.Sort();
            nodeGUI.ClearText();
            nodeGUI.AppendText("Welcome, " + this.nickname);
            // nodeGUI.AppendText("Deleting chat. Rebuilding from local data..." + Environment.NewLine);
            try
            {
                foreach (Tuple<DateTime, long, string, string> item in messageHistory)
                {
                    nodeGUI.AppendText(item.Item4 + " said: " + item.Item3);
                }
            }
            catch(InvalidOperationException ex) 
            {
                MessageBox.Show("Please wait. Building chat history...");
                this.nlistener.Run();
            }
        }

        public void SendImage()
        {
            List<RemoteNode> temp_nodes = this.nodes;
            string msg = "/send_image " + Encoding.ASCII.GetString(imageToByteArray(imageToSend));
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            foreach (RemoteNode rn in temp_nodes)
            {
                this.client.Send(bytes, bytes.Length, rn.ip, rn.port);
            }
        }

        public void ReceiveImage(byte[] img)
        {
            System.Drawing.Image image = byteArrayToImage(img);
            ImageBox imgBox = new ImageBox();
            imgBox.SetImage(image);
            imgBox.Show();
        }

        /*
         * Disclaimer:
         * Credit to https://www.codeproject.com/Articles/15460/C-Image-to-Byte-Array-and-Byte-Array-to-Image-Conv
         * 
         * I do not claim the below functions as my own.
         */
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
    }
}