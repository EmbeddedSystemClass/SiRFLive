﻿namespace SiRFLive.Utilities
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class SiRFLiveEvent
    {
        public event SiRFLiveEventHandler SetSiRFLiveEventHdlr;

        private void OnSiRFLiveEventSet()
        {
            if (this.SetSiRFLiveEventHdlr != null)
            {
                foreach (Delegate delegate2 in this.SetSiRFLiveEventHdlr.GetInvocationList())
                {
                    SiRFLiveEventHandler handler2 = (SiRFLiveEventHandler) delegate2;
                    try
                    {
                        handler2(this, new SiRFLiveEventArgs(true));
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "SiRFLive Event");
                    }
                }
            }
        }

        public void SiRFLiveEventSet()
        {
            this.OnSiRFLiveEventSet();
        }

        public delegate void SiRFLiveEventHandler(object sender, SiRFLiveEventArgs e);
    }
}

