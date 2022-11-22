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
    public partial class Selection : Form
    {
        public Selection()
        {
            InitializeComponent();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            Client client= new Client();
            client.Show();
            this.Hide();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            Server server = new Server();
            server.Show(); 
            this.Hide();
        }
    }
}
