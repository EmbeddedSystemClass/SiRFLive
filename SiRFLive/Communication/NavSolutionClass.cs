﻿namespace SiRFLive.Communication
{
    using System;

    public class NavSolutionClass
    {
        public double Alt;
        public uint ClockBias;
        public uint ClockDrift;
        public int ECEFX;
        public int ECEFY;
        public int ECEFZ;
        public uint EstGPSTime;
        public double Lat;
        public double Lon;
        public byte SVsUsed;
        public uint TOW;
        public int WeekNumber;
    }
}

