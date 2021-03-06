﻿namespace OpenNETCF.IO.Serial
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading;

    public class CommWrapper : IDisposable
    {
        private bool isDisposed;
        public Port port;
        public DetailedPortSettings portSettings = new HandshakeNone();

        public CommWrapper()
        {
            this.port = new Port("COM1", this.portSettings);
            this.port.InputLen = 0;
        }

        public bool Close()
        {
            return this.port.Close();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
            {
                this.port.Dispose();
                this.port = null;
            }
            this.isDisposed = true;
        }

        ~CommWrapper()
        {
            this.Dispose(false);
        }

        public static string[] GetPortNames()
        {
            List<string> list = new List<string>();
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Hardware\DeviceMap\SerialComm");
            foreach (string str in key.GetValueNames())
            {
                string input = key.GetValue(str).ToString();
                if ((input != null) && input.Contains("COM"))
                {
                    string pattern = @"(?<myCom>COM\d+)";
                    Regex regex = new Regex(pattern, RegexOptions.Compiled);
                    if (regex.IsMatch(input))
                    {
                        list.Add(regex.Match(input).Result("${myCom}"));
                    }
                }
            }
            return list.ToArray();
        }

        public bool Open()
        {
            this.port.UpdatePortSettings(this.portSettings);
            return this.port.Open();
        }

        public byte[] Read()
        {
            return this.port.Input;
        }

        public bool SetRTS(bool status)
        {
            this.port.RTSEnable = status;
            return true;
        }

        public bool ToggleRTS()
        {
            this.port.RTSEnable = true;
            Thread.Sleep(10);
            this.port.RTSEnable = false;
            return true;
        }

        public bool UpdateBaudSettings(uint baud)
        {
            return this.port.UpdateBaud(baud);
        }

        public bool UpdateSetttings()
        {
            this.port.UpdatePortSettings(this.portSettings);
            return this.port.UpdateSettings();
        }

        public void Write(byte[] inBuf, int index, int numBytes)
        {
            byte[] destinationArray = new byte[numBytes];
            Array.Copy(inBuf, index, destinationArray, 0, numBytes);
            this.port.Output = destinationArray;
        }

        public int BaudRate
        {
            get
            {
                return (int) this.portSettings.BasicSettings.BaudRate;
            }
            set
            {
                this.portSettings.BasicSettings.BaudRate = (BaudRates) value;
            }
        }

        public int BytesToRead
        {
            get
            {
                return this.port.InBufferCount;
            }
        }

        public int DataBits
        {
            get
            {
                return this.portSettings.BasicSettings.ByteSize;
            }
            set
            {
                this.portSettings.BasicSettings.ByteSize = (byte) value;
            }
        }

        public DetailedPortSettings Handshake
        {
            get
            {
                return this.portSettings;
            }
            set
            {
                this.portSettings = value;
            }
        }

        public bool IsOpen
        {
            get
            {
                return this.port.IsOpen;
            }
        }

        public OpenNETCF.IO.Serial.Parity Parity
        {
            get
            {
                return this.portSettings.BasicSettings.Parity;
            }
            set
            {
                this.portSettings.BasicSettings.Parity = value;
            }
        }

        public string PortName
        {
            get
            {
                return this.port.PortName;
            }
            set
            {
                this.port.PortName = value;
            }
        }

        public OpenNETCF.IO.Serial.StopBits StopBits
        {
            get
            {
                return this.portSettings.BasicSettings.StopBits;
            }
            set
            {
                this.portSettings.BasicSettings.StopBits = value;
            }
        }
    }
}

