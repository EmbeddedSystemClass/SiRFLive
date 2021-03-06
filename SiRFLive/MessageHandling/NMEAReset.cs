﻿namespace SiRFLive.MessageHandling
{
    using System;
    using System.Text;

    public class NMEAReset : ReceiverReset
    {
        public override void Reset(string resetType)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("$PSRF101,");
            builder.Append(base.ResetInitParams.ECEFX);
            builder.Append(",");
            builder.Append(base.ResetInitParams.ECEFY);
            builder.Append(",");
            builder.Append(base.ResetInitParams.ECEFZ);
            builder.Append(",");
            builder.Append(base.ResetInitParams.ClockDrift);
            builder.Append(",");
            builder.Append(base.ResetInitParams.TOW);
            builder.Append(",");
            builder.Append(base.ResetInitParams.WeekNumber);
            builder.Append(",");
            builder.Append(base.ResetInitParams.Channels);
            builder.Append(",");
            switch (resetType)
            {
                case "SLC_HOT":
                case "HOT":
                    builder.Append("1");
                    break;

                case "WARM_NO_INIT":
                    builder.Append("2");
                    break;

                case "WARM_INIT":
                    builder.Append("3");
                    break;

                case "SLC_COLD":
                case "COLD":
                    builder.Append("4");
                    break;

                case "FACTORY":
                    builder.Append("8");
                    break;

                default:
                    builder.Append("1");
                    break;
            }
            base._navData.IsNav = false;
            base.ResetDataInit();
            base._rxComm.RxCtrl.NavDataInit();
            string msg = NMEAReceiver.NMEA_AddCheckSum(builder.ToString());
            base._rxComm.WriteData(msg);
            if (base.ResetInitParams.Enable_Development_Data)
            {
                base._rxComm.WriteData(NMEAReceiver.NMEA_AddCheckSum("$PSRF105,1"));
            }
            else
            {
                base._rxComm.WriteData(NMEAReceiver.NMEA_AddCheckSum("$PSRF105,0"));
            }
        }
    }
}

