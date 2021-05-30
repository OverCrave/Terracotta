﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Terracotta
{
    public class Server
    {
        private TcpListener listener;
        private IPEndPoint endpoint;

        internal void Start()
        {
            Console.WriteLine("Server starting!");

            byte[] ip = new byte[4] { 127, 0, 0, 1 };
            int port = 25565;
            endpoint = new IPEndPoint(new IPAddress(ip), port);
            listener = new TcpListener(endpoint);
            listener.Start();
            listener.BeginAcceptTcpClient(OnClientConnect, null);
        }

        private void OnClientConnect(IAsyncResult ar)
        {
            Console.WriteLine("Client connecting!");
            TcpClient newClient = (TcpClient)ar;
            listener.BeginAcceptTcpClient(OnClientConnect, null);
        }
    }
}