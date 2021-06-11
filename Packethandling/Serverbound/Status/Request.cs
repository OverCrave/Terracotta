using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Terracotta.Packethandling.Serverbound.Status
{
    class Request
    {
        internal static void Handle(Guid clientID, byte[] pData)
        {
            DataHandler handler = new();
            handler.Write(pData);

            int packetLength = handler.ReadVarInt();
            int packetID = handler.ReadVarInt();

            string defaultFavicon = "data:image/png;base64,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";

            ServerListResponse response = new()
            {
                version = new()
                {
                    name = "1.17",
                    protocol = 755
                },
                players = new()
                {
                    max = 64,
                    online = 0,
                    sample = new Sample[1]
                    {
                        new()
                        {
                            name = "OverCrave",
                            id = "ece6e9d0-82c6-4484-bc60-229952b53f70"
                        }
                    }
                },
                description = new()
                {
                    text = "A Terracotta Test Server!"
                },
                favicon = defaultFavicon
            };

            JsonSerializerOptions options = new() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(response, options);
            int responseID = 0x00;
            
            DataHandler prehandler = new();
            prehandler.WriteVarInt(responseID);
            prehandler.Write(jsonString);
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

        private class ServerListResponse
        {
            public Version version { get; set; }
            public Players players { get; set; }
            public Description description { get; set; }
            public string favicon { get; set; }
        }

        private class Version
        {
            public string name { get; set; }
            public int protocol { get; set; }
        }

        private class Players
        {
            public int max { get; set; }
            public int online { get; set; }
            public Sample[] sample { get; set; }
        }

        private class Sample
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        private class Description
        {
            public string text { get; set; }
        }
    }
}
