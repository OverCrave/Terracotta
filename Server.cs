using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Terracotta.Packethandling;
using static Terracotta.Packethandling.PacketDictionary;

namespace Terracotta
{
    public class Server
    {
        private TcpListener listener;
        private IPEndPoint endpoint;
        public Dictionary<Guid, Client> clients;

        public Dictionary<int, Packet> handshakePackets;
        public Dictionary<int, Packet> statusPackets;
        public Dictionary<int, Packet> loginPackets;
        public Dictionary<int, Packet> playPackets;

        public ProtocolVersion targetVersion;

        public static Server I { get; set; }

        internal void Start()
        {
            I = this;

            targetVersion = ProtocolVersion.v1_17_0;

            switch (targetVersion)
            {
                case ProtocolVersion.v1_13_0:
                    break;
                case ProtocolVersion.v1_13_2:
                    break;
                case ProtocolVersion.v1_14_0:
                    break;
                case ProtocolVersion.v1_14_1:
                    break;
                case ProtocolVersion.v1_15_2:
                    break;
                case ProtocolVersion.v1_16_3:
                    break;
                case ProtocolVersion.v1_16_5:
                    break;
                case ProtocolVersion.v1_17_0:
                    handshakePackets = v1_17_0_Handshake;
                    statusPackets = v1_17_0_Status;
                    loginPackets = v1_17_0_Login;
                    playPackets = v1_17_0_Play;
                    break;
                default:
                    break;
            }

            clients = new();

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
            TcpClient newClient = listener.EndAcceptTcpClient(ar);
            listener.BeginAcceptTcpClient(OnClientConnect, null);
            Guid clientID = Guid.NewGuid();
            Client c = new(newClient, clientID);
            clients.Add(clientID, c);
            c.Init();
        }
    }
}
