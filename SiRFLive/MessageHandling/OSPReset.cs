﻿namespace SiRFLive.MessageHandling
{
    using CommMgrClassLibrary;
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.Collections;
    using System.Text;
    using System.Threading;
    using System.Timers;

    public class OSPReset : ReceiverReset
    {
        private void OSPSetMessageRateForFactory(object source, ElapsedEventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
                if (base.IsProtocolSwitchedOnFactory)
                {
                    base._rxComm.RxCtrl.AutoDetectBaudAfterFacRst4E(ref this._rxComm);
                }
                base._rxComm.RxCtrl.SetMessageRateForFactory();
            }
            catch
            {
            }
        }

        public override void Reset(string resetType)
        {
            string csvMessage = string.Empty;
            int mid = 0x80;
            int sid = 1;
            string messageName = "Receiver Init";
            string msg = string.Empty;
            string protocol = string.Empty;
            string str5 = string.Empty;
            if (base._rxComm.ProductFamily != CommonClass.ProductType.GSD4e)
            {
                base.IsProtocolSwitchedOnFactory = false;
            }
            base.IsFirstTTFS = false;
            base._rxComm.ToSwitchProtocol = base._rxComm.MessageProtocol;
            base._rxComm.ToSwitchBaud = base._rxComm.BaudRate;
            SSB_Format format = new SSB_Format();
            if (resetType == "SLC_COLD")
            {
                resetType = "COLD";
            }
            else if (resetType == "SLC_HOT")
            {
                resetType = "HOT";
            }
            else if (resetType == "SLC_FACTORY")
            {
                resetType = "FACTORY";
            }
            base.ResetType = resetType;
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(this.OSPSetMessageRateForFactory);
            timer.Interval = 2000.0;
            timer.AutoReset = false;
            if (base._rxComm.RxType == CommunicationManager.ReceiverType.SLC)
            {
                base._rxComm.PrepareAutoReplyData();
            }
            protocol = "SSB";
            if (clsGlobal.IsDROn)
            {
                messageName = "Initialize GPS/DR Navigation";
                mid = 0xac;
                sid = 1;
            }
            else
            {
                messageName = "Receiver Init";
                mid = 0x80;
            }
            byte num3 = format.GetResetBitMap(resetType, base.ResetInitParams.Enable_Navlib_Data, base.ResetInitParams.Enable_Development_Data, base.ResetInitParams.EnableFullSystemReset);
            if (base.ResetInitParams.KeepFlashData)
            {
                num3 = (byte) (num3 | 1);
            }
            str5 = num3.ToString();
            base._navData.IsNav = false;
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            if (!clsGlobal.IsDROn)
            {
                list = base._rxComm.m_Protocols.GetInputMessageStructure(mid, -1, messageName, protocol);
                if (resetType == "WARM_INIT")
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        switch (((InputMsg) list[i]).fieldName)
                        {
                            case "Message ID":
                                builder.Append(mid.ToString());
                                builder.Append(",");
                                break;

                            case "ECEF X":
                                builder.Append(base.ResetInitParams.ECEFX);
                                builder.Append(",");
                                break;

                            case "ECEF Y":
                                builder.Append(base.ResetInitParams.ECEFY);
                                builder.Append(",");
                                break;

                            case "ECEF Z":
                                builder.Append(base.ResetInitParams.ECEFZ);
                                builder.Append(",");
                                break;

                            case "Clock Drift":
                                builder.Append(base.ResetInitParams.ClockDrift);
                                builder.Append(",");
                                break;

                            case "TOW":
                                builder.Append(base.ResetInitParams.TOW);
                                builder.Append(",");
                                break;

                            case "Week Number":
                                builder.Append(base.ResetInitParams.WeekNumber);
                                builder.Append(",");
                                break;

                            case "Channels":
                                builder.Append(base.ResetInitParams.Channels);
                                builder.Append(",");
                                break;

                            case "Reset Config Bitmap":
                                builder.Append(str5);
                                break;
                        }
                    }
                    csvMessage = builder.ToString().TrimEnd(new char[] { ',' });
                }
                else
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if ((((InputMsg) list[j]).fieldName == "Reset Config Bitmap") || (((InputMsg) list[j]).fieldName == "RESET TYPE"))
                        {
                            builder.Append(str5);
                            builder.Append(",");
                        }
                        else
                        {
                            builder.Append(((InputMsg) list[j]).defaultValue);
                            builder.Append(",");
                        }
                    }
                    csvMessage = builder.ToString().TrimEnd(new char[] { ',' });
                }
            }
            else
            {
                list = base._rxComm.m_Protocols.GetInputMessageStructure(mid, sid, messageName, protocol);
                if (resetType == "WARM_INIT")
                {
                    for (int k = 0; k < list.Count; k++)
                    {
                        switch (((InputMsg) list[k]).fieldName)
                        {
                            case "Message ID":
                                builder.Append(mid.ToString());
                                builder.Append(",");
                                break;

                            case "Message Sub ID":
                                builder.Append(sid.ToString());
                                builder.Append(",");
                                break;

                            case "Latitude":
                                builder.Append(base.ResetInitParams.ECEFX);
                                builder.Append(",");
                                break;

                            case "Longitude":
                                builder.Append(base.ResetInitParams.ECEFY);
                                builder.Append(",");
                                break;

                            case "Altitude (Ellipsoid)":
                                builder.Append(base.ResetInitParams.ECEFZ);
                                builder.Append(",");
                                break;

                            case "True Heading":
                                builder.Append(base.ResetInitParams.TrueHeading);
                                builder.Append(",");
                                break;

                            case "Clock Drift":
                                builder.Append(base.ResetInitParams.ClockDrift);
                                builder.Append(",");
                                break;

                            case "GPS TOW":
                                builder.Append(base.ResetInitParams.TOW);
                                builder.Append(",");
                                break;

                            case "GPS Week Number":
                                builder.Append(base.ResetInitParams.WeekNumber);
                                builder.Append(",");
                                break;

                            case "Channel Count":
                                builder.Append(base.ResetInitParams.Channels);
                                builder.Append(",");
                                break;

                            case "Reset Configuration Bits":
                                builder.Append(str5);
                                break;
                        }
                    }
                    csvMessage = builder.ToString().TrimEnd(new char[] { ',' });
                }
                else
                {
                    for (int m = 0; m < list.Count; m++)
                    {
                        if ((((InputMsg) list[m]).fieldName == "Reset Configuration Bits") || (((InputMsg) list[m]).fieldName == "RESET TYPE"))
                        {
                            builder.Append(str5);
                            builder.Append(",");
                        }
                        else
                        {
                            builder.Append(((InputMsg) list[m]).defaultValue);
                            builder.Append(",");
                        }
                    }
                    csvMessage = builder.ToString().TrimEnd(new char[] { ',' });
                }
            }
            msg = base._rxComm.m_Protocols.ConvertFieldsToRaw(csvMessage, messageName, protocol);
            if ((base._rxComm.CMC.HostAppI2CSlave.I2CTalkMode == CommMgrClass.I2CSlave.I2CCommMode.COMM_MODE_I2C_SLAVE) && (base._rxComm.InputDeviceMode == CommonClass.InputDeviceModes.I2C))
            {
                base._rxComm.I2CModeSwitchDone = false;
                Thread.Sleep(50);
                base.ResetDataInit();
                base._rxComm.RxCtrl.NavDataInit();
                base._rxComm.WriteData(msg);
                base._rxComm.CMC.HostAppI2CSlave.I2CTalkMode = CommMgrClass.I2CSlave.I2CCommMode.COMM_MODE_I2C_MULTI_MASTER;
                Thread.Sleep(50);
                base._rxComm.I2CModeSwitchDone = true;
            }
            else
            {
                base.ResetDataInit();
                base._rxComm.RxCtrl.NavDataInit();
                base._rxComm.WriteData(msg);
            }
            base.StartResetTime = DateTime.Now.Ticks;
            if (base.ResetType.Contains("FACTORY"))
            {
                timer.Start();
            }
            base.DisplayResetType = base.ResetType;
            if (base._rxComm.AutoReplyCtrl.AutoReplyParams.AutoReply)
            {
                if (!base.ResetType.Contains("FACTORY"))
                {
                    base.DisplayResetType = base.DisplayResetType + "_Aided";
                }
                else if (base.IsAidingPerformedOnFactory)
                {
                    base.DisplayResetType = base.DisplayResetType + "_Aided";
                }
            }
        }
    }
}

