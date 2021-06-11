using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terracotta.Packethandling.Serverbound;
using Terracotta.Packethandling.Serverbound.Handshake;
using Terracotta.Packethandling.Serverbound.Login;
using Terracotta.Packethandling.Serverbound.Play;
using Terracotta.Packethandling.Serverbound.Status;
using static Terracotta.Packethandling.DataHandler;

namespace Terracotta.Packethandling
{
    public static class PacketDictionary
    {
        public delegate void Packet(Guid clientID, byte[] pData);

        public static Dictionary<int, Packet> v1_17_0_Handshake { get; } = new Dictionary<int, Packet>()
        {
            { 0x00, Handshake.Handle },
        };
        public static Dictionary<int, Packet> v1_17_0_Status { get; } = new Dictionary<int, Packet>()
        {
            { 0x00, Request.Handle },
            { 0x01, Ping.Handle },
        };
        public static Dictionary<int, Packet> v1_17_0_Login { get; } = new Dictionary<int, Packet>()
        {
            { 0x00, LoginStart.Handle },
        };
        public static Dictionary<int, Packet> v1_17_0_Play { get; } = new Dictionary<int, Packet>()
        {
            { 0x00, TeleportConfirm.Handle },
        };
    }

    public enum State
    {
        Handshake,
        Status,
        Login,
        Play
    }

    public enum ProtocolVersion
    {
        v1_13_0 = 393,
        v1_13_2 = 404,
        v1_14_0 = 477,
        v1_14_1 = 480,
        v1_15_2 = 578,
        v1_16_3 = 753,
        v1_16_5 = 754,
        v1_17_0 = 755
    }
}
