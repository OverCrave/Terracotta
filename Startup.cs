using System;

namespace Terracotta
{
    public class Startup
    {
        private static bool serverRunning;

        public static void Main(string[] args)
        {
            if (args is null)
            {
                Console.WriteLine("No args given..");
            }

            serverRunning = true;

            Server server = new();
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
