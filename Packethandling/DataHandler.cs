using System;
using System.Collections.Generic;
using System.Text;
using Terracotta.Packethandling.Serverbound;

namespace Terracotta.Packethandling
{
    public class DataHandler : IDisposable
    {
        private byte[] data;
        private int readPos;

        public DataHandler(byte[] initialData)
        {
            data = initialData;
            readPos = 0;
        }

        public byte[] GetRest()
        {
            if(readPos == data.Length)
            {
                Dispose();
                return data;
            }

            byte[] temp = new byte[data.Length - readPos];
            Array.Copy(data, readPos, temp, 0, temp.Length);
            return temp;
        }

        public byte ReadByte()
        {
            try
            {
                byte b = data[readPos];
                readPos++;
                return b;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int ReadVarInt()
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

        public long ReadVarLong()
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

        public string ReadString()
        {
            int stringLength = ReadVarInt();
            readPos += stringLength;
            return Encoding.UTF8.GetString(data, readPos - stringLength, stringLength);
        }

        public ushort ReadUShort()
        {
            readPos += 2;
            return BitConverter.ToUInt16(data, readPos - 2);
        }

        public void Dispose()
        {
            readPos = 0;
            data = new byte[0];
        }
    }
}
