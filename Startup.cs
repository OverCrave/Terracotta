using System;

namespace Terracotta
{
    public class Startup
    {
        private static bool serverRunning;

        public static void Main(string[] args)
        {
            serverRunning = true;

            Server server = new Server();
            server.Start();

            while (serverRunning)
            {
                string s = Console.ReadLine();
                if (s == "stop")
                {
                    serverRunning = false;
                }
            }
        }
    }
}
