namespace OmronFins_TCP
{
    using System;

    internal class ErrorCode
    {
        internal static bool CheckEndCode(byte Main, byte Sub)
        {
            byte num = Main;
            switch (num)
            {
                case 0x20:
                    switch (Sub)
                    {
                        case 2:
                            return false;

                        case 3:
                            return false;

                        case 4:
                            return false;

                        case 5:
                            return false;

                        case 6:
                            return false;

                        case 7:
                            return false;
                    }
                    break;

                case 0x21:
                    switch (Sub)
                    {
                        case 1:
                            return false;

                        case 2:
                            return false;

                        case 3:
                            return false;

                        case 5:
                            return false;

                        case 6:
                            return false;

                        case 7:
                            return false;

                        case 8:
                            return false;
                    }
                    break;

                case 0x22:
                    switch (Sub)
                    {
                        case 1:
                            return false;

                        case 2:
                            return false;

                        case 3:
                            return false;

                        case 4:
                            return false;

                        case 5:
                            return false;

                        case 6:
                            return false;

                        case 7:
                            return false;

                        case 8:
                            return false;
                    }
                    break;

                case 0x23:
                    switch (Sub)
                    {
                        case 1:
                            return false;

                        case 2:
                            return false;

                        case 3:
                            return false;
                    }
                    break;

                case 0x24:
                    num = Sub;
                    if (num != 1)
                    {
                        break;
                    }
                    return false;

                case 0x25:
                    switch (Sub)
                    {
                        case 2:
                            return false;

                        case 3:
                            return false;

                        case 4:
                            return false;

                        case 5:
                            return false;

                        case 6:
                            return false;

                        case 7:
                            return false;

                        case 9:
                            return false;

                        case 10:
                            return false;

                        case 13:
                            return false;

                        case 15:
                            return false;

                        case 0x10:
                            return false;
                    }
                    break;

                case 0x26:
                    switch (Sub)
                    {
                        case 1:
                            return false;

                        case 2:
                            return false;

                        case 4:
                            return false;

                        case 5:
                            return false;

                        case 6:
                            return false;

                        case 7:
                            return false;

                        case 8:
                            return false;

                        case 9:
                            return false;

                        case 10:
                            return false;

                        case 11:
                            return false;
                    }
                    break;

                case 0x30:
                    num = Sub;
                    if (num != 1)
                    {
                        break;
                    }
                    return false;

                case 0x40:
                    num = Sub;
                    if (num != 1)
                    {
                        break;
                    }
                    return false;

                case 0:
                    switch (Sub)
                    {
                        case 0:
                            return true;

                        case 1:
                            return false;
                    }
                    break;

                case 1:
                    switch (Sub)
                    {
                        case 1:
                            return false;

                        case 2:
                            return false;

                        case 3:
                            return false;

                        case 4:
                            return false;

                        case 5:
                            return false;

                        case 6:
                            return false;
                    }
                    break;

                case 2:
                    switch (Sub)
                    {
                        case 1:
                            return false;

                        case 2:
                            return false;

                        case 3:
                            return false;

                        case 4:
                            return false;

                        case 5:
                            return false;
                    }
                    break;

                case 3:
                    switch (Sub)
                    {
                        case 1:
                            return false;

                        case 2:
                            return false;

                        case 3:
                            return false;

                        case 4:
                            return false;
                    }
                    break;

                case 4:
                    switch (Sub)
                    {
                        case 1:
                            return false;

                        case 2:
                            return false;
                    }
                    break;

                case 5:
                    switch (Sub)
                    {
                        case 1:
                            return false;

                        case 2:
                            return false;

                        case 3:
                            return false;

                        case 4:
                            return false;
                    }
                    break;

                case 0x10:
                    switch (Sub)
                    {
                        case 1:
                            return false;

                        case 2:
                            return false;

                        case 3:
                            return false;

                        case 4:
                            return false;

                        case 5:
                            return false;
                    }
                    break;

                case 0x11:
                    switch (Sub)
                    {
                        case 1:
                            return false;

                        case 2:
                            return false;

                        case 3:
                            return false;

                        case 4:
                            return false;

                        case 6:
                            return false;

                        case 9:
                            return false;

                        case 10:
                            return false;

                        case 11:
                            return false;

                        case 12:
                            return false;
                    }
                    break;
            }
            return false;
        }

        internal static bool CheckHeadError(byte Code)
        {
            switch (Code)
            {
                case 0:
                    return true;

                case 1:
                    return false;

                case 2:
                    return false;

                case 3:
                    return false;
            }
            return false;
        }
    }
}

