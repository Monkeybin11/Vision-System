namespace OmronFins_TCP
{
    using System;

    internal class FinsClass
    {
        internal static byte[] FinsCmd(RorW rw, PlcMemory mr, MemoryType mt, short ch, short offset, short cnt)
        {
            byte[] buffer = new byte[0x22];
            buffer[0] = 70;
            buffer[1] = 0x49;
            buffer[2] = 0x4e;
            buffer[3] = 0x53;
            buffer[4] = 0;
            buffer[5] = 0;
            if (rw == RorW.Read)
            {
                buffer[6] = 0;
                buffer[7] = 0x1a;
            }
            else if (mt == MemoryType.Word)
            {
                buffer[6] = (byte) (((cnt * 2) + 0x1a) / 0x100);
                buffer[7] = (byte) (((cnt * 2) + 0x1a) % 0x100);
            }
            else
            {
                buffer[6] = 0;
                buffer[7] = 0x1b;
            }
            buffer[8] = 0;
            buffer[9] = 0;
            buffer[10] = 0;
            buffer[11] = 2;
            buffer[12] = 0;
            buffer[13] = 0;
            buffer[14] = 0;
            buffer[15] = 0;
            buffer[0x10] = 0x80;
            buffer[0x11] = 0;
            buffer[0x12] = 2;
            buffer[0x13] = 0;
            buffer[20] = BasicClass.plcNode;
            buffer[0x15] = 0;
            buffer[0x16] = 0;
            buffer[0x17] = BasicClass.pcNode;
            buffer[0x18] = 0;
            buffer[0x19] = 0xff;
            if (rw == RorW.Read)
            {
                buffer[0x1a] = 1;
                buffer[0x1b] = 1;
            }
            else
            {
                buffer[0x1a] = 1;
                buffer[0x1b] = 2;
            }
            buffer[0x1c] = GetMemoryCode(mr, mt);
            buffer[0x1d] = (byte) (ch / 0x100);
            buffer[30] = (byte) (ch % 0x100);
            buffer[0x1f] = (byte) offset;
            buffer[0x20] = (byte) (cnt / 0x100);
            buffer[0x21] = (byte) (cnt % 0x100);
            return buffer;
        }

        private static byte GetMemoryCode(PlcMemory mr, MemoryType mt)
        {
            if (mt == MemoryType.Bit)
            {
                switch (mr)
                {
                    case PlcMemory.CIO:
                        return 0x30;

                    case PlcMemory.WR:
                        return 0x31;

                    case PlcMemory.DM:
                        return 2;
                }
                return 0;
            }
            switch (mr)
            {
                case PlcMemory.CIO:
                    return 0xb0;

                case PlcMemory.WR:
                    return 0xb1;

                case PlcMemory.DM:
                    return 130;
            }
            return 0;
        }

        internal static byte[] HandShake()
        {
            return new byte[] { 
                70, 0x49, 0x4e, 0x53, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0
             };
        }
    }
}

