﻿namespace SiRFLive.Reporting
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PositionInLatLonAlt
    {
        public string name;
        public double latitude;
        public double longitude;
        public double altitude;
    }
}

