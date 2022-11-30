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
        bool botStarted = false;
        string nickname;
        public bool debug;
        string[] botMessages = new string[] {"Beep boop... hello there!", "How's it going?", "Did you see that " +
            "ludicrous display last night?", "This is an automated message.", "CHAT BOT ACTIVE!", "Hi guys!", "Great work y'all!",
            "I just started using this great new Distributed Chat system!", "What's everyone having for dinner?", "Probably gonna have to go soon",
            "I love P2P Messenger", "Craving some pringles right about now", "Played any new games recently?",
            "Wanna play quake?", "hi wua?", "a/s/l?", "I am from the San Fransisco Bay Area", "I love Shredded Wheat",
            "Seen the new season of big bang theory?", "Am I a living, thinking creature?", "greetings", "Chatbot Not Destroy",
            "yummy messages! :)"};
        public SoundPlayer messageTone = new SoundPlayer(@"..\RoyaltyFreeMessageTone.wav");
        Random r = new Random();
        public NodeGUI(string localIP, int localPort, string remoteIP, int remotePort, string nickname, bool debug)
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
    }
}
