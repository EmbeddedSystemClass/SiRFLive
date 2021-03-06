﻿namespace SiRFLive.DeviceControl
{
    using System;
    using System.Text.RegularExpressions;

    public class GPIB_Mgr_Agilent_HP8648C
    {
        private int _BoardID;
        private byte _PrimaryAddress;
        private byte _SecondaryAddress;
        private GPIB_Mgr Mgr_8648;

        public GPIB_Mgr_Agilent_HP8648C()
        {
            this.Mgr_8648 = new GPIB_Mgr(0, 7, 0);
        }

        public GPIB_Mgr_Agilent_HP8648C(int BoardID, byte PrimaryAddress, byte SecondaryAddress)
        {
            this.Mgr_8648 = new GPIB_Mgr(BoardID, PrimaryAddress, SecondaryAddress);
        }

        public void AutoAttenuation(bool autoAttenuation)
        {
            if (autoAttenuation)
            {
                this.Mgr_8648.GPIBWrite(":POW:ATT:AUTO ON;");
            }
            else
            {
                this.Mgr_8648.GPIBWrite(":POW:ATT:AUTO OFF;");
            }
        }

        public void Close()
        {
            this.Mgr_8648.GPIBClose();
        }

        public void ConfigAmpliude(double amplitude, AmpUnit ampUnit)
        {
            string str = string.Empty;
            str = ":POW:AMPL " + amplitude.ToString() + " ";
            switch (ampUnit)
            {
                case AmpUnit.DBM:
                    str = str + "DBM";
                    break;

                case AmpUnit.MV:
                    str = str + "MV";
                    break;

                case AmpUnit.UV:
                    str = str + "UV";
                    break;

                case AmpUnit.MVEMF:
                    str = str + "MVEMF";
                    break;

                case AmpUnit.UVEMF:
                    str = str + "UVEMF";
                    break;

                case AmpUnit.DBUV:
                    str = str + "DBUV";
                    break;

                case AmpUnit.DBUVEMF:
                    str = str + "DBUVEMF";
                    break;

                default:
                    str = str + "DBM";
                    break;
            }
            this.Mgr_8648.GPIBWrite(str);
        }

        public void ConfigAmpliudeModulation(bool AMState, bool AMInternalFreq, double AMDepth, SrcType AMSrc)
        {
            string str = string.Empty;
            if (!AMState)
            {
                str = ":AM:STAT OFF;";
            }
            else
            {
                str = ":FM:STAT OFF;:PM:STAT OFF;\n:AM:STAT ON;:AM:DEPT " + AMDepth.ToString() + " PCT;:AM:EXT:COUP AC;";
                if (AMInternalFreq)
                {
                    str = str + ":AM:INT:FREQ 1 KHZ;:AM:SOUR ";
                }
                else
                {
                    str = str + ":AM:INT:FREQ 400 HZ;:AM:SOUR ";
                }
                if (AMSrc == SrcType.INT)
                {
                    str = str + "INT;";
                }
                else if (AMSrc == SrcType.EXT)
                {
                    str = str + "EXT;";
                }
                else
                {
                    str = str + "INT,EXT;";
                }
            }
            this.Mgr_8648.GPIBWrite(str);
        }

        public void ConfigFrequency(double freq, FreqUnit freqUnit)
        {
            string str = string.Empty;
            str = ":FREQ:CW " + freq.ToString() + " ";
            switch (freqUnit)
            {
                case FreqUnit.MHZ:
                    str = str + "MHZ";
                    break;

                case FreqUnit.KHZ:
                    str = str + "KHZ";
                    break;

                case FreqUnit.HZ:
                    str = str + "HZ";
                    break;

                default:
                    str = str + "MHZ";
                    break;
            }
            this.Mgr_8648.GPIBWrite(str);
        }

        public void ConfigFrequencyModulation(bool FMState, double FMDeviation_kHz, SrcType FMSrc, bool FMInternalFreq, bool FMExternalFreq)
        {
            string str = string.Empty;
            if (!FMState)
            {
                str = ":FM:STAT OFF;";
            }
            else
            {
                str = ":AM:STAT OFF;:PM:STAT OFF;\n:FM:STAT ON;:FM:DEV " + FMDeviation_kHz.ToString() + " KHZ;:FM:EXT:COUP ";
                if (FMExternalFreq)
                {
                    str = str + "AC;";
                }
                else
                {
                    str = str + "DC;";
                }
                if (FMInternalFreq)
                {
                    str = str + ":FM:INT:FREQ 1 KHZ;:FM:SOUR ";
                }
                else
                {
                    str = str + ":FM:INT:FREQ 400 HZ;:FM:SOUR ";
                }
                if (FMSrc == SrcType.INT)
                {
                    str = str + "INT;";
                }
                else if (FMSrc == SrcType.EXT)
                {
                    str = str + "EXT;";
                }
                else
                {
                    str = str + "INT,EXT;";
                }
            }
            this.Mgr_8648.GPIBWrite(str);
        }

        public void ConfigPhaseModulation(bool PMState, bool PMInternalFreq, double PMDeviation_RAD, SrcType PMSrc)
        {
            string str = string.Empty;
            if (!PMState)
            {
                str = ":PM:STAT OFF;";
            }
            else
            {
                str = ":AM:STAT OFF;:FM:STAT OFF;\n:PM:STAT ON;:PM:DEV " + PMDeviation_RAD.ToString() + " RAD;:PM:EXT:COUP AC;";
                if (PMInternalFreq)
                {
                    str = str + ":PM:INT:FREQ 1 KHZ;:PM:SOUR ";
                }
                else
                {
                    str = str + ":PM:INT:FREQ 400 HZ;:PM:SOUR ";
                }
                if (PMSrc == SrcType.INT)
                {
                    str = str + "INT;";
                }
                else if (PMSrc == SrcType.EXT)
                {
                    str = str + "EXT;";
                }
                else
                {
                    str = str + "INT,EXT;";
                }
            }
            this.Mgr_8648.GPIBWrite(str);
        }

        public bool Error_Query()
        {
            this.Mgr_8648.GPIBWrite(":SYST:ERR?");
            string input = this.Mgr_8648.GPIBRead();
            Regex regex = new Regex("[0-9]+");
            return (Convert.ToInt16(regex.Match(input).Value) == 0);
        }

        public string getIDN()
        {
            this.Mgr_8648.GPIBWrite("*IDN?");
            return this.Mgr_8648.GPIBRead();
        }

        public bool Init(bool reset)
        {
            this.Mgr_8648.GPIBWrite("*IDN?");
            if (!this.Mgr_8648.GPIBRead().Contains("8648"))
            {
                return false;
            }
            if (reset)
            {
                this.Mgr_8648.GPIBWrite("*RST?");
            }
            return true;
        }

        public void Open()
        {
            this.Mgr_8648.GPIBOpen();
        }

        public double QueryParam_Amplitude()
        {
            this.Mgr_8648.GPIBWrite(":POW:AMPL?");
            return Convert.ToDouble(this.Mgr_8648.GPIBRead());
        }

        public double QueryParam_Frequency()
        {
            this.Mgr_8648.GPIBWrite(":FREQ:CW?");
            return (Convert.ToDouble(this.Mgr_8648.GPIBRead()) / 1000000.0);
        }

        public bool QueryParam_Output()
        {
            this.Mgr_8648.GPIBWrite(":OUTP:STAT?");
            return (this.Mgr_8648.GPIBRead().Substring(0, 1) == "1");
        }

        public void Reset()
        {
            this.Mgr_8648.GPIBWrite("*RST?");
        }

        public void RF_Output(bool RFoutput)
        {
            if (RFoutput)
            {
                this.Mgr_8648.GPIBWrite(":OUTP:STAT ON");
            }
            else
            {
                this.Mgr_8648.GPIBWrite(":OUTP:STAT OFF");
            }
        }

        public void SelfTest()
        {
            this.Mgr_8648.GPIBWrite("*TST?");
        }

        public void SetRefenceAmpliude(bool refState, double ampRefValue)
        {
            string str = string.Empty;
            if (!refState)
            {
                str = ":POW:REF:STAT OFF";
            }
            else
            {
                str = ":POW:REF:STAT ON;:POW:REF " + ampRefValue.ToString() + " DBM;";
            }
            this.Mgr_8648.GPIBWrite(str);
        }
    }
}

