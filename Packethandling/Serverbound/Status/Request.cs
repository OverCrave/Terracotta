using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Terracotta.Packethandling.Serverbound.Status
{
    class Request
    {
        internal static void Handle(int clientID, byte[] pData)
        {
            Console.WriteLine("Client " + clientID + " awaits our response...");

            ServerListResponse response = new()
            {
                version = new Version(),
                players = new Players(),
                description = new Description(),
                favicon = new Favicon()
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(response, options);

            //Prepare response packet...
            int responseID = 0x00;
            byte[] jsonData = Encoding.UTF8.GetBytes(jsonString);
            int stringLength = jsonData.Length;
            

            //DataHandler handler = new DataHandler();

            throw new NotImplementedException();
        }

        private class ServerListResponse
        {
            public Version version;
            public Players players;
            public Description description;
            public Favicon favicon;
        }

        private class Version
        {
            public string name = "1.17.0";
            public int protocol = 755;
        }

        private class Players
        {
            public int max = 100;
            public int online = 5;
            public Sample[] sample = new Sample[1] { new Sample() };
        }

        private class Sample
        {
            public string name = "OverCrave";
            public string id = "ece6e9d0-82c6-4484-bc60-229952b53f70";
        }

        private class Description
        {
            public string text = "Terracotta Pre-Alpha";
        }

        private class Favicon
        {
            public string favicon = "data:image/png;base64,<data>";
        }
    }
}
