﻿namespace SiRFLive.Utilities
{
    using System;
    using System.Timers;

    public class AppTimers
    {
        private Timer _appTimer;
        private int _appTimerInterval;
        public SiRFLiveEvent EventsHdlr;

        public AppTimers()
        {
            this.EventsHdlr = new SiRFLiveEvent();
            this._appTimer = new Timer();
            this._appTimerInterval = 0x3e8;
            this._appTimer.Elapsed += new ElapsedEventHandler(this.AppTimerHdlr);
            this._appTimer.Enabled = false;
            this._appTimer.Interval = this._appTimerInterval;
        }

        public AppTimers(int interval, bool isContinous)
        {
            this.EventsHdlr = new SiRFLiveEvent();
            this._appTimer = new Timer();
            this._appTimerInterval = 0x3e8;
            this._appTimer.Elapsed += new ElapsedEventHandler(this.AppTimerHdlr);
            this._appTimerInterval = interval * 0x3e8;
            this._appTimer.Interval = interval;
            this._appTimer.AutoReset = isContinous;
        }

        public void AppTimerClose()
        {
            this._appTimer.Stop();
            this._appTimer.Close();
        }

        public void AppTimerHdlr(object source, ElapsedEventArgs e)
        {
            this.EventsHdlr.SiRFLiveEventSet();
            if (!this._appTimer.AutoReset)
            {
                this.AppTimerClose();
            }
        }

        public void AppTimerStart()
        {
            this._appTimer.Interval = this._appTimerInterval;
            this._appTimer.Start();
        }

        public void AppTimerStop()
        {
            this._appTimer.Stop();
        }

        public int AppInterval
        {
            get
            {
                return this._appTimerInterval;
            }
            set
            {
                this._appTimerInterval = value;
            }
        }
    }
}

