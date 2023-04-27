using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public partial class Form1 : Form
    {
        Server s;
        public Form1()
        {
            InitializeComponent();
        }

        //включить
        private void button1_Click(object sender, EventArgs e)
        {
           s = new Server();
           s.Start();
           Task t = Task.Run(() => { Listening();});
        }

        //послать сообщение
        private void button2_Click(object sender, EventArgs e)
        {
            if (s.state != "sleep")
            {
                s.Send_message(richTextBox1.Text);
                richTextBox1.Clear();
            }
        }

        //прослкшивание входящих сообщений
        private void Listening()
        {
            while (true)
            {
                if (s.state != "sleep")
                {
                    if (s.data != null)
                    {
                        richTextBox2.Text += s.data;
                        s.data = null;
                    }
                }
            }
        }

    }
}
