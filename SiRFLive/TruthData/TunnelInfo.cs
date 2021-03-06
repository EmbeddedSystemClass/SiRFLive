﻿namespace SiRFLive.TruthData
{
    using System;

    public class TunnelInfo
    {
        public int enter_tow;
        public int exit_tow;

        public bool InsideTunnel(int gpsTOW)
        {
            return ((gpsTOW >= this.enter_tow) && (gpsTOW <= this.exit_tow));
        }

        public bool RecentTunnelExit(int gpsTOW, int maxTimeSinceExit)
        {
            return ((gpsTOW >= this.exit_tow) && (gpsTOW <= (this.exit_tow + maxTimeSinceExit)));
        }
    }
}

