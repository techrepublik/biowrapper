using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BioWrapper;
using BioWrapper.bio;

namespace BioTest
{
    public partial class Form2 : Form
    {
        AttLog _attAttLog = new AttLog();
        private Boolean _bConnect = false;
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) { 
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            _bConnect = _attAttLog.ConnectNet(MacType.Net, "192.168.1.201", 4370, 1);
            if (_bConnect)
            {
                MessageBox.Show("Connected to Device");
            }
            else
            {
                MessageBox.Show("Cannot connect to device");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _attAttLog.GetAttLogAll();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = String.Format("{0}", _attAttLog.GetLogCountAll());
        }
    }
}
