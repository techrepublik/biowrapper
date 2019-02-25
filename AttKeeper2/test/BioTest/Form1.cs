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
    public partial class Form1 : Form
    {
        RtEvent _rtEvent = new RtEvent();
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {

            _rtEvent.IpAddress = textBoxIp.Text.Trim();
            _rtEvent.Port = Convert.ToInt16(textBoxPort.Text);
            if (_rtEvent.ConnectToDevice(MacType.Net))
            {
                MessageBox.Show(@"Connected");
                _rtEvent.ExecuteEvent();
                _rtEvent.OnFingerEvent += OnFinger;
                _rtEvent.OnVerifyEvent += OnVerify;
                _rtEvent.OnAttTransactionEvent += new RtEvent.OnFingerAction(OnAttTransaction);
                //listBox1.Items.Add(r.OnFinger);
                //listBox1.Items.Add(r.OnVerify);
            }
        }

        private void OnFinger()
        {
            listBox1.Items.Insert(0,@"Fingerprint sensor triggered.");
        }

        private void OnVerify()
        {
            listBox1.Items.Insert(0,@"Verify process triggered.");
            listBox1.Items.Insert(0,@".." + _rtEvent.OnVerify);
        }

        private void OnAttTransaction()
        {
            listBox1.Items.Insert(0,@"Attendance log triggered.");
            listBox1.Items.Insert(0,@".." + _rtEvent.OnAttTransaction);
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            _rtEvent.DisconnectToDevice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f = new Form2();
            f.ShowDialog();
        }
    }
}
