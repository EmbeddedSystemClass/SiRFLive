﻿namespace SiRFLive.Communication
{
    using System;

    public class SignalData
    {
        public float[] CHAN_Arr_Azimuth = new float[MAX_CHAN];
        public float[] CHAN_Arr_Azimuth_All = new float[MAX_CHAN_ALL];
        public float[] CHAN_Arr_CNO = new float[MAX_CHAN];
        public float[] CHAN_Arr_CNO_All = new float[MAX_CHAN_ALL];
        public float[] CHAN_Arr_Elev = new float[MAX_CHAN];
        public float[] CHAN_Arr_Elev_All = new float[MAX_CHAN_ALL];
        public byte[] CHAN_Arr_ID = new byte[MAX_CHAN];
        public byte[] CHAN_Arr_ID_All = new byte[MAX_CHAN_ALL];
        public ushort[] CHAN_Arr_State = new ushort[MAX_CHAN];
        public ushort[] CHAN_Arr_State_All = new ushort[MAX_CHAN_ALL];
        public uint[] CHAN_Arr_Status = new uint[MAX_CHAN];
        public uint[] CHAN_Arr_Status_All = new uint[MAX_CHAN_ALL];
        public int[][] CHAN_MEAS_CNO;
        public ushort[] CHAN_SV_Info = new ushort[MAX_CHAN];
        private static int MAX_CHAN = 12;
        private static int MAX_CHAN_ALL = 60;

        public SignalData(int maxChan)
        {
            int[][] numArray = new int[12][];
            numArray[0] = new int[10];
            numArray[1] = new int[10];
            numArray[2] = new int[10];
            numArray[3] = new int[10];
            numArray[4] = new int[10];
            numArray[5] = new int[10];
            numArray[6] = new int[10];
            numArray[7] = new int[10];
            numArray[8] = new int[10];
            numArray[9] = new int[10];
            numArray[10] = new int[10];
            numArray[11] = new int[10];
            this.CHAN_MEAS_CNO = numArray;
            MAX_CHAN = maxChan;
        }
    }
}

