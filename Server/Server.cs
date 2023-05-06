using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Server
{
    //выделим сервер в отдельный класс
    internal class Server
    {
        IPAddress IPAddress;
        IPEndPoint endPoint;
        Socket socket;
        public string messages;
        public string state = "sleep"; // состояние работы сервера
        public List<Client> clients;

        //в конструкторе по умолчанию сделаем общие действия для синхронной и асинхронной работы
        public Server()
        {
            IPAddress = IPAddress.Parse("127.0.0.1");
            endPoint = new IPEndPoint(IPAddress, 777);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clients = new List<Client>();
        }


        //запуск сервера
        public void Start()
        {
            Task task = Task.Run(() => { TurnOn(); });
        }

        private void TurnOn()
        {
            try
            {
                socket.Bind(endPoint);
                socket.Listen(10);
                while (true)
                {
                    Socket s_client = socket.Accept();
                    Registration(s_client);
                    Task task2 = Task.Run(() => { Auto_otvet(); });
                    if (s_client != null)
                        state = "Working";
                }
            }
            catch (Exception) {}
            finally { socket.Close(); state = "sleep"; }
        }

        //послать сообщение об ответе
        public void Registration(Socket s)
        { 
            {
                try
                {
                    byte[] res = new byte[1024];
                    NetworkStream sr = new NetworkStream(s);
                    int l = sr.Read(res, 0, res.Length);
                    if (res != null)
                    {
                        string name = Encoding.UTF8.GetString(res, 0, res.Length);
                        clients.Add(new Client(name, s));
                    }
                }
                catch (Exception) { }
            }
        }

        public void Send_message_chat(string message, Client sender)
        {
            foreach (var client in clients)
            {
                byte[] buf = System.Text.Encoding.Default.GetBytes(message);
                try
                {
                   // if (client != sender)
                        client.socket.Send(buf, 0, buf.Length, SocketFlags.None);
                }
                catch (Exception) { }
            }
        }

        private void Auto_otvet()
        {
            while (true)
            {
                foreach (var client in clients)
                {
                    {
                        NetworkStream sr = new NetworkStream(client.socket);
                        byte[] buffer = new byte[1096];
                        int lenth = sr.Read(buffer, 0, buffer.Length);
                        if (buffer != null)
                        {
                            string message = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                            messages = message;
                            Send_message_chat($"{client.Name} - {messages}", client);
                        }
                    }
                }
            }
        }
    }
}
