﻿namespace SiRFLive.Analysis
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct almDatastruct
    {
        public byte subAlmSVPrn;
        public ushort subAlmWeekNum;
        public byte subAlmToa;
    }
}

