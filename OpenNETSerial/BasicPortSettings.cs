﻿namespace OpenNETCF.IO.Serial
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class BasicPortSettings
    {
        public BaudRates BaudRate = BaudRates.CBR_115200;
        public byte ByteSize = 8;
        public OpenNETCF.IO.Serial.Parity Parity;
        public OpenNETCF.IO.Serial.StopBits StopBits;
    }
}

