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
using System.Net;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace udp_p2p_client
{
    public partial class NodeGUI : Form
    {
        int count = 0;
        string localIP;
        int port;
        string remoteIP;
        int remotePort;
        Node node = null;
        bool botStarted = false;
        string externalString = "Current price of LEGO Star Wars: The Skywalker Saga: ";
        string nickname;
        public bool debug;
        public bool clientDisabled = false;
        public bool broadcast = false;

        string[] botMessages = new string[] {"Beep boop... hello there!", "How's it going?", "Did you see that " +
            "ludicrous display last night?", "This is an automated message.", "CHAT BOT ACTIVE!", "Hi guys!", "What are your thoughts on the pedestrianisation"
            + " of Norwich city centre?", "Have I got a second series?",
            "I just started using this great new Distributed Chat system!", "What's everyone having for dinner?", "Probably gonna have to go soon",
            "I love P2P Messenger", "Craving some pringles right about now", "Played any new games recently?",
            "Wanna play quake?",  "The thing about Arsenal is they always try and walk it in",
            "Am I a living thinking creature?", "greetings", "What was Wenger thinking sending Walcott on that early?"};
        
        Random r = new Random();
        public NodeGUI(string localIP, int localPort, string remoteIP, int remotePort, string nickname, bool debug, bool broadcast)
        {
            InitializeComponent();

            this.localIP = localIP;
            this.port = localPort;
            this.remoteIP = remoteIP;
            this.remotePort = remotePort;
            this.nickname= nickname;
            this.debug = debug;
            node = new Node();
            txtOutput.Text += "Welcome, " + nickname + Environment.NewLine;
            this.Icon = Properties.Resources.p2p;
            HtmlAgilityPack.HtmlDocument docOld = new HtmlAgilityPack.HtmlDocument();
            node.broadcast = broadcast;
            
            node.go(this, port, localIP, remoteIP, remotePort, nickname);
        }

        private void btnSend_Click(object sender, EventArgs e) { SendText(false); }

        public void AppendText(string text)
        {
            if (txtOutput.InvokeRequired)
            {
                txtOutput.Invoke(new MethodInvoker(delegate { txtOutput.AppendText(Environment.NewLine + text); }));
            }
            else
            {
                txtOutput.AppendText(Environment.NewLine + text);
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
            if (!txtInput.Text.Contains("`"))
            {
                if (!isBot)
                {
                    node.messageToSend = txtInput.Text;
                    node.Send(txtInput.Text);
                    txtInput.Text = "";
                }
                else
                {
                    int index = r.Next(botMessages.Length);
                    node.Send(botMessages[index]);
                }
            }
            else
            {
                MessageBox.Show("Invalid character.");
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

        public void ClearText()
        {
            if (txtOutput.InvokeRequired)
            {
                txtOutput.Invoke(new MethodInvoker(delegate { txtOutput.Clear(); }));
            }
            else
            {
                txtOutput.Clear();
            }
        }

        private void btnBotSend_Click(object sender, EventArgs e)
        {
            if (botStarted == false)
            {
                timerBot.Start();
                botStarted = true;
                btnBotSend.BackColor = Color.LightGreen;
            }
            else
            {
                timerBot.Stop();
                botStarted = false;
                btnBotSend.BackColor = Color.LightCoral;
            }
        }

        private void timerBot_Tick(object sender, EventArgs e)
        {
            SendText(true);
        }

        /*
         * Note - this uses HtmlAgilityPack, an external library which I
         * do not take credit for
         */
        private void btnPost_Click(object sender, EventArgs e)
        {
            string url = "https://store.steampowered.com/app/920210/LEGO_Star_Wars_The_Skywalker_Saga/";
            var web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);
            ////*[@id="game_area_purchase_section_add_to_cart_296489"]/div[2]/div/div[1]
            externalString = "hey guys, the current price of the new LEGO Star Wars is " + doc.DocumentNode.SelectNodes("//*[@id=\"game_area_purchase_section_add_to_cart_296489\"]/div[2]/div/div[1]")[0].InnerText;

            node.Send(externalString);
        }

        private void btnSendMalformedData_Click(object sender, EventArgs e)
        {
            node.Send("``h``hy>?????//`/`/`ghfhad");
        }

        private void btnRebuildFromNetwork_Click(object sender, EventArgs e)
        {
            this.ClearText();
            //bool rebuilt = false;
            node.messageHistory.Clear();
            int index = r.Next(node.nodes.Count);
            foreach (RemoteNode rn in node.nodes)
            {
                node.Send("/request_history ", rn.ip, rn.port);
            }
            //node.SortMessages();

            for (int i = 0; i < node.messageHistory.Count - 1; i+=(node.nodes.Count-1))
            {
                node.nodes.RemoveAt(i);
            }

            node.SortMessages();
            /*int count = 0;
            while (rebuilt == false)
            {
                if (count < node.nodes.Count)
                {
                    node.Send("/request_history ", node.nodes[count].ip, node.nodes[count].port);
                    System.Threading.Thread.Sleep(1000);
                    if (this.txtOutput.Text != "")
                    {
                        rebuilt = true;
                    }
                    else
                    {
                        count++;
                    }
                }
                else
                {
                    MessageBox.Show("Could not rebuild chat history");
                    rebuilt = true;
                }
                
            }*/

            //node.Send("/request_history ");
        }

        private void btnSendImage_Click(object sender, EventArgs e)
        {
            // node.SendImage();
            node.SortMessages();
        }

        private void btnFindMissingData_Click(object sender, EventArgs e)
        {
            int count = 0;
            for (int i = 0; i < node.messageHistory.Count; i++)
            {
                if(i%3 == 0)
                {
                    node.messageHistory.RemoveAt(i);
                    count++;
                }
            }
            node.nodeGUI.AppendText("Deleted " + count + " chat items. Now rebuilding...");
            foreach (RemoteNode rn in node.nodes)
            {
                node.Send("/request_history ", rn.ip, rn.port);
            }


            node.SortMessages();
            //node.Send("/request_history ", node.nodes[node.nodes.Count - 1].ip, node.nodes[node.nodes.Count - 1].port);
            //node.nodeGUI.AppendText("Deleted " + count + " chat items. Now rebuilding...");
        }
        /*
        private void btnPatchyNetwork_Click(object sender, EventArgs e)
        {
            if (clientDisabled)
            {
                node.go(this, port, localIP, remoteIP, remotePort, nickname);
                clientDisabled = false;
                btnPatchyNetwork.BackColor = Color.LightCoral;
            }
            else
            {
                node.client.Close();
                clientDisabled = true;
                btnPatchyNetwork.BackColor = Color.LightGreen;
            }
        }
        */
    }
}
