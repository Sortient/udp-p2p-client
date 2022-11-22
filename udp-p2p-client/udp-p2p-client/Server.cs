using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udp_p2p_client
{
    public partial class Server : Form
    {
        UdpClient server;
        public Server()
        {
            InitializeComponent();
        }

        private void DisableControl(Control control)
        {
            control.Enabled = false;
        }

        private void EnableControl(Control control)
        {
            control.Enabled = true;
        }

        private void SwitchControls()
        {
            DisableControl(txtIPAddress);
            DisableControl(txtPort);
            EnableControl(txtOutput);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            SwitchControls();
        }
    }
}
