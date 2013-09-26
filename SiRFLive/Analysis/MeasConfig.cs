﻿namespace SiRFLive.Analysis
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct MeasConfig
    {
        public int measTimeUncertInMs;
        public int measPosUncertInMeter;
    }
}

