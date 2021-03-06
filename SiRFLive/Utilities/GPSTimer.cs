﻿namespace SiRFLive.Utilities
{
    using System;

    public class GPSTimer
    {
        private TimeSpan timeOffset = new TimeSpan(0L);

        public GPSDateTime GetTime()
        {
            GPSDateTime time = new GPSDateTime();
            time.SetTime(DateTime.Now + this.timeOffset);
            return time;
        }

        public void SetTime(GPSDateTime inTime)
        {
            this.timeOffset = (TimeSpan) (inTime.GetTime() - DateTime.Now);
        }
    }
}

