using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace Network
{
    public partial class Form1 : Form
    {
        Socket socket;
        string data;
        public Form1()
        {
            InitializeComponent();
        }

        //connect
        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress IPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(IPAddress, 777);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(endPoint);
                //прослушивание сообщений
                Task t = Task.Run(() => { Listening(); });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (socket.Connected)
            {
                String s = richTextBox1.Text;
                socket.Send(Encoding.Default.GetBytes(s));
                richTextBox1.Clear();
            }
        }

        //Disconnect
        private void button3_Click(object sender, EventArgs e)
        {
            if (socket.Connected)
            {
                socket.Close();
            }
        }

        private void Listening()
        {
            while (true)
            {
                if (socket.Connected)
                {
                        byte[] buffer = new byte[4096];
                        int lenth = 0;
                        lenth = socket.Receive(buffer);
                        data = Encoding.Default.GetString(buffer, 0, lenth);
                        richTextBox1.Text += data;
                        data = null;
                }
            }
        }

    }
}
