﻿namespace OpenNETCF.IO.Serial
{
    using System;

    public class HandshakeCtsRts : DetailedPortSettings
    {
        protected override void Init()
        {
            base.Init();
            base.OutCTS = true;
            base.OutDSR = false;
            base.OutX = false;
            base.InX = false;
            base.RTSControl = RTSControlFlows.Handshake;
            base.DTRControl = DTRControlFlows.Enable;
            base.TxContinueOnXOff = true;
            base.DSRSensitive = false;
        }
    }
}

