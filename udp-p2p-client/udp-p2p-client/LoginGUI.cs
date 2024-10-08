﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udp_p2p_client
{
    public partial class LoginGUI : Form
    {
        public bool macOSMode = false;
        string[] defaultNicknames = new string[] {"user123", "greg", "bill", "larry",
            "wilhelm", "sly bertram", "sneaky pete"};
        string[] adjectives = new string[] {"Cowardly","Lovely","Slovenly","Big","Small","Lil",
        "Hated","Loved","Stupid","Sly"};
        string[] nouns = new string[] {"Badger","Gamer","Snail","Guy","Clown","Cricket","User",
            "Soldier", "Peep"};
        Random r = new Random();

        public LoginGUI()
        {
            InitializeComponent();
            txtNickname.Text = adjectives[r.Next(adjectives.Length)] +
                nouns[r.Next(nouns.Length)] + r.Next(99).ToString(); //defaultNicknames[r.Next(defaultNicknames.Length)];
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string localIP = txtLocalIPAddress.Text;
            int localPort = Convert.ToInt32(txtLocalPort.Text);
            string remoteIP = txtRemoteIP.Text;
            int remotePort = Convert.ToInt32(txtRemotePort.Text);
            string nickname = txtNickname.Text;
            bool debug = false;

            if (chkDebug.Checked)
            {
                debug = true;
            }

            NodeGUI nodeGUI = new NodeGUI(localIP, localPort, remoteIP, remotePort, nickname, debug, false);
            nodeGUI.Show();
            this.Hide();
        }

        private void btnBroadcast_Click(object sender, EventArgs e)
        {
            string localIP = txtLocalIPAddress.Text;
            int localPort = Convert.ToInt32(txtLocalPort.Text);
            string remoteIP = "192.168.0.255"; //txtLocalIPAddress.Text;
            int remotePort = Convert.ToInt32(txtLocalPort.Text);
            string nickname = txtNickname.Text;
            bool debug = false;

            if (chkDebug.Checked)
            {
                debug = true;
            }
            
            NodeGUI nodeGUI = new NodeGUI(localIP, localPort, remoteIP, remotePort, nickname, debug, true);
            nodeGUI.broadcast= true;
            nodeGUI.Show();
            this.Hide();
        }

        private void chkApple_CheckedChanged(object sender, EventArgs e)
        {
            if (macOSMode)
            {
                this.pictureBox1.BackgroundImage = Properties.Resources.avatar_client;
                this.label6.Text = "P2P Messenger";
                this.label6.Font = new Font("Arial", 18);
                this.Font = new Font("Arial", 10);
                this.BackgroundImage = Properties.Resources.clientbg;
                this.txtNickname.Text = adjectives[r.Next(adjectives.Length)] +
                nouns[r.Next(nouns.Length)] + r.Next(99).ToString();
                macOSMode = false;
            }
            else
            {
                this.pictureBox1.BackgroundImage = Properties.Resources.apple;
                this.label6.Text = "Messaging. Reimagined.";
                this.label6.Font = new Font("Lucida Sans", 14);
                this.Font = new Font("Lucida Sans", 10);
                this.BackgroundImage = null;
                this.BackColor = Color.FromArgb(225, 225, 225);
                this.txtNickname.Text = "TimApple";
                this.btnConnect.FlatStyle = FlatStyle.Flat;
                macOSMode = true;

            }
        }
    }
}
