using System;
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

            NodeGUI nodeGUI = new NodeGUI(localIP, localPort, remoteIP, remotePort, nickname, debug);
            nodeGUI.Show();
            this.Hide();
        }
    }
}
