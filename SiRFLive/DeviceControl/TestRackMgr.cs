﻿namespace SiRFLive.DeviceControl
{
    using AxTestControl;
    using System;
    using System.Windows.Forms;

    public class TestRackMgr
    {
        private bool _isInit;
        private string _stateHWInit;
        private AxControlHW testCtrlIntf;

        public TestRackMgr()
        {
            this.testCtrlIntf = new AxControlHW();
            this._stateHWInit = "HWinit";
            try
            {
                this.testCtrlIntf.CreateControl();
                this.Init(true);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error!");
            }
        }

        public TestRackMgr(bool initialState)
        {
            this.testCtrlIntf = new AxControlHW();
            this._stateHWInit = "HWinit";
            try
            {
                this.testCtrlIntf.CreateControl();
                this.Init(initialState);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error!");
            }
        }

        public int Init()
        {
            if (!this._isInit)
            {
                this.testCtrlIntf.GPIBaddress = 0x63;
                this.testCtrlIntf.Instrument = "34970A";
                this.testCtrlIntf.HWPlatform = "HWinit";
                this.testCtrlIntf.HWexecute = 0;
                if (this.testCtrlIntf.HWstatus != this._stateHWInit)
                {
                    return 1;
                }
                this._isInit = true;
            }
            return 0;
        }

        public int Init(bool initialState)
        {
            int num = 0;
            if (!this._isInit)
            {
                this.testCtrlIntf.GPIBaddress = 0x63;
                this.testCtrlIntf.Instrument = "34970A";
                this.testCtrlIntf.HWPlatform = "HWinit";
                this.testCtrlIntf.HWexecute = 0;
                if (this.testCtrlIntf.HWstatus != this._stateHWInit)
                {
                    num = 1;
                }
                else
                {
                    this._isInit = true;
                }
            }
            if (initialState)
            {
                this.Power(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            }
            return num;
        }

        public void Power(int[] values)
        {
            if (!this._isInit)
            {
                this.Init();
            }
            for (int i = 1; i <= 8; i++)
            {
                string str = string.Format("Power {0} {1:D}", (values[i - 1] == 1) ? "On" : "Off", i);
                this.testCtrlIntf.HWPlatform = str;
                this.testCtrlIntf.HWexecute = (short) i;
            }
        }

        public void Power(int port, bool state)
        {
            if (!this._isInit)
            {
                this.Init();
            }
            string str = string.Format("Power {0} {1:D}", state ? "On" : "Off", port);
            this.testCtrlIntf.HWPlatform = str;
            this.testCtrlIntf.HWexecute = (short) port;
        }

        public void Query()
        {
        }

        public void SetAtten(int port, int value)
        {
            if (!this._isInit)
            {
                this.Init();
            }
            if (value < 0)
            {
                value = 0;
            }
            if (value > 0x7f)
            {
                value = 0x7f;
            }
            if (port > 0)
            {
                if (port <= 8)
                {
                    string str = string.Format("Set Attenuation {0}", port);
                    this.testCtrlIntf.HWPlatform = str;
                    this.testCtrlIntf.HWexecute = (short) value;
                }
            }
            else
            {
                for (int i = 1; i <= 8; i++)
                {
                    string str2 = string.Format("Set Attenuation {0}", i);
                    this.testCtrlIntf.HWPlatform = str2;
                    this.testCtrlIntf.HWexecute = (short) value;
                }
            }
        }
    }
}

