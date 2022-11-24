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
        public LoginGUI()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string localIP = txtLocalIPAddress.Text;
            int localPort = Convert.ToInt32(txtLocalPort.Text);
            string remoteIP = txtLocalIPAddress.Text;
            int remotePort = Convert.ToInt32(txtRemotePort.Text);
            string nickname = txtNickname.Text;

            NodeGUI nodeGUI = new NodeGUI(localIP, localPort, remoteIP, remotePort, nickname);
            nodeGUI.Show();
            this.Hide();
        }
    }
}
