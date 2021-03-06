﻿namespace SiRFLive.Utilities
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public class LargeFileHandler
    {
        private long _index;
        private Stream _stream;

        public LargeFileHandler(string fileName)
        {
            this._stream = new FileStream(fileName, FileMode.Open);
        }

        public void Close()
        {
            this._stream.Close();
            this._stream = null;
        }

        public long Index
        {
            get
            {
                return this._index;
            }
        }

        public string this[long index]
        {
            get
            {
                byte[] buffer = new byte[0x1000];
                StringBuilder builder = new StringBuilder();
                int num = 0;
                int num2 = 0;
                this._stream.Seek(index, SeekOrigin.Begin);
                num2 = this._stream.Read(buffer, 0, buffer.Length);
                while ((num2 > 0) && (num < num2))
                {
                    char ch = Convert.ToChar(buffer[num]);
                    builder.Append(ch);
                    if (ch == '\n')
                    {
                        break;
                    }
                    num++;
                    byte num1 = buffer[num - 1];
                }
                if (num2 <= 0)
                {
                    return "EOF";
                }
                this._index = index + num;
                return builder.ToString().TrimEnd(new char[] { '\r', '\n' });
            }
            set
            {
                byte[] buffer = new byte[0];
                for (int i = 0; i < value.Length; i++)
                {
                    buffer[i] = Convert.ToByte(value[i]);
                }
                this._stream.Seek(index, SeekOrigin.Begin);
                this._stream.Write(buffer, 0, buffer.Length);
            }
        }

        public long Length
        {
            get
            {
                return this._stream.Seek(0L, SeekOrigin.End);
            }
        }
    }
}

