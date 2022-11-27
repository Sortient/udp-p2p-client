﻿using System;
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
using System.Media;

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
        public SoundPlayer messageTone = new SoundPlayer(@"..\RoyaltyFreeMessageTone.wav");
        public NodeGUI(string localIP, int localPort, string remoteIP, int remotePort, string nickname)
        {
            InitializeComponent();

            this.localIP = localIP;
            this.port = localPort;
            this.remoteIP = remoteIP;
            this.remotePort = remotePort;
            this.nickname= nickname;
            node = new Node();
            txtOutput.Text += "Welcome, " + nickname + Environment.NewLine;
            this.Icon = Properties.Resources.p2p;
            node.go(this, port, localIP, remoteIP, remotePort, nickname);
        }

        private void btnSend_Click(object sender, EventArgs e) { SendText(false); }

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

        public void ClearKnownNodesList()
        {
            if (lstKnownNodes.InvokeRequired)
            {
                lstKnownNodes.Invoke(new MethodInvoker(delegate { lstKnownNodes.Items.Clear(); }));
            }
            else
            {
                lstKnownNodes.Items.Clear();
            }
        }
        private void NodeGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            node.client.Close();
            System.Windows.Forms.Application.Exit();
        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            node.client.Close();
            System.Windows.Forms.Application.Exit();
        }

        private void SendText(bool isBot)
        {
            if(!isBot)
            {
                node.messageToSend = txtInput.Text;
                node.Send(txtInput.Text);
                txtInput.Text = "";
            }
            else
            {
                node.Send("Beep boop... this is an auto generated message!");
            }
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return) { SendText(false); }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            node.SortMessages();
        }

        public static void OnTimedPing(Object source, System.Timers.ElapsedEventArgs e)
        {
                
        }

        private void btnBotSend_Click(object sender, EventArgs e)
        {
            SendText(true);
        }
    }
}
