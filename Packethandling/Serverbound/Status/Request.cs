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
            Console.WriteLine("Client " + clientID + " awaits our response...");

            ServerListResponse response = new()
            {
                version = new() { name = "TEST", protocol = 755 },
                players = new() { max = 199, online = 22, sample = new Sample[1] { new() { name = "OverCrave", id = "ece6e9d0-82c6-4484-bc60-229952b53f70" } } },
                description = new() { text = "sdgsfdhdfh" },
                favicon = "data:image/png;base64,<data>"
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

            Console.WriteLine("Sending response: \n" + jsonString);

            NetworkStream clientStream = Server.I.clients[clientID].stream;
            clientStream.BeginWrite(finalPackage, 0, finalPackage.Length, null, null);
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
