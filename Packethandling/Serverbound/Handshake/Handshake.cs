using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Terracotta.Packethandling.PacketDictionary;

namespace Terracotta.Packethandling.Serverbound.Handshake
{
    class Handshake
    {
        internal static void Handle(Guid clientID, byte[] pData)
        {
            DataHandler handler = new();
            handler.Write(pData);

            int packetLength = handler.ReadVarInt();
            int packetID = handler.ReadVarInt();
            int clientProtocolVersion = handler.ReadVarInt();
            string endpoint = handler.ReadString();
            ushort port = handler.ReadUShort();
            int nextState = handler.ReadVarInt();

            handler.Dispose();

            Console.WriteLine("Client " + clientID + " requests to be put into state " + ((State)nextState).ToString());
            Server.I.clients[clientID].clientState = (State)nextState;

            //Send a response immediately without waiting for a request packet
            Server.I.statusPackets.TryGetValue(0x00, out Packet stpacket);
            if (stpacket != null)
            {
                stpacket.Invoke(clientID, null);
            }

            Server.I.clients[clientID].Dispose();
        }
    }
}
