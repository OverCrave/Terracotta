using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Terracotta
{
    public class Client
    {
        private TcpClient socket;
        private IPEndPoint ip;
        internal NetworkStream stream;
        private byte[] recBuffer;
        internal int ID;

        internal Client(TcpClient client, int clientID)
        {
            ID = clientID;
            socket = client;
            ip = (IPEndPoint)socket.Client.RemoteEndPoint;
            Console.WriteLine("Client with EndPoint " + ip.ToString() + " and ID " + ID.ToString() + " connected!");
        }

        internal void Init()
        {
            int s = 4096;
            recBuffer = new byte[s];
            socket.SendBufferSize = s;
            socket.ReceiveBufferSize = s;
            stream = socket.GetStream();
            stream.BeginRead(recBuffer, 0, s, OnPacketReceived, null);
        }

        private void OnPacketReceived(IAsyncResult ar)
        {
            Console.WriteLine("Client " + ID.ToString() + " sent a packet!");

            try
            {
                int l = stream.EndRead(ar);
                if (l <= 0)
                {
                    Disconnect();
                    return;
                }

                byte[] temp = new byte[l];
                Array.Copy(recBuffer, temp, l);

                HandlePacket handler = new HandlePacket();
                handler.Handle(ID, temp);

                stream.BeginRead(recBuffer, 0, 4096, OnPacketReceived, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Disconnect();
            }
        }

        private void Disconnect()
        {
            Server.I.clients.Remove(this);
            socket.Close();
        }
    }
}
