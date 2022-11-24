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
    public partial class SplashScreen : Form
    {
        public Random rand = new Random();
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //panel1.Width = 0;
            panel1.Width += rand.Next(10);
            if (panel1.Width >= 918)
            {
                timer1.Enabled = false;
                this.Hide();
                LoginGUI loginGUI = new LoginGUI();
                loginGUI.Show();
            }          
        }
    }
}
