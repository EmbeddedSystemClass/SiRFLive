﻿namespace LogManagerClassLibrary
{
    using System;
    using System.IO;

    internal class writeText : writeModule
    {
        internal writeText(StreamWriter st)
        {
            base.m_writeTextObj = st;
        }

        internal override void Write(string msg)
        {
            base.m_writeTextObj.Write(msg);
        }

        internal override void WriteLine(string msg)
        {
            base.m_writeTextObj.WriteLine(msg);
        }
    }
}

