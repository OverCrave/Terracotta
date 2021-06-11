using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Terracotta.Packethandling.Serverbound.Status
{
    class Ping
    {
        internal static void Handle(Guid clientID, byte[] pData)
        {
            DataHandler handler = new();
            handler.Write(pData);

            int packetLength = handler.ReadVarInt();
            int packetID = handler.ReadVarInt();
            long payload = handler.ReadLong();

            DataHandler prehandler = new();
            prehandler.WriteVarInt(0x01);
            prehandler.Write(payload);
            byte[] rawPackage = prehandler.Buffer;
            prehandler.Dispose();

            DataHandler finalhandler = new();
            finalhandler.WriteVarInt(rawPackage.Length);
            finalhandler.Write(rawPackage);
            byte[] finalPackage = finalhandler.Buffer;
            finalhandler.Dispose();

            NetworkStream clientStream = Server.I.clients[clientID].stream;
            clientStream.BeginWrite(finalPackage, 0, finalPackage.Length, null, null);

            if (handler.ByteCountLeft > 0)
            {
                Server.I.clients[clientID].Handle(clientID, handler.Remaining);
            }

            handler.Dispose();

            //Disconnect after pong
            Server.I.clients[clientID].Dispose();
        }
    }
}
