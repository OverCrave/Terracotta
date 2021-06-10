using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terracotta.Packethandling.Serverbound.Handshake
{
    class Handshake
    {
        internal static void Handle(int clientID, byte[] pData)
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
            Server.I.clients[clientID - 1].clientState = (State)nextState;
        }
    }
}
