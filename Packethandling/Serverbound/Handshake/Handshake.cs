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
            State nextState = (State)handler.ReadVarInt();

            Server.I.clients[clientID].clientState = nextState;

            if (handler.ByteCountLeft > 0)
            {
                Server.I.clients[clientID].Handle(clientID, handler.Remaining);
            }

            handler.Dispose();
        }
    }
}
