﻿namespace OpenNETCF.IO.Serial
{
    using System;

    public class HandshakeNone : DetailedPortSettings
    {
        protected override void Init()
        {
            base.Init();
            base.OutCTS = false;
            base.OutDSR = false;
            base.OutX = false;
            base.InX = false;
            base.RTSControl = RTSControlFlows.Disable;
            base.DTRControl = DTRControlFlows.Enable;
            base.TxContinueOnXOff = true;
            base.DSRSensitive = false;
        }
    }
}

