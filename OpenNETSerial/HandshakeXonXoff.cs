﻿namespace OpenNETCF.IO.Serial
{
    using System;

    public class HandshakeXonXoff : DetailedPortSettings
    {
        protected override void Init()
        {
            base.Init();
            base.OutCTS = false;
            base.OutDSR = false;
            base.OutX = true;
            base.InX = true;
            base.RTSControl = RTSControlFlows.Enable;
            base.DTRControl = DTRControlFlows.Enable;
            base.TxContinueOnXOff = true;
            base.DSRSensitive = false;
            base.XonChar = '\x0011';
            base.XoffChar = '\x0013';
        }
    }
}

