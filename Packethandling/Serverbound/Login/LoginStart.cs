using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Terracotta.Packethandling.Serverbound.Login
{
    class LoginStart
    {
        internal static void Handle(Guid clientID, byte[] pData)
        {
            DataHandler handler = new();
            handler.Write(pData);

            int packetLength = handler.ReadVarInt();
            int packetID = handler.ReadVarInt();
            string name = handler.ReadString();

            Console.WriteLine("Client " + clientID.ToString() + " username is " + name);

            //Encryption request to be sent
            //byte[] publicKey = Server.I.key;
            //byte[] verifyToken = new byte[4] { 1,2,3,4 };

            //DataHandler prehandler = new();
            //prehandler.WriteVarInt(0x01);
            //prehandler.Write(string.Empty);
            //prehandler.Write(publicKey.Length);
            //prehandler.Write(publicKey);
            //prehandler.Write(verifyToken.Length);
            //prehandler.Write(verifyToken);
            //byte[] rawPackage = prehandler.Buffer;
            //prehandler.Dispose();

            //DataHandler finalhandler = new();
            //finalhandler.WriteVarInt(rawPackage.Length);
            //finalhandler.Write(rawPackage);
            //byte[] finalPackage = finalhandler.Buffer;
            //finalhandler.Dispose();

            //Send login success
            DataHandler prehandler = new();
            prehandler.WriteVarInt(0x02);
            prehandler.Write(clientID.ToString());
            prehandler.Write(name);
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
        }
    }
}
