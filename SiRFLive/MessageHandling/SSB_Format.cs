﻿namespace SiRFLive.MessageHandling
{
    using System;

    internal class SSB_Format
    {
        private const byte ENABLE_DBG_MSG = 0x20;
        private const byte ENABLE_RAW_TRK = 0x10;
        private const byte ENABLE_SYSTEM_RESET = 0x80;
        private const byte RESET_COLD = 4;
        private const byte RESET_FACTORY = 8;
        private const byte RESET_FACTORY_XO = 0x48;
        private const byte RESET_HOT = 0;
        private const byte RESET_WARM_INIT = 3;
        private const byte RESET_WARM_NOINIT = 2;
        private const byte STARTUP_CLEAR_RAM = 4;
        private const byte STARTUP_EPH_INVALID = 2;
        private const byte STARTUP_USE_INIT = 1;

        internal SSB_Format()
        {
        }

        internal byte GetResetBitMap(string reset, bool enable_Navlib_Data, bool enable_Development_Data, bool enableFullSystemReset)
        {
            byte num = 0;
            switch (reset)
            {
                case "HOT":
                    num = 0;
                    break;

                case "WARM_INIT":
                    num = 3;
                    break;

                case "COLD":
                    num = 4;
                    break;

                case "FACTORY":
                    num = 8;
                    break;

                case "FACTORY_XO":
                    num = 0x48;
                    break;

                case "WARM_NO_INIT":
                    num = 2;
                    break;

                default:
                    num = 4;
                    break;
            }
            if (enable_Navlib_Data)
            {
                num = (byte) (num | 0x10);
            }
            if (enable_Development_Data)
            {
                num = (byte) (num | 0x20);
            }
            if (enableFullSystemReset)
            {
                num = (byte) (num | 0x80);
            }
            return num;
        }

        internal static string GetResetTypeFromBitMap(byte maskByte)
        {
            if ((maskByte & 15) == 0)
            {
                return "HOT";
            }
            if ((maskByte & 3) == 3)
            {
                return "WARM_INIT";
            }
            if ((maskByte & 4) == 4)
            {
                return "COLD";
            }
            if ((maskByte & 2) == 2)
            {
                return "WARM_NO_INIT";
            }
            if ((maskByte & 8) == 8)
            {
                return "FACTORY";
            }
            if ((maskByte & 0x48) == 0x48)
            {
                return "FACTORY_XO";
            }
            return "Unknown";
        }
    }
}

