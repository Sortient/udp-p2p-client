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
    public partial class Client : Form
    {
        string ipAddress;
        int port;
        UdpClient listener;
        public Client()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.Port = Convert.ToInt32(txtPort.Text);
            this.IPAddress = txtIPAddress.Text;
            this.SwitchControls();
            this.CreateClientConnection();
            txtOutput.Text += Environment.NewLine + "Client listening on " + 
                this.listener.Client + ":" + this.Port;
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
            EnableControl(txtInput);
            EnableControl(txtOutput);
            EnableControl(btnSend);
        }

        private void CreateClientConnection()
        {
            this.listener = new UdpClient(this.ipAddress, this.port);
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

    }
}
