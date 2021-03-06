﻿namespace SiRFLive.Utilities
{
    using System;

    public class OSPListnerManager : ListenerManager
    {
        public override ListenerContent Create(string listenerName, string comport)
        {
            if (((listenerName == null) || (listenerName.Length == 0)) || ((comport == null) || (comport.Length == 0)))
            {
                return null;
            }
            switch (listenerName)
            {
                case "MPM_navState1":
                case "MPM_navState1_GUI":
                    return this.Create(listenerName, 0, -1, 0x44, 0xff, comport);

                case "MPM_navState_V2":
                case "MPM_navState_V2_GUI":
                    return this.Create(listenerName, 0, -1, 0x44, 0xe1, comport);

                case "MPM_SVD1":
                case "MPM_SVD1_GUI":
                    return this.Create(listenerName, 0, -1, 0xff, 0, comport);

                case "MPM_navState1:status":
                case "MPM_navState1:status_GUI":
                    return this.Create(listenerName, 0, -1, 0xff, 0, comport);

                case "valid sats":
                case "valid sats_GUI":
                    return this.Create(listenerName, 0, -1, 0xff, 0, comport);

                case "MPM_STATS_SV_DATA_UPDATE":
                case "MPM_STATS_SV_DATA_UPDATE_GUI":
                    return this.Create(listenerName, 0, -1, 0x4d, 1, comport);

                case "MPM_STATS_EPH_COLLECTION":
                case "MPM_STATS_EPH_COLLECTION_GUI":
                    return this.Create(listenerName, 0, -1, 0x4d, 2, comport);

                case "MPM_STATS_ALM_COLLECTION":
                case "MPM_STATS_ALM_COLLECTION_GUI":
                    return this.Create(listenerName, 0, -1, 0x4d, 3, comport);

                case "MPM_STATS_GPS_UPDATE":
                case "MPM_STATS_GPS_UPDATE_GUI":
                    return this.Create(listenerName, 0, -1, 0x4d, 4, comport);

                case "MPM_STATS_RECOVERY_MODE":
                case "MPM_STATS_RECOVERY_MODE_GUI":
                    return this.Create(listenerName, 0, -1, 0x4d, 5, comport);

                case "MPM_STATS_FULLPOWER_SV_DATA_UPDATE":
                case "MPM_STATS_FULLPOWER_SV_DATA_UPDATE_GUI":
                    return this.Create(listenerName, 0, -1, 0x4d, 7, comport);

                case "HWConfig":
                case "HWConfig_GUI":
                    return this.Create(listenerName, 0, -1, 0x47, -1, comport);

                case "TimeAiding":
                case "TimeAiding_GUI":
                    return this.Create(listenerName, 0, -1, 0x49, 2, comport);

                case "FreqAiding":
                case "FreqAiding_GUI":
                    return this.Create(listenerName, 0, -1, 0x49, 3, comport);

                case "ApproxPosition":
                case "ApproxPosition_GUI":
                    return this.Create(listenerName, 0, -1, 0x49, 1, comport);

                case "Nav":
                case "Nav_GUI":
                    return this.Create(listenerName, 0, -1, 2, -1, comport);

                case "CWControllerScanResult":
                case "CWControllerScanResult_GUI":
                    return this.Create(listenerName, 0, -1, 0x5c, 1, comport);

                case "TTFF":
                case "TTFF_GUI":
                    return this.Create(listenerName, 0, -1, 0xe1, 6, comport);

                case "TTFF_MSA":
                case "TTFF_MSA_GUI":
                    return this.Create(listenerName, 0, -1, 0xe1, 7, comport);

                case "XOLearning_CurrentTemp":
                case "XOLearning_CurrentTemp_GUI":
                    return this.Create(listenerName, 0, -1, 0x5d, 12, comport);

                case "ClockStatus":
                case "ClockStatus_GUI":
                    return this.Create(listenerName, 0, -1, 7, -1, comport);

                case "MeasuredNavigationData":
                case "MeasuredNavigationData_GUI":
                    return this.Create(listenerName, 0, -1, 2, -1, comport);

                case "GeodeticNavigationData":
                case "GeodeticNavigationData_GUI":
                    return this.Create(listenerName, 0, -1, 0x29, -1, comport);

                case "NavLibMeasurement":
                case "NavLibMeasurement_GUI":
                    return this.Create(listenerName, 0, -1, 0x1c, -1, comport);

                case "PostionResponse":
                case "PostionResponse_GUI":
                    return this.Create(listenerName, 0, -1, 0x45, 1, comport);

                case "MeasurementResponse":
                case "MeasurementResponse_GUI":
                    return this.Create(listenerName, 0, -1, 0x45, 2, comport);

                case "SWVersion":
                case "SWVersion_GUI":
                    return this.Create(listenerName, 0, -1, 6, -1, comport);

                case "CNO":
                case "CNO_GUI":
                    return this.Create(listenerName, 0, -1, 4, -1, comport);

                case "SVS":
                case "SVS_GUI":
                    return this.Create(listenerName, 0, -1, 4, -1, comport);

                case "ACK":
                case "ACK_GUI":
                    return this.Create(listenerName, 0, -1, 0x4b, 1, comport);

                case "PollAlm_GUI":
                    return this.Create(listenerName, 0, -1, 14, -1, comport);

                case "PollEph_GUI":
                    return this.Create(listenerName, 0, -1, 15, -1, comport);
            }
            return null;
        }
    }
}

