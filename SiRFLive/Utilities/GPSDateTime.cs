﻿namespace SiRFLive.Utilities
{
    using System;

    public class GPSDateTime
    {
        private DateTime d = DateTime.Now;
        private double gpsTOW;
        private int gpsWeek;
        private int UTCOffset = 14;

        public GPSDateTime()
        {
            this.UpdateGPSWeekTow(this.d);
        }

        private int GetDayOfWeekNumber(string day)
        {
            switch (day)
            {
                case "Sunday":
                    return 0;

                case "Monday":
                    return 1;

                case "Tuesday":
                    return 2;

                case "Wednesday":
                    return 3;

                case "Thursday":
                    return 4;

                case "Friday":
                    return 5;

                case "Saturday":
                    return 6;
            }
            return 0;
        }

        public double GetGPSTOW()
        {
            return this.gpsTOW;
        }

        public int GetGPSWeek()
        {
            return this.gpsWeek;
        }

        public DateTime GetTime()
        {
            return this.d;
        }

        public void SetTime(DateTime inTime)
        {
            this.d = inTime;
            this.UpdateGPSWeekTow(this.d);
        }

        public void SetTime(int inWeek, double inTOW)
        {
            this.gpsWeek = inWeek;
            this.gpsTOW = inTOW;
            this.UpdateDateTime(inWeek, inTOW);
        }

        public void SetUTCOffset(int inUTCOffset)
        {
            this.UTCOffset = inUTCOffset;
            this.UpdateGPSWeekTow(this.d);
        }

        private void UpdateDateTime(int inGPSWeek, double inGPSTOW)
        {
            DateTime time = new DateTime(0x7bc, 1, 6);
            this.d = time;
            this.d = this.d.AddDays((double) (inGPSWeek * 7));
            this.d = this.d.AddSeconds(inGPSTOW - this.UTCOffset);
        }

        private void UpdateGPSWeekTow(DateTime inDate)
        {
            DateTime time = new DateTime(0x7bc, 1, 6);
            TimeSpan span = (TimeSpan) (inDate - time);
            this.gpsWeek = span.Days / 7;
            this.gpsTOW = (span.TotalSeconds - ((this.gpsWeek * 7) * 0x15180)) + this.UTCOffset;
        }
    }
}

