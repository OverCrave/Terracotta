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
            DataHandler handler = new(pData);
            int clientProtocolVersion = handler.ReadVarInt();
            string endpoint = handler.ReadString();
            ushort port = handler.ReadUShort();
            int nextState = handler.ReadVarInt();
            Console.WriteLine("Client " + clientID + " requests to be put into state " + ((State)nextState).ToString());
            Server.I.clients[clientID - 1].clientState = (State)nextState;
        }
    }
}
