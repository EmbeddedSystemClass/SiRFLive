﻿namespace SiRFLive.Communication
{
    using System;

    public class SiRFawareStatsParams
    {
        public byte AlmID;
        public uint CumulateTimeInFullPower;
        public byte IsBitSynch;
        public byte IsFrameSynch;
        public byte IsNav;
        public byte IsSuccessAlmCollection;
        public bool isValid_AlmID;
        public bool isValid_IsBitSynch;
        public bool isValid_IsFrameSynch;
        public bool isValid_IsNav;
        public bool isValid_IsSuccessAlmCollection;
        public bool isValid_MeanCodePhaseCorrection;
        public bool isValid_MeanDopplerResidual;
        public bool isValid_RTCCorrectionPerform;
        public bool isValid_RTCWakeupUncUs;
        public bool isValid_StdDeltaRanges;
        public bool isValid_StdPseudoRanges;
        public bool isValid_SVAfterEphCollection;
        public bool isValid_SVBeforeEphCollection;
        public bool isValid_TempRecord;
        public bool isValid_TimeSpentInFullPowerSec;
        public bool isValid_TotalSVMeasureWithAlm;
        public bool isValid_TotalSVMeasureWithBE;
        public bool isValid_TotalSVMeasureWithEE;
        public bool isValid_TotalTimeCorrection;
        public bool isValid_TOW;
        public bool isValid_uNavTimeCorrection;
        public bool isValid_UnusedTokenLeft;
        public int MeanCodePhaseCorrection;
        public int MeanDopplerResidual;
        public ushort RTCCorrectionPerform;
        public ushort RTCWakeupUncUs;
        public int StdDeltaRanges;
        public int StdPseudoRanges;
        public uint SVAfterEphCollection;
        public uint SVBeforeEphCollection;
        public double TempRecordC;
        public byte TempRecordT;
        public ushort TimeSpentInFullPowerSec;
        public byte TotalSVMeasureWithAlm;
        public byte TotalSVMeasureWithBE;
        public byte TotalSVMeasureWithEE;
        public int TotalTimeCorrection;
        public double TotalTimeInMPMSec;
        public uint TOW;
        public double TTFF;
        public short uNavTimeCorrection;
        public byte UnusedTokenLeft;
        public byte UpdateType;
    }
}

