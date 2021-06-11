using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Terracotta.Packethandling;
using static Terracotta.Packethandling.PacketDictionary;

namespace Terracotta
{
    public class Client : IDisposable
    {
        private TcpClient socket;
        private IPEndPoint ip;
        internal NetworkStream stream;
        private byte[] recBuffer;
        internal Guid ID;
        public State clientState = State.Handshake;
        private int bSize = 2097151; //The maximum size a packet can have
        private bool disposedValue = false;

        internal Client(TcpClient client, Guid clientID)
        {
            ID = clientID;
            socket = client;
            ip = (IPEndPoint)socket.Client.RemoteEndPoint;
            Console.WriteLine("Client with EndPoint " + ip.ToString() + " and ID " + ID.ToString() + " connected!");
        }

        internal void Init()
        {
            recBuffer = new byte[bSize];
            socket.SendBufferSize = bSize;
            socket.ReceiveBufferSize = bSize;
            stream = socket.GetStream();
            stream.BeginRead(recBuffer, 0, bSize, OnPacketReceived, null);
        }

        private void OnPacketReceived(IAsyncResult ar)
        {
            try
            {
                int l = stream.EndRead(ar);

                if (l <= 0)
                {
                    Dispose();
                    return;
                }

                byte[] temp = new byte[l];
                Array.Copy(recBuffer, temp, l);
                recBuffer = new byte[bSize];

                Handle(ID, temp);

                if (stream == null)
                {
                    return;
                }

                stream.BeginRead(recBuffer, 0, bSize, OnPacketReceived, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Handle(Guid clientID, byte[] pData)
        {
            DataHandler handler = new();
            handler.Write(pData);

            int packetLength = handler.ReadVarInt();
            int packetID = handler.ReadVarInt();
            handler.Dispose();

            Console.WriteLine("Client " + ID.ToString() + " sent a " + clientState.ToString() + " packet with ID " + packetID + "!");

            switch (clientState)
            {
                case State.Handshake:
                    Server.I.handshakePackets.TryGetValue(packetID, out Packet hspacket);
                    if (hspacket != null)
                    {
                        hspacket.Invoke(clientID, pData);
                    }
                    break;
                case State.Status:
                    Server.I.statusPackets.TryGetValue(packetID, out Packet stpacket);
                    if (stpacket != null)
                    {
                        stpacket.Invoke(clientID, pData);
                    }
                    break;
                case State.Login:
                    Server.I.loginPackets.TryGetValue(packetID, out Packet lopacket);
                    if (lopacket != null)
                    {
                        lopacket.Invoke(clientID, pData);
                    }
                    break;
                case State.Play:
                    Server.I.playPackets.TryGetValue(packetID, out Packet plpacket);
                    if (plpacket != null)
                    {
                        plpacket.Invoke(clientID, pData);
                    }
                    break;
                default:
                    break;
            }
        }

        private void Disconnect()
        {
            Console.WriteLine("Client " + ID.ToString() + " disconnected");
            Server.I.clients.Remove(ID);
            stream.Close();
            stream = null;
            socket.Close();
            socket = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Disconnect();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
