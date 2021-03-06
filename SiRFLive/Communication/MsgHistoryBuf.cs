﻿namespace SiRFLive.Communication
{
    using System;
    using System.Collections;

    public class MsgHistoryBuf
    {
        private Queue _MsgHistoryBuf = new Queue();
        private int _MsgHistoryBufSize;

        public MsgHistoryBuf(int inMaxSize)
        {
            this._MsgHistoryBufSize = inMaxSize;
        }

        public void AddData(string comBuffer)
        {
            this._MsgHistoryBuf.Enqueue(comBuffer);
            if (this._MsgHistoryBuf.Count > this._MsgHistoryBufSize)
            {
                this._MsgHistoryBuf.Dequeue();
            }
        }

        private int GetMIDFromMsg(string msg, bool useChannelInHeader)
        {
            return -1;
        }

        public string[] GetMsgByMID(int MID, int SUBID)
        {
            return new string[] { "" };
        }

        private int GetSIDFromMsg(string msg, bool useChannelInHeader)
        {
            return -1;
        }

        public int GetSize()
        {
            return this._MsgHistoryBuf.Count;
        }
    }
}

