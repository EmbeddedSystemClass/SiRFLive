﻿namespace SiRFLive.Analysis
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct SV_Infostruct
    {
        public byte sv_prn;
        public byte c_No;
        public byte inv_weights;
    }
}

