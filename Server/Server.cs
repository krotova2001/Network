using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    //выделим сервер в отдельный класс
    internal class Server
    {
        IPAddress IPAddress;
        IPEndPoint endPoint;
        Socket socket;
        Socket s_client;
        public string data;
        public string state = "sleep"; // состояние работы сервера

        //в конструкторе по умолчанию сделаем общие действия для синхронной и асинхронной работы
        public Server()
        {
            IPAddress = IPAddress.Parse("127.0.0.1");
            endPoint = new IPEndPoint(IPAddress, 777);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public Server(IPAddress iP, IPEndPoint iPEnd)
        {
            IPAddress = iP;
            endPoint = iPEnd;
        }

        //запуск сервера
        public void Start()
        {
            Task task = Task.Run(() => { Connect(); });
        }

        private void Connect ()
        {
            try
            {
                socket.Bind(endPoint);
                socket.Listen(10);
                while (true)
                {
                    s_client = socket.Accept();
                    Task task = Task.Run(() => { Send_message("Connection opened\n"); });
                    Task task2 = Task.Run(() => { Auto_otvet(); });
                    if (s_client != null)
                        state = "Working";
                }
            }
            catch (Exception) {}
            finally { socket.Close(); state = "sleep"; }
        }

        //послать сообщение об ответе
        public void Send_message(string message)
        {
            if (s_client != null)
            {
                byte[] buf = System.Text.Encoding.Default.GetBytes(message);
                try
                {
                    
                    s_client.Send(buf, 0, buf.Length, SocketFlags.None);
                }
                catch (Exception) { }
            }
        }

        //логика общения
        private void Auto_otvet()
        {
            while (true)
            {
                if (s_client.Connected)
                {
                    byte[] buffer = new byte[4096];
                    int lenth = 0;
                    lenth = s_client.Receive(buffer);
                    data = Encoding.Default.GetString(buffer, 0, lenth);
                }
                if (data != null)
                {
                    switch (data)
                    {
                        case "d":
                            Send_message(DateTime.Now.ToString());
                            break;
                        case "b":
                            Send_message("Disconnected");
                            s_client.Close();
                            state = "sleep";
                            break;
                        //проверка как эхо-бота
                        default:
                            Index_worker index = new Index_worker(data);
                            string res = index.data;
                            Send_message(res);
                            break;
                    }
                }
            }
        }
    }
}
