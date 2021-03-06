﻿namespace SiRFLive.Communication
{
    using System;

    public class SiRFNavParams
    {
        public uint BaudRate = 0x1c200;
        public uint CodeLoadBaudRate = 0x1c200;
        public byte DebugSettings = 1;
        public byte FlashMode;
        public byte FlowControl;
        public ushort I2CHostAddress = 0x62;
        public byte I2CMode;
        public byte I2CRate;
        public uint I2CTrackerAddress = 0x60;
        public ushort[] IOPinConfiguration = new ushort[11];
        public byte IOPinConfigurationMode;
        public byte LDOEnable;
        public byte LNAType;
        public int ONOffControl;
        public uint ReferenceClockFrequency = 0xf9c568;
        public int ReferenceClockOffset = 0x177fa;
        public uint ReferenceClockUncertainty = 0xbb8;
        public ushort ReferenceClockWarmupDelay = 0x3ff;
        public byte SPIRate = 5;
        public uint StartMode;
        public uint StopMode;
        public byte StorageMode = 1;
        public string TrackerPort = @"\\.\COM3";
        public byte TrackerPortSeleted = 1;
        public byte UARTIdleByteWakeupDelay;
        public byte UARTMaxPreamble;
        public double Version = 1.0;
        public int WeakSignalEnable = 1;
    }
}

