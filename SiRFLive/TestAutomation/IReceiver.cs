﻿namespace SiRFLive.TestAutomation
{
    using LogManagerClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.Reporting;
    using System;
    using System.Collections;

    public interface IReceiver
    {
        void CloseChannel();
        void CloseSession();
        LogManager CreateLog(string logName);
        string DecodeGeodeticNavigationDataToCSV(string message);
        string DecodeNavLibMeasurementDataToCSV(string message);
        string DecodeRawMeasuredNavigationDataToCSV(string message);
        string DecodeRawMessageToCSV(string protocol, string message);
        Hashtable DecodeRawMessageToHash(string protocol, string message);
        void GetAutoReplyParameters(string configFilePath);
        string[] GetLatLonAltPosition(string csvString);
        byte GetNavigationMode(string message);
        string GetPerChanAvgCNo(string message);
        void GetSWVersion(string message);
        string GetTTFF(string logTime, string message, int type);
        byte IsNavigated(string message);
        void LogCleanup();
        void LogClose(string logName);
        void LogWrite(string logName, string inputString);
        void OpenChannel(string channel);
        void OpenSession();
        void PollSWVersion();
        void SaveTestSetup(string key, string inputValue);
        void SendApproximatePositionAiding(string site);
        void SendFreqAiding();
        void SendHWConfig();
        void SendPositionRequest(string EphemerisSite);
        void SendRaw(string rawString);
        void SendTimeAiding();
        void SetAidingVersion(string version);
        void SetClockDrift(string message);
        void SetControlChannelVersion(string version);
        void SetLowPower();

        string AidingProtocol { get; set; }

        string DefaultFreqOffset { get; set; }

        string MessageProtocol { get; set; }

        IReset ResetCtrl { get; set; }

        CommunicationManager RxCommWindow { get; set; }

        NavigationAnalysisData RxNavData { get; set; }

        TestSetup RxTestSetup { get; set; }
    }
}

