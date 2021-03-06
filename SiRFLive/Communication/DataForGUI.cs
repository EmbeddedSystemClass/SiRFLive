﻿namespace SiRFLive.Communication
{
    using SiRFLive.MessageHandling;
    using System;

    public class DataForGUI
    {
        private byte _calStatus;
        private double _headingDegrees;
        private ushort _headingUnc;
        private double _pitchDegrees;
        private ushort _pitchUnc;
        public byte _PMODE;
        private double _rollDegrees;
        private ushort _rollUnc;
        private ushort _sensorID;
        public int AGC_Gain;
        public byte dataReg;
        public int MAX_CHAN;
        public static int MAX_PRN = 0xff;
        public int MEMS_State;
        public byte numBytesReg;
        public PositionInfo Positions;
        public float[] PRN_Arr_Azimuth;
        public float[] PRN_Arr_CNO;
        public float[] PRN_Arr_Elev;
        public byte[] PRN_Arr_ID;
        public ushort[] PRN_Arr_Info;
        public int[] PRN_Arr_PRNforSolution;
        public ushort[] PRN_Arr_State;
        public uint[] PRN_Arr_Status;
        public bool[] PRN_Arr_UseCGEE;
        public bool[] PRN_Arr_UseSGEE;
        public ushort sensID;
        public SignalData SignalDataForGUI;
        public SignalData SignalDataForGUI_All;
        public DateTime TimeLastMsg30Rcvd;
        public PositionInfo TruePositions;
        public short XValue_Acc;
        public short XValue_Mag;
        public short YValue_Acc;
        public short YValue_Mag;
        public short ZValue_Acc;
        public short ZValue_Mag;

        public DataForGUI()
        {
            this.MAX_CHAN = 12;
            this.MEMS_State = -1;
            this.PRN_Arr_PRNforSolution = new int[MAX_PRN];
            this.PRN_Arr_CNO = new float[MAX_PRN];
            this.PRN_Arr_Azimuth = new float[MAX_PRN];
            this.PRN_Arr_Elev = new float[MAX_PRN];
            this.PRN_Arr_ID = new byte[MAX_PRN];
            this.PRN_Arr_State = new ushort[MAX_PRN];
            this.PRN_Arr_Info = new ushort[MAX_PRN];
            this.PRN_Arr_Status = new uint[MAX_PRN];
            this.PRN_Arr_UseCGEE = new bool[MAX_PRN];
            this.PRN_Arr_UseSGEE = new bool[MAX_PRN];
            this.SignalDataForGUI = new SignalData(12);
            this.SignalDataForGUI_All = new SignalData(60);
            this.Positions = new PositionInfo();
            this.TruePositions = new PositionInfo();
        }

        public DataForGUI(int maxChan)
        {
            this.MAX_CHAN = 12;
            this.MEMS_State = -1;
            this.PRN_Arr_PRNforSolution = new int[MAX_PRN];
            this.PRN_Arr_CNO = new float[MAX_PRN];
            this.PRN_Arr_Azimuth = new float[MAX_PRN];
            this.PRN_Arr_Elev = new float[MAX_PRN];
            this.PRN_Arr_ID = new byte[MAX_PRN];
            this.PRN_Arr_State = new ushort[MAX_PRN];
            this.PRN_Arr_Info = new ushort[MAX_PRN];
            this.PRN_Arr_Status = new uint[MAX_PRN];
            this.PRN_Arr_UseCGEE = new bool[MAX_PRN];
            this.PRN_Arr_UseSGEE = new bool[MAX_PRN];
            this.SignalDataForGUI = new SignalData(12);
            this.SignalDataForGUI_All = new SignalData(60);
            this.Positions = new PositionInfo();
            this.TruePositions = new PositionInfo();
            this.MAX_CHAN = maxChan;
        }

        public void ClearMsg30EphData()
        {
            for (int i = 0; i < MAX_PRN; i++)
            {
                this.PRN_Arr_UseCGEE[i] = false;
                this.PRN_Arr_UseSGEE[i] = false;
            }
        }

        ~DataForGUI()
        {
            this.SignalDataForGUI = null;
            this.SignalDataForGUI_All = null;
            this.Positions = null;
            this.TruePositions = null;
            this.PRN_Arr_PRNforSolution = null;
            this.PRN_Arr_Azimuth = null;
            this.PRN_Arr_CNO = null;
            this.PRN_Arr_Elev = null;
            this.PRN_Arr_ID = null;
            this.PRN_Arr_PRNforSolution = null;
            this.PRN_Arr_State = null;
            this.PRN_Arr_UseCGEE = null;
            this.PRN_Arr_UseSGEE = null;
        }

        public bool ResetMsg30EphFlagsIfTimeout()
        {
            TimeSpan span = (TimeSpan) (DateTime.Now - this.TimeLastMsg30Rcvd);
            if (span.Seconds > 3)
            {
                this.ClearMsg30EphData();
                return true;
            }
            return false;
        }

        public byte CalStatus
        {
            get
            {
                return this._calStatus;
            }
            set
            {
                this._calStatus = value;
            }
        }

        public double HeadingDegrees
        {
            get
            {
                return this._headingDegrees;
            }
            set
            {
                this._headingDegrees = value;
            }
        }

        public ushort HeadingUnc
        {
            get
            {
                return this._headingUnc;
            }
            set
            {
                this._headingUnc = value;
            }
        }

        public double PitchDegrees
        {
            get
            {
                return this._pitchDegrees;
            }
            set
            {
                this._pitchDegrees = value;
            }
        }

        public ushort PitchUnc
        {
            get
            {
                return this._pitchUnc;
            }
            set
            {
                this._pitchUnc = value;
            }
        }

        public double RollDegrees
        {
            get
            {
                return this._rollDegrees;
            }
            set
            {
                this._rollDegrees = value;
            }
        }

        public ushort RollUnc
        {
            get
            {
                return this._rollUnc;
            }
            set
            {
                this._rollUnc = value;
            }
        }

        public ushort SensorID
        {
            get
            {
                return this._sensorID;
            }
            set
            {
                this._sensorID = value;
            }
        }
    }
}

