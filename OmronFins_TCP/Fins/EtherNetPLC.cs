namespace OmronFins_TCP
{
    using System;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class EtherNetPLC
    {
        public EtherNetPLC()
        {
            BasicClass.Client = new TcpClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rIP"></param>
        /// <param name="rPort"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public short Link(string rIP, short rPort, short timeOut = 0xbb8)
        {
            if (BasicClass.PingCheck(rIP, timeOut))
            {
                BasicClass.Client.Connect(rIP, rPort);
                BasicClass.Stream = BasicClass.Client.GetStream();
                Thread.Sleep(10);
                if (BasicClass.SendData(FinsClass.HandShake()) != 0)
                {
                    return -1;
                }
                byte[] rd = new byte[0x18];
                if (BasicClass.ReceiveData(rd) != 0)
                {
                    return -1;
                }
                if (rd[15] != 0)
                {
                    return -1;
                }
                BasicClass.pcNode = rd[0x13];
                BasicClass.plcNode = rd[0x17];
                return 0;

            }
            return -1;
        }

        /// <summary>
        /// 关闭Fins TCP
        /// </summary>
        /// <returns></returns>
        public short Close()
        {
            try
            {
                BasicClass.Stream.Close();
                BasicClass.Client.Close();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 读取Bit位值
        /// </summary>
        /// <param name="mr">PLC 内存类型</param>
        /// <param name="ch"></param>
        /// <param name="bs"></param>
        /// <returns></returns>
        public short GetBitState(PlcMemory mr, string ch, out short bs)
        {
            bs = 0;
            byte[] rd = new byte[0x1f];
            short num = short.Parse(ch.Split(new char[] { '.' })[0]);
            short offset = short.Parse(ch.Split(new char[] { '.' })[1]);
            if (BasicClass.SendData(FinsClass.FinsCmd(RorW.Read, mr, MemoryType.Bit, num, offset, 1)) == 0)
            {
                if (BasicClass.ReceiveData(rd) == 0)
                {
                    bool flag = true;
                    if (rd[11] == 3)
                    {
                        flag = ErrorCode.CheckHeadError(rd[15]);
                    }
                    if (flag)
                    {
                        if (ErrorCode.CheckEndCode(rd[0x1c], rd[0x1d]))
                        {
                            bs = rd[30];
                            return 0;
                        }
                        return -1;
                    }
                }
                return -1;
            }
            return -1;
        }

        /// <summary>
        /// 写Bit位值
        /// </summary>
        /// <param name="mr"></param>
        /// <param name="ch"></param>
        /// <param name="bs"></param>
        /// <returns></returns>
        public short SetBitState(PlcMemory mr, string ch, BitState bs)
        {
            byte[] rd = new byte[30];
            short num = short.Parse(ch.Split(new char[] { '.' })[0]);
            short offset = short.Parse(ch.Split(new char[] { '.' })[1]);
            byte[] buffer2 = FinsClass.FinsCmd(RorW.Write, mr, MemoryType.Bit, num, offset, 1);
            byte[] array = new byte[0x23];
            buffer2.CopyTo(array, 0);
            array[0x22] = (byte)bs;
            if (BasicClass.SendData(array) == 0)
            {
                if (BasicClass.ReceiveData(rd) == 0)
                {
                    bool flag = true;
                    if (rd[11] == 3)
                    {
                        flag = ErrorCode.CheckHeadError(rd[15]);
                    }
                    if (flag)
                    {
                        if (ErrorCode.CheckEndCode(rd[0x1c], rd[0x1d]))
                        {
                            return 0;
                        }
                        return -1;
                    }
                }
                return -1;
            }
            return -1;
        }

        /// <summary>
        /// 从PLC指定地址读取Real类型数据值：浮点型
        /// </summary>
        /// <param name="mr">PLC内存类型</param>
        /// <param name="ch">内存地址</param>
        /// <param name="reData">读取的数据</param>
        /// <returns></returns>
        public short ReadReal(PlcMemory mr, short ch, out float reData)
        {
            reData = 0f;
            int num = 34;
            byte[] rd = new byte[num];
            if (BasicClass.SendData(FinsClass.FinsCmd(RorW.Read, mr, MemoryType.Word, ch, 0, 2)) == 0)
            {
                if (BasicClass.ReceiveData(rd) == 0)
                {
                    bool flag = true;
                    if (rd[11] == 3)
                    {
                        flag = ErrorCode.CheckHeadError(rd[15]);
                    }
                    if (flag)
                    {
                        if (ErrorCode.CheckEndCode(rd[0x1c], rd[0x1d]))
                        {
                            byte[] buffer3 = new byte[] { rd[31], rd[30], rd[33], rd[32] };
                            reData = BitConverter.ToSingle(buffer3, 0);
                            return 0;
                        }
                        return -1;
                    }
                }
                return -1;
            }
            return -1;
        }

        /// <summary>
        /// 向PLC指定地址写Real类型数据：浮点型
        /// </summary>
        /// <param name="mr">PLC内存类型</param>
        /// <param name="ch">内存地址</param>
        /// <param name="inData">要写入的数据</param>
        /// <returns></returns>
        public short WriteReal(PlcMemory mr, short ch, float inData)
        {
            byte[] rd = new byte[30];
            byte[] buffer2 = FinsClass.FinsCmd(RorW.Write, mr, MemoryType.Word, ch, 0, 2);
            byte[] buffer3 = new byte[4];

            byte[] bytes = BitConverter.GetBytes(inData);
            buffer3[0] = bytes[1];
            buffer3[1] = bytes[0];
            buffer3[2] = bytes[3];
            buffer3[3] = bytes[2];

            byte[] array = new byte[4 + 0x22];
            buffer2.CopyTo(array, 0);
            buffer3.CopyTo(array, 0x22);
            if (BasicClass.SendData(array) == 0)
            {
                if (BasicClass.ReceiveData(rd) == 0)
                {
                    bool flag = true;
                    if (rd[11] == 3)
                    {
                        flag = ErrorCode.CheckHeadError(rd[15]);
                    }
                    if (flag)
                    {
                        if (ErrorCode.CheckEndCode(rd[0x1c], rd[0x1d]))
                        {
                            return 0;
                        }
                        return -1;
                    }
                }
                return -1;
            }
            return -1;
        }

        /// <summary>
        /// 从PLC指定地址读一个字:整型
        /// </summary>
        /// <param name="mr">PLC内存类型</param>
        /// <param name="ch">内存地址</param>
        /// <param name="reData">读取的数据</param>
        /// <returns></returns>
        public short ReadWord(PlcMemory mr, short ch, out short reData)
        {
            short[] numArray;
            reData = 0;
            if (this.ReadWords(mr, ch, 1, out numArray) != 0)
            {
                return -1;
            }
            reData = numArray[0];
            return 0;
        }

        /// <summary>
        /// 从PLC指定地址读取多个字：整型
        /// </summary>
        /// <param name="mr">PLC内存类型</param>
        /// <param name="ch">内存地址</param>
        /// <param name="cnt">整型的个数</param>
        /// <param name="reData">读取的数据</param>
        /// <returns></returns>
        public short ReadWords(PlcMemory mr, short ch, short cnt, out short[] reData)
        {
            reData = new short[cnt];
            int num = 30 + (cnt * 2);
            byte[] rd = new byte[num];
            if (BasicClass.SendData(FinsClass.FinsCmd(RorW.Read, mr, MemoryType.Word, ch, 0, cnt)) == 0)
            {
                if (BasicClass.ReceiveData(rd) == 0)
                {
                    bool flag = true;
                    if (rd[11] == 3)
                    {
                        flag = ErrorCode.CheckHeadError(rd[15]);
                    }
                    if (!flag)
                    {
                        return -1;
                    }
                    if (ErrorCode.CheckEndCode(rd[0x1c], rd[0x1d]))
                    {
                        for (int i = 0; i < cnt; i++)
                        {
                            byte[] buffer3 = new byte[] { rd[(30 + (i * 2)) + 1], rd[30 + (i * 2)] };
                            reData[i] = BitConverter.ToInt16(buffer3, 0);
                        }
                        return 0;
                    }
                }
                return -1;
            }
            return -1;
        }

        /// <summary>
        /// 向PLC指定地址写入一个字：整型
        /// </summary>
        /// <param name="mr">PLC内存类型</param>
        /// <param name="ch">内存地址</param>
        /// <param name="inData">要写入的数据</param>
        /// <returns></returns>
        public short WriteWord(PlcMemory mr, short ch, short inData)
        {
            short[] numArray = new short[] { inData };
            if (this.WriteWords(mr, ch, 1, numArray) != 0)
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// 向PLC指定地址写入多个字：整型
        /// </summary>
        /// <param name="mr">PLC内存类型</param>
        /// <param name="ch">内存起始地址</param>
        /// <param name="cnt">整型个数</param>
        /// <param name="inData">要写入的整型数据</param>
        /// <returns></returns>
        public short WriteWords(PlcMemory mr, short ch, short cnt, short[] inData)
        {
            byte[] rd = new byte[30];
            byte[] buffer2 = FinsClass.FinsCmd(RorW.Write, mr, MemoryType.Word, ch, 0, cnt);
            byte[] buffer3 = new byte[cnt * 2];
            for (int i = 0; i < cnt; i++)
            {
                byte[] bytes = BitConverter.GetBytes(inData[i]);
                buffer3[i * 2] = bytes[1];
                buffer3[(i * 2) + 1] = bytes[0];
            }
            byte[] array = new byte[(cnt * 2) + 0x22];
            buffer2.CopyTo(array, 0);
            buffer3.CopyTo(array, 0x22);
            if (BasicClass.SendData(array) == 0)
            {
                if (BasicClass.ReceiveData(rd) == 0)
                {
                    bool flag = true;
                    if (rd[11] == 3)
                    {
                        flag = ErrorCode.CheckHeadError(rd[15]);
                    }
                    if (flag)
                    {
                        if (ErrorCode.CheckEndCode(rd[0x1c], rd[0x1d]))
                        {
                            return 0;
                        }
                        return -1;
                    }
                }
                return -1;
            }
            return -1;
        }

        public string PCNode
        {
            get
            {
                return BasicClass.pcNode.ToString();
            }
        }

        public string PLCNode
        {
            get
            {
                return BasicClass.plcNode.ToString();
            }
        }
        public bool FinsConnected
        {
            get
            {
                return BasicClass.Client.Connected;
            }
        }
    }
}

