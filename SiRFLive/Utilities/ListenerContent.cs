﻿namespace SiRFLive.Utilities
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    [Serializable]
    public class ListenerContent
    {
        public int Chan;
        public string ComPort;
        public DateTime CreationDate;
        public BackgroundWorker DoMainWork;
        public BackgroundWorker DoUserWork;
        public string ListenerName;
        public string MessageText;
        public string MessageTime;
        public int MsgId;
        public Queue MsgQData;
        public Queue QueueData;
        public int Source;
        public bool State;
        public int SubId;

        public ListenerContent()
        {
            this.QueueData = new Queue(0x2710);
            this.CreationDate = DateTime.Now;
            this.ListenerName = string.Empty;
            this.MessageText = string.Empty;
            this.MessageTime = string.Empty;
            this.Source = -1;
            this.MsgId = -1;
            this.SubId = -1;
            this.Chan = -1;
            this.ComPort = string.Empty;
            this.MsgQData = new Queue(0x2710);
        }

        public ListenerContent(string messageText)
        {
            this.QueueData = new Queue(0x2710);
            this.CreationDate = DateTime.Now;
            this.ListenerName = string.Empty;
            this.MessageText = string.Empty;
            this.MessageTime = string.Empty;
            this.Source = -1;
            this.MsgId = -1;
            this.SubId = -1;
            this.Chan = -1;
            this.ComPort = string.Empty;
            this.MsgQData = new Queue(0x2710);
            this.MessageText = messageText;
        }
    }
}

