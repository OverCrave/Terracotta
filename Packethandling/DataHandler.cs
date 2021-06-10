using System;
using System.Collections.Generic;
using System.Text;
using Terracotta.Packethandling.Serverbound;

namespace Terracotta.Packethandling
{
    public class DataHandler : IDisposable
    {
        private List<byte> mainBuffer;
        private byte[] readBuffer;
        private int readPos;
        private bool bufferUpdated;
        private bool disposedValue = false;

        public DataHandler()
        {
            mainBuffer = new List<byte>();
            readPos = 0;
            bufferUpdated = false;
        }

        public int ReadPos 
        { 
            get 
            {
                return readPos;
            }
            private set 
            {
                readPos = value;
            } 
        }
        public byte[] Buffer 
        { 
            get 
            { 
                return mainBuffer.ToArray(); 
            } 
        }
        public int ByteCount 
        { 
            get 
            { 
                return mainBuffer.Count; 
            } 
        }
        public int ByteCountLeft
        {
            get
            {
                return mainBuffer.Count - readPos;
            }
        }

        public void Clear()
        {
            mainBuffer.Clear();
            readPos = 0;
        }

        public void Write(bool input)
        {
            mainBuffer.AddRange(BitConverter.GetBytes(input));
            bufferUpdated = true;
        }

        public void Write(byte input)
        {
            mainBuffer.Add(input);
            bufferUpdated = true;
        }

        public void Write(byte[] input)
        {
            mainBuffer.AddRange(input);
            bufferUpdated = true;
        }

        public void Write(short input)
        {
            mainBuffer.AddRange(BitConverter.GetBytes(input));
            bufferUpdated = true;
        }

        public void Write(ushort input)
        {
            mainBuffer.AddRange(BitConverter.GetBytes(input));
            bufferUpdated = true;
        }

        public void Write(int input)
        {
            mainBuffer.AddRange(BitConverter.GetBytes(input));
            bufferUpdated = true;
        }

        public void Write(long input)
        {
            mainBuffer.AddRange(BitConverter.GetBytes(input));
            bufferUpdated = true;
        }

        public void Write(float input)
        {
            mainBuffer.AddRange(BitConverter.GetBytes(input));
            bufferUpdated = true;
        }

        public void Write(double input)
        {
            mainBuffer.AddRange(BitConverter.GetBytes(input));
            bufferUpdated = true;
        }

        public void Write(string input)
        {
            byte[] stringData = Encoding.UTF8.GetBytes(input);
            WriteVarInt(stringData.Length);
            mainBuffer.AddRange(stringData);
            bufferUpdated = true;
        }

        public void WriteVarInt(int input)
        {
            do
            {
                byte temp = (byte)(input & 0b01111111);
                input >>= 7;
                if (input != 0)
                {
                    temp |= 0b10000000;
                }
                Write(temp);
            } while (input != 0);
            bufferUpdated = true;
        }

        public void WriteVarLong(long input)
        {
            do
            {
                byte temp = (byte)(input & 0b01111111);
                input >>= 7;
                if (input != 0)
                {
                    temp |= 0b10000000;
                }
                Write(temp);
            } while (input != 0);
            bufferUpdated = true;
        }

        public bool ReadBool(bool Peek = true)
        {
            if (ByteCount > ReadPos)
            {
                if (bufferUpdated)
                {
                    readBuffer = Buffer;
                    bufferUpdated = false;
                }

                bool val = BitConverter.ToBoolean(ReadBytes(1, Peek));
                return val;
            }
            else
            {
                throw new Exception("Not a BOOL");
            }
        }

        public byte ReadByte(bool Peek = true)
        {
            if (ByteCount > ReadPos)
            {
                if (bufferUpdated)
                {
                    readBuffer = Buffer;
                    bufferUpdated = false;
                }

                byte val = readBuffer[ReadPos];

                if (Peek)
                {
                    ReadPos++;
                }

                return val;
            }
            else
            {
                throw new Exception("Not a BYTE");
            }
        }

        public byte[] ReadBytes(int length, bool Peek = true)
        {
            if (ByteCount > ReadPos)
            {
                if (bufferUpdated)
                {
                    readBuffer = Buffer;
                    bufferUpdated = false;
                }
                
                byte[] val = mainBuffer.GetRange(ReadPos, length).ToArray();

                if (Peek)
                {
                    ReadPos += length;
                }

                return val;
            }
            else
            {
                throw new Exception("Not a BYTE[]");
            }
        }

        public short ReadShort(bool Peek = true)
        {
            if (ByteCount > ReadPos)
            {
                if (bufferUpdated)
                {
                    readBuffer = Buffer;
                    bufferUpdated = false;
                }

                short val = BitConverter.ToInt16(ReadBytes(2, Peek));
                return val;
            }
            else
            {
                throw new Exception("Not a SHORT");
            }
        }

        public ushort ReadUShort(bool Peek = true)
        {
            if (ByteCount > ReadPos)
            {
                if (bufferUpdated)
                {
                    readBuffer = Buffer;
                    bufferUpdated = false;
                }

                ushort val = BitConverter.ToUInt16(ReadBytes(2, Peek));
                return val;
            }
            else
            {
                throw new Exception("Not a USHORT");
            }
        }

        public int ReadInt(bool Peek = true)
        {
            if (ByteCount > ReadPos)
            {
                if (bufferUpdated)
                {
                    readBuffer = Buffer;
                    bufferUpdated = false;
                }

                int val = BitConverter.ToInt32(ReadBytes(4, Peek));
                return val;
            }
            else
            {
                throw new Exception("Not an INT");
            }
        }

        public long ReadLong(bool Peek = true)
        {
            if (ByteCount > ReadPos)
            {
                if (bufferUpdated)
                {
                    readBuffer = Buffer;
                    bufferUpdated = false;
                }

                long val = BitConverter.ToInt64(ReadBytes(8, Peek));
                return val;
            }
            else
            {
                throw new Exception("Not a LONG");
            }
        }

        public float ReadFloat(bool Peek = true)
        {
            if (ByteCount > ReadPos)
            {
                if (bufferUpdated)
                {
                    readBuffer = Buffer;
                    bufferUpdated = false;
                }

                float val = BitConverter.ToSingle(ReadBytes(4, Peek));
                return val;
            }
            else
            {
                throw new Exception("Not a FLOAT");
            }
        }

        public double ReadDouble(bool Peek = true)
        {
            if (ByteCount > ReadPos)
            {
                if (bufferUpdated)
                {
                    readBuffer = Buffer;
                    bufferUpdated = false;
                }

                double val = BitConverter.ToDouble(ReadBytes(8, Peek));
                return val;
            }
            else
            {
                throw new Exception("Not a DOUBLE");
            }
        }

        public string ReadString(bool Peek = true)
        {
            if (ByteCount > ReadPos)
            {
                if (bufferUpdated)
                {
                    readBuffer = Buffer;
                    bufferUpdated = false;
                }

                int stringLength = ReadVarInt(Peek);
                string val = Encoding.UTF8.GetString(ReadBytes(stringLength, Peek));
                return val;
            }
            else
            {
                throw new Exception("Not a STRING");
            }
        }

        public int ReadVarInt(bool Peek = true)
        {
            int numRead = 0;
            int result = 0;
            byte read;
            do
            {
                read = ReadByte(Peek);
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

        public long ReadVarLong(bool Peek = true)
        {
            int numRead = 0;
            long result = 0;
            byte read;
            do
            {
                read = ReadByte(Peek);
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Clear();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
