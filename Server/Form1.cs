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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress IPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(IPAddress, 777);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Task task = Task.Run(() => { Start_server(socket, endPoint); });
            
        }

        private void Start_server (Socket s, EndPoint e)
        {
            try
            {
                s.Bind(e);
                s.Listen(10);
                while (true)
                {
                   Socket s_client = s.Accept();
                   Task task = Task.Run(() => { Send_otvet(s_client); });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { s.Close(); }
        }

        private void Send_otvet(Socket s_client)
        {
            byte[] buf = new byte[1024];
            try
            {
                int l = s_client.Receive(buf);
                if(l > 0)
                {
                    string mess = Encoding.Default.GetString(buf, 0, l);
                    s_client.Send(Encoding.Default.GetBytes(DateTime.Now.ToString() + " " + mess));
                }
                else
                {
                    s_client.Send(Encoding.Default.GetBytes(DateTime.Now.ToString()));
                }
                //s_client.Shutdown(SocketShutdown.Both);
            }
           catch (Exception e) { MessageBox.Show(e.Message); }
           //finally { s_client.Close(); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
