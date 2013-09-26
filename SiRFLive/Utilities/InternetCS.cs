﻿namespace SiRFLive.Utilities
{
    using System;
    using System.Runtime.InteropServices;

    public class InternetCS
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(int Description, int ReservedValue);
        public static bool IsConnectedToInternet()
        {
            int description = 0;
            return InternetGetConnectedState(description, 0);
        }
    }
}

