using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace udp_p2p_client
{
    public partial class NodeGUI : Form
    {
        string localIP;
        int port;
        string remoteIP;
        int remotePort;
        Node node = null;
        string nickname;
        public NodeGUI(string localIP, int localPort, string remoteIP, int remotePort, string nickname)
        {
            InitializeComponent();

            this.localIP = localIP;
            this.port = localPort;
            this.remoteIP = remoteIP;
            this.remotePort = remotePort;
            this.nickname= nickname;
            node = new Node();
            node.go(this, port, localIP, remoteIP, remotePort, nickname);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //node.Send(this.localIP + ":" + this.port + " says: " + txtInput.Text, remoteIP, remotePort);
            //txtInput.Text = "";
            node.Send(txtInput.Text);
            txtInput.Text = "";
        }

        public void AppendText(string text)
        {
            if (txtOutput.InvokeRequired)
            {
                txtOutput.Invoke(new MethodInvoker(delegate { txtOutput.Text += Environment.NewLine + text; }));
            }
            else
            {
                txtOutput.Text += Environment.NewLine + text;
            }
        }

        public void AddToKnownNodesList(RemoteNode rn)
        {
            if (lstKnownNodes.InvokeRequired)
            {
                lstKnownNodes.Invoke(new MethodInvoker(delegate { lstKnownNodes.Items.Add(rn.ip + rn.port); }));
            }
            else
            {
                lstKnownNodes.Items.Add(rn.ip + rn.port);
            }
            
        }
        private void NodeGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
