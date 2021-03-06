﻿namespace SiRFLive.Communication
{
    using System;
    using System.Windows.Forms;

    public class TrackSVRec
    {
        public static int MAX_SVT = 0x24;
        private int numSVs;
        public int[] SVIDs = new int[MAX_SVT];

        public int getIdx(int prn)
        {
            int index = 0;
            while (((this.SVIDs[index] != prn) && (this.SVIDs[index] != 0)) && (index < (MAX_SVT - 1)))
            {
                index++;
            }
            if (index == MAX_SVT)
            {
                MessageBox.Show("Number of SVs are over Array limit of 36");
                return -1;
            }
            if (this.SVIDs[index] == 0)
            {
                this.SVIDs[index] = prn;
                this.numSVs++;
                return index;
            }
            return index;
        }

        public int getIdx_readonly(int prn)
        {
            int index = 0;
            while (((this.SVIDs[index] != prn) && (this.SVIDs[index] != 0)) && (index < (MAX_SVT - 1)))
            {
                index++;
            }
            if (this.SVIDs[index] == prn)
            {
                return index;
            }
            return -1;
        }

        public int getNumTrackedSVs()
        {
            return this.numSVs;
        }
    }
}

