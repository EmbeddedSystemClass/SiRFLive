﻿namespace SiRFLive.Utilities
{
    using SiRFLive.Communication;
    using System;

    public class NMEAListnerManager : ListenerManager
    {
        public override ListenerContent Create(string listenerName, string comport)
        {
            if (((listenerName == null) || (listenerName.Length == 0)) || ((comport == null) || (comport.Length == 0)))
            {
                return null;
            }
            ListenerContent content = null;
            switch (listenerName)
            {
                case "HWConfig":
                case "HWConfig_GUI":
                case "TimeAiding":
                case "TimeAiding_GUI":
                case "FreqAiding":
                case "FreqAiding_GUI":
                case "ApproxPosition":
                case "ApproxPosition_GUI":
                    return content;

                case "Nav":
                case "Nav_GUI":
                    return this.Create(listenerName, 0, -1, 0x29, -1, comport);

                case "TTFF":
                case "TTFF_GUI":
                    return this.Create(listenerName, 0, -1, 0xe1, 6, comport);

                case "ClockStatus":
                case "ClockStatus_GUI":
                    return this.Create(listenerName, 0, -1, 7, -1, comport);

                case "MeasuredNavigationData":
                case "MeasuredNavigationData_GUI":
                case "GeodeticNavigationData":
                case "GeodeticNavigationData_GUI":
                    return this.Create(listenerName, 0, -1, 0x29, -1, comport);

                case "NavLibMeasurement":
                case "NavLibMeasurement_GUI":
                    return this.Create(listenerName, 0, -1, 0x1c, -1, comport);

                case "PostionResponse":
                case "PostionResponse_GUI":
                    return this.Create(listenerName, 0, -1, 0x29, -1, comport);

                case "SWVersion":
                case "SWVersion_GUI":
                    return this.Create(listenerName, 0, -1, 6, -1, comport);

                case "CNO":
                case "CNO_GUI":
                    return this.Create(listenerName, 0, -1, 4, -1, comport);

                case "SVS":
                case "SVS_GUI":
                    if (base._rxType != CommunicationManager.ReceiverType.SLC)
                    {
                        return this.Create(listenerName, 0, -1, 4, -1, comport);
                    }
                    return this.Create(listenerName, 0, 0xee, 4, -1, comport);
            }
            return content;
        }
    }
}

