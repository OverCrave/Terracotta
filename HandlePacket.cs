using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terracotta
{
    public class HandlePacket
    {
        private byte[] data;
        private int readPos;

        public void InitPackets()
        {

        }

        public void Handle(int conID, byte[] temp)
        {
            data = temp;
            readPos = 0;

            int packetLength = ReadVarInt();
            int packetID = ReadVarInt();

            Console.WriteLine("Packet length: " + packetLength);
            Console.WriteLine("Packet ID: " + packetID);

            if (packetID == 0)
            {
                //This is a Handshake..
                int clientProtocolVersion = ReadVarInt();
                Console.WriteLine("Protocol Version: " + clientProtocolVersion);
                //We know a string comes next..
                int stringLength = ReadVarInt();
                string endpoint = Encoding.UTF8.GetString(data, readPos, stringLength);
                readPos += stringLength;
                ushort port = BitConverter.ToUInt16(data, readPos);
                readPos += 2;
                Console.WriteLine("Endpoint: " + endpoint + ":" + port);
                int nextState = ReadVarInt();
                Console.WriteLine("Requested State: " + nextState);
            }
        }

        private byte ReadByte()
        {
            byte b = data[readPos];
            readPos++;
            return b;
        }

        private int ReadVarInt()
        {
            int numRead = 0;
            int result = 0;
            byte read;
            do
            {
                read = ReadByte();
                int value = (read & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5)
                {
                    throw new Exception("VarInt is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }

        private long ReadVarLong()
        {
            int numRead = 0;
            long result = 0;
            byte read;
            do
            {
                read = ReadByte();
                long value = (read & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 10)
                {
                    throw new Exception("VarLong is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }
    }
}
