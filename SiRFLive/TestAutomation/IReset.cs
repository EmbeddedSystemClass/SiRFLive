﻿namespace SiRFLive.TestAutomation
{
    using SiRFLive.Communication;
    using SiRFLive.MessageHandling;
    using SiRFLive.Reporting;
    using SiRFLive.Utilities;
    using System;

    public interface IReset
    {
        void CloseTTFFLog();
        void OpenTTFFLog(string fileName);
        void Reset(string resetType);
        void ResetTimerClose();
        void ResetTimerStart();
        void ResetTimerStop();

        CommunicationManager ResetComm { get; set; }

        SiRFLiveEvent ResetDoneEvent { get; }

        resetInitParams ResetInitParams { get; set; }

        int ResetInterval { get; set; }

        NavigationAnalysisData ResetNavData { get; set; }

        bool ResetPositionAvailable { get; set; }

        int ResetRandomRange { get; set; }

        string ResetRxSwVersion { get; set; }

        TestSetup ResetTestSetup { get; set; }

        bool ResetTTFFAvailable { get; set; }

        string ResetType { get; set; }

        int TotalNumberOfResets { get; set; }
    }
}

