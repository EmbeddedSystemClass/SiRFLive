﻿namespace SiRFLive.Analysis
{
    using System;

    public class tSVD_SVState
    {
        public float[] acc = new float[3];
        public double af0;
        public double af1;
        public double af2;
        public int cct;
        public float clkVar;
        public double clockBias;
        public double clockDrift;
        public int dataAvail;
        public byte ephAge;
        public double gct;
        public short gcw;
        public byte iode;
        public float ionoDelayStd;
        public float[] jrk = new float[3];
        public double[] pos = new double[3];
        public float posVar;
        public float rcd;
        public sbyte slw;
        public byte svid;
        public float tcr;
        public double tgd;
        public int tow;
        public double[] vel = new double[3];
        public short wno;
    }
}

