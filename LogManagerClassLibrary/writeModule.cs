﻿namespace LogManagerClassLibrary
{
    using System;
    using System.IO;

    internal class writeModule
    {
        protected BinaryWriter m_writeBinObj;
        protected StreamWriter m_writeTextObj;

        internal writeModule()
        {
        }

        internal void Close()
        {
            if (m_writeTextObj != null)
            {
                m_writeTextObj.Close();
            }
            if (m_writeBinObj != null)
            {
                m_writeBinObj.Close();
            }
        }

        internal virtual void Write(string msg)
        {
        }

        internal virtual void Write(byte[] inB)
        {
        }

        internal virtual void WriteLine(string msg)
        {
        }

        internal virtual void WriteLine(byte[] inB)
        {
        }
    }
}

