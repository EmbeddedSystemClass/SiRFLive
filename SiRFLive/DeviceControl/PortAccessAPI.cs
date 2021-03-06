﻿namespace SiRFLive.DeviceControl
{
    using System;
    using System.Runtime.InteropServices;

    public class PortAccessAPI
    {
        [DllImport("inpout32.dll", EntryPoint="Inp32")]
        public static extern int Input(int address);
        [DllImport("inpout32.dll", EntryPoint="Out32")]
        public static extern void Output(int address, int value);
    }
}

