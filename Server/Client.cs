using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Client
    {
        public string Name { get; set; }
        public Socket socket { get; set; }
      
        public Client(string n, Socket s) 
        { 
            Name = n;
            socket = s;
        }
    }
}
