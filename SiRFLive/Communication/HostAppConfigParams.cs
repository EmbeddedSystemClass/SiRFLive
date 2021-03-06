﻿namespace SiRFLive.Communication
{
    using System;

    public class HostAppConfigParams
    {
        public string BaudRate = "115200";
        public int DebugSettings = 1;
        public string DefaultTCXOFreq = string.Empty;
        public string ExtSResetNLineUsage = "uart_dtr";
        public string HostAppCfgFilePath = string.Empty;
        public string HostAppMEMSCfgPath = string.Empty;
        public string HostSWFilePath = string.Empty;
        public int LDOMode;
        public int LNAType;
        public string OnOffLineUsage = "uart_cts";
        public string ProgramFile = string.Empty;
        public string ResetPort = string.Empty;
        public string SiRFLiveInterfacePortName = string.Empty;
        public string TrackerPort = string.Empty;
        public string TrackerPortSelect = "uart";
        public int WarmupDelay = 0x3ff;
    }
}

