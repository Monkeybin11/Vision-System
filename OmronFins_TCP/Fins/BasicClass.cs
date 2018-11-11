namespace OmronFins_TCP
{
    using System;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;

    internal class BasicClass
    {
        internal static TcpClient Client;
        internal static byte pcNode;
        internal static byte plcNode;
        internal static NetworkStream Stream;

        internal static bool PingCheck(string ip, int timeOut)
        {
            Ping ping = new Ping();
            return (ping.Send(ip, timeOut).Status == IPStatus.Success);
        }

        internal static short ReceiveData(byte[] rd)
        {
            try
            {
                int offset = 0;
                do
                {
                    int num2 = Stream.Read(rd, offset, rd.Length - offset);
                    if (num2 == 0)
                    {
                        return -1;
                    }
                    offset += num2;
                }
                while (offset < rd.Length);
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        internal static short SendData(byte[] sd)
        {
            try
            {
                Stream.Write(sd, 0, sd.Length);
                return 0;
            }
            catch
            {
                return -1;
            }
        }
    }
}

