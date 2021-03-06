﻿namespace SiRFLive.MessageHandling
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.Text;
    using System.Threading;
    using System.Timers;

    public class FAndSSBReset : ReceiverReset
    {
        public override void Reset(string resetType)
        {
            string csvMessage = string.Empty;
            int mid = 0x80;
            string messageName = "Receiver Init";
            string msg = string.Empty;
            string protocol = string.Empty;
            string str5 = string.Empty;
            SSB_Format format = new SSB_Format();
            base.ResetType = resetType;
            base._rxComm.AutoReplyCtrl.AidingFlag = 0;
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(this.SS3OpenSSBAndStatsForFactory);
            timer.Interval = 2000.0;
            timer.AutoReset = false;
            if (base._rxComm.RxType == CommunicationManager.ReceiverType.SLC)
            {
                if (base._rxComm.ListenersCtrl != null)
                {
                    base._rxComm.AutoReplyCtrl.AutoReplyHWCfgResp();
                    base._rxComm.AutoReplyCtrl.AutoReplyFreqTransferResp();
                    base._rxComm.AutoReplyCtrl.AutoReplyApproxPositionResp();
                }
                if (base._rxComm.AutoReplyCtrl.AutoReplyParams.AutoAid_Eph_fromTTB && base._rxComm.TTBPort.IsOpen)
                {
                    string ephDataFromTTB = base._rxComm.GetEphDataFromTTB();
                    if (ephDataFromTTB != string.Empty)
                    {
                        base._rxComm.AutoReplyCtrl.EphDataMsg = ephDataFromTTB;
                        base._rxComm.AutoReplyCtrl.EphDataMsgBackup = ephDataFromTTB;
                    }
                    if (base._rxComm.AutoReplyCtrl.PositionRequestCtrl.LocMethod != 0)
                    {
                        base._rxComm.AutoReplyCtrl.AidingFlag = (byte) (base._rxComm.AutoReplyCtrl.AidingFlag | 1);
                    }
                }
                else if (base._rxComm.AutoReplyCtrl.AutoReplyParams.AutoAid_Eph_fromFile || base._rxComm.AutoReplyCtrl.AutoReplyParams.AutoAid_ExtEph_fromFile)
                {
                    GPSDateTime time = base.ResetGPSTimer.GetTime();
                    int gPSWeek = time.GetGPSWeek();
                    double gPSTOW = time.GetGPSTOW();
                    double num4 = (gPSWeek * 0x93a80) + gPSTOW;
                    string[] strArray = num4.ToString().Split(new char[] { '.' });
                    string ephCsvString = base._rxComm.AutoReplyCtrl.GetEphAidingMsgFromFile(base._rxComm.AutoReplyCtrl.ControlChannelVersion, base._rxComm.AutoReplyCtrl.EphFilePath, strArray[0]);
                    if (ephCsvString != string.Empty)
                    {
                        if (ephCsvString.Contains("Error"))
                        {
                            base._rxComm.WriteApp("### " + ephCsvString + "###");
                        }
                        else
                        {
                            base._rxComm.AutoReplyCtrl.EphDataMsg = ephCsvString;
                            base._rxComm.AutoReplyCtrl.EphDataMsgBackup = AutoReplyMgr.BuildOSPEphAidingMsg(ephCsvString, "1.0");
                        }
                    }
                    if (base._rxComm.AutoReplyCtrl.PositionRequestCtrl.LocMethod != 0)
                    {
                        base._rxComm.AutoReplyCtrl.AidingFlag = (byte) (base._rxComm.AutoReplyCtrl.AidingFlag | 1);
                    }
                }
                if (base._rxComm.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromTTB && base._rxComm.TTBPort.IsOpen)
                {
                    string acqAssistDataFromTTB = base._rxComm.GetAcqAssistDataFromTTB();
                    if (acqAssistDataFromTTB != string.Empty)
                    {
                        base._rxComm.AutoReplyCtrl.AcqAssistDataMsg = acqAssistDataFromTTB;
                    }
                    base._rxComm.AutoReplyCtrl.AidingFlag = (byte) (base._rxComm.AutoReplyCtrl.AidingFlag | 2);
                }
                else if (base._rxComm.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromFile)
                {
                    double gpsTowNow = base.ResetGPSTimer.GetTime().GetGPSTOW();
                    string str9 = base._rxComm.AutoReplyCtrl.GetAcqAssistMsgFromFile(base._rxComm.RxCtrl.ControlChannelVersion, base._rxComm.AutoReplyCtrl.AcqDataFilePath, gpsTowNow);
                    if (str9 != string.Empty)
                    {
                        if (str9.Contains("Error"))
                        {
                            base._rxComm.WriteApp("### " + str9 + "###");
                        }
                        else
                        {
                            base._rxComm.AutoReplyCtrl.AcqAssistDataMsg = str9;
                        }
                    }
                    base._rxComm.AutoReplyCtrl.AidingFlag = (byte) (base._rxComm.AutoReplyCtrl.AidingFlag | 2);
                }
                if (base._rxComm.AutoReplyCtrl.AutoReplyParams.AutoAid_Alm && base._rxComm.TTBPort.IsOpen)
                {
                    base._rxComm.AutoReplyCtrl.AidingFlag = (byte) (base._rxComm.AutoReplyCtrl.AidingFlag | 4);
                }
                if (base._rxComm.AutoReplyCtrl.AutoReplyParams.AutoAid_NavBit && base._rxComm.TTBPort.IsOpen)
                {
                    string str10 = string.Empty;
                    base._rxComm.AutoReplyCtrl.SetupAuxNavMsgFromTTB(base._rxComm.GetAuxNavDataFromTTB());
                    str10 = base._rxComm.AutoReplyCtrl.SetupNavSF45FromTTB(base._rxComm.GetNavBitSF45DataSet0FromTTB());
                    if (str10 != string.Empty)
                    {
                        base._rxComm.AutoReplyCtrl.SF45DataSet0MsgFromTTB = str10;
                    }
                    str10 = base._rxComm.AutoReplyCtrl.SetupNavSF45FromTTB(base._rxComm.GetNavBitSF45DataSet1FromTTB());
                    if (str10 != string.Empty)
                    {
                        base._rxComm.AutoReplyCtrl.SF45DataSet1MsgFromTTB = str10;
                    }
                    base._rxComm.AutoReplyCtrl.AidingFlag = (byte) (base._rxComm.AutoReplyCtrl.AidingFlag | 8);
                }
            }
            StringBuilder builder = new StringBuilder();
            string str11 = resetType;
            if (str11 != null)
            {
                if (!(str11 == "SLC_HOT"))
                {
                    if (str11 == "SLC_COLD")
                    {
                        protocol = "F";
                        messageName = "Reset GPS Command";
                        mid = 0x16;
                        str5 = "1";
                        builder.Append("2,");
                        goto Label_0650;
                    }
                    if (str11 == "SLC_FACTORY")
                    {
                        protocol = "F";
                        messageName = "Reset GPS Command";
                        mid = 0x16;
                        str5 = "2";
                        builder.Append("2,");
                        goto Label_0650;
                    }
                }
                else
                {
                    protocol = "F";
                    messageName = "Reset GPS Command";
                    mid = 0x16;
                    str5 = "0";
                    builder.Append("2,");
                    goto Label_0650;
                }
            }
            protocol = "SSB";
            messageName = "Receiver Init";
            mid = 0x80;
            str5 = format.GetResetBitMap(resetType, base.ResetInitParams.Enable_Navlib_Data, base.ResetInitParams.Enable_Development_Data, base.ResetInitParams.EnableFullSystemReset).ToString();
            if (base._rxComm._rxType == CommunicationManager.ReceiverType.SLC)
            {
                builder.Append("238,");
            }
            else if (base._rxComm._rxType == CommunicationManager.ReceiverType.TTB)
            {
                builder.Append("204,");
            }
        Label_0650:
            base._navData.IsNav = false;
            ArrayList list = new ArrayList();
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
            msg = base._rxComm.m_Protocols.ConvertFieldsToRaw(csvMessage, messageName, protocol);
            base.ResetDataInit();
            base._rxComm.RxCtrl.NavDataInit();
            base._rxComm.WriteData(msg);
            base.StartResetTime = DateTime.Now.Ticks;
            if (base.ResetType.Contains("FACTORY"))
            {
                timer.Start();
            }
            base.DisplayResetType = base.ResetType;
            if (base._rxComm.AutoReplyCtrl.AutoReplyParams.AutoReply && !base.ResetType.Contains("FACTORY"))
            {
                base.DisplayResetType = base.DisplayResetType + "_Aided";
            }
        }

        private void SS3OpenSSBAndStatsForFactory(object source, ElapsedEventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
                base._rxComm.RxCtrl.OpenChannel("SSB");
                base._rxComm.RxCtrl.OpenChannel("STAT");
                base._rxComm.RxCtrl.SetMessageRateForFactory();
            }
            catch
            {
            }
        }
    }
}

