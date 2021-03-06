﻿namespace SiRFLive.Utilities
{
    using SiRFLive.Communication;
    using System;
    using System.Collections;
    using System.Messaging;

    public interface IListener
    {
        void Cleanup();
        void Cleanup(string listenerName);
        void Cleanup(string listenerName, string comport);
        MessageQueue Create(string listenerName, string comport);
        MessageQueue Create(string listenerName, int source, int chan, int msgId, int subId, string thisCom);
        void Delete(string queueName);
        void DeleteAllListeners();
        void DeleteListener(string listenerName);
        MessageQueue GetListenerQRef(string m_key);
        MessageQueue GetListenerQRef(string listenerName, string comPort);
        ListenerContent GetListenerRef(string m_key);
        ListenerContent Receive(Message message);
        ListenerContent Receive(MessageQueue queue);
        ListenerContent Receive(string queueName);
        void Send(ListenerContent message);
        void Send(string input, string label);
        void Start();
        void Start(string listenerName);
        void Start(string listenerName, string comPort);
        void Stop();
        void Stop(string listenerName);
        void Stop(string listenerName, string comPort);

        string AidingProtocol { get; set; }

        Hashtable ListenerList { get; set; }

        object ListenerLock { get; set; }

        Hashtable ListenersQ { get; set; }

        CommunicationManager.ReceiverType RxType { get; set; }
    }
}

