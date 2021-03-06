﻿namespace OpenNETCF.IO.Serial
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    public class Port : IDisposable
    {
        private int brk;
        public readonly CommCapabilities Capabilities;
        private IntPtr closeEvent;
        private string closeEventName;
        private DCB dcb;
        private int dtr;
        private bool dtravail;
        private Thread eventThread;
        private IntPtr hPort;
        private int inputLength;
        private bool isOpen;
        private CommAPI m_CommAPI;
        private string portName;
        private DetailedPortSettings portSettings;
        private int ptxBuffer;
        private int rthreshold;
        private int rts;
        private bool rtsavail;
        private Mutex rxBufferBusy;
        private int rxBufferSize;
        private Queue rxFIFO;
        private IntPtr rxOverlapped;
        private int setir;
        private int sthreshold;
        private ManualResetEvent threadStarted;
        private byte[] txBuffer;
        private int txBufferSize;
        private IntPtr txOverlapped;

        public event CommChangeEvent CTSChange;

        public event CommEvent DataReceived;

        public event CommChangeEvent DSRChange;

        public event CommEvent FlagCharReceived;

        public event CommEvent HighWater;

        public event CommErrorEvent OnError;

        public event CommEvent PowerEvent;

        public event CommChangeEvent RingChange;

        public event CommChangeEvent RLSDChange;

        public event CommEvent TxDone;

        public Port(string PortName)
        {
            this.hPort = (IntPtr) (-1);
            this.rxBufferSize = 0x400000;
            this.rthreshold = 1;
            this.txBufferSize = 0x400000;
            this.sthreshold = 1;
            this.rxBufferBusy = new Mutex();
            this.dcb = new DCB();
            this.threadStarted = new ManualResetEvent(false);
            this.closeEventName = "CloseEvent";
            this.txOverlapped = IntPtr.Zero;
            this.rxOverlapped = IntPtr.Zero;
            this.Capabilities = new CommCapabilities();
            this.PortName = PortName;
            this.Init();
        }

        public Port(string PortName, BasicPortSettings InitialSettings)
        {
            this.hPort = (IntPtr) (-1);
            this.rxBufferSize = 0x400000;
            this.rthreshold = 1;
            this.txBufferSize = 0x400000;
            this.sthreshold = 1;
            this.rxBufferBusy = new Mutex();
            this.dcb = new DCB();
            this.threadStarted = new ManualResetEvent(false);
            this.closeEventName = "CloseEvent";
            this.txOverlapped = IntPtr.Zero;
            this.rxOverlapped = IntPtr.Zero;
            this.Capabilities = new CommCapabilities();
            this.PortName = PortName;
            this.Init();
            this.portSettings.BasicSettings = InitialSettings;
        }

        public Port(string PortName, DetailedPortSettings InitialSettings)
        {
            this.hPort = (IntPtr) (-1);
            this.rxBufferSize = 0x400000;
            this.rthreshold = 1;
            this.txBufferSize = 0x400000;
            this.sthreshold = 1;
            this.rxBufferBusy = new Mutex();
            this.dcb = new DCB();
            this.threadStarted = new ManualResetEvent(false);
            this.closeEventName = "CloseEvent";
            this.txOverlapped = IntPtr.Zero;
            this.rxOverlapped = IntPtr.Zero;
            this.Capabilities = new CommCapabilities();
            this.PortName = PortName;
            this.Init();
            this.portSettings = InitialSettings;
        }

        public Port(string PortName, int RxBufferSize, int TxBufferSize)
        {
            this.hPort = (IntPtr) (-1);
            this.rxBufferSize = 0x400000;
            this.rthreshold = 1;
            this.txBufferSize = 0x400000;
            this.sthreshold = 1;
            this.rxBufferBusy = new Mutex();
            this.dcb = new DCB();
            this.threadStarted = new ManualResetEvent(false);
            this.closeEventName = "CloseEvent";
            this.txOverlapped = IntPtr.Zero;
            this.rxOverlapped = IntPtr.Zero;
            this.Capabilities = new CommCapabilities();
            this.rxBufferSize = RxBufferSize;
            this.txBufferSize = TxBufferSize;
            this.PortName = PortName;
            this.Init();
        }

        public Port(string PortName, BasicPortSettings InitialSettings, int RxBufferSize, int TxBufferSize)
        {
            this.hPort = (IntPtr) (-1);
            this.rxBufferSize = 0x400000;
            this.rthreshold = 1;
            this.txBufferSize = 0x400000;
            this.sthreshold = 1;
            this.rxBufferBusy = new Mutex();
            this.dcb = new DCB();
            this.threadStarted = new ManualResetEvent(false);
            this.closeEventName = "CloseEvent";
            this.txOverlapped = IntPtr.Zero;
            this.rxOverlapped = IntPtr.Zero;
            this.Capabilities = new CommCapabilities();
            this.rxBufferSize = RxBufferSize;
            this.txBufferSize = TxBufferSize;
            this.PortName = PortName;
            this.Init();
            this.portSettings.BasicSettings = InitialSettings;
        }

        public Port(string PortName, DetailedPortSettings InitialSettings, int RxBufferSize, int TxBufferSize)
        {
            this.hPort = (IntPtr) (-1);
            this.rxBufferSize = 0x400000;
            this.rthreshold = 1;
            this.txBufferSize = 0x400000;
            this.sthreshold = 1;
            this.rxBufferBusy = new Mutex();
            this.dcb = new DCB();
            this.threadStarted = new ManualResetEvent(false);
            this.closeEventName = "CloseEvent";
            this.txOverlapped = IntPtr.Zero;
            this.rxOverlapped = IntPtr.Zero;
            this.Capabilities = new CommCapabilities();
            this.rxBufferSize = RxBufferSize;
            this.txBufferSize = TxBufferSize;
            this.PortName = PortName;
            this.Init();
            this.portSettings = InitialSettings;
        }

        public bool Close()
        {
            CommErrorFlags flags;
            if (this.txOverlapped != IntPtr.Zero)
            {
                LocalFree(this.txOverlapped);
                this.txOverlapped = IntPtr.Zero;
            }
            if (this.rxOverlapped != IntPtr.Zero)
            {
                LocalFree(this.rxOverlapped);
                this.rxOverlapped = IntPtr.Zero;
            }
            if (this.hPort == IntPtr.Zero)
            {
                return false;
            }
            int num = 0;
        Label_0068:
            flags = 0;
            CommStat stat = new CommStat();
            if (!this.m_CommAPI.ClearCommError(this.hPort, ref flags, stat) || (stat.cbOutQue != 0))
            {
                Thread.Sleep(1);
                if (num < 3)
                {
                    num++;
                    Thread.Sleep(10);
                    goto Label_0068;
                }
            }
            this.m_CommAPI.PurgeComm(this.hPort, 0);
            this.m_CommAPI.CloseHandle(this.hPort);
            this.m_CommAPI.SetEvent(this.closeEvent);
            this.isOpen = false;
            this.hPort = (IntPtr) (-1);
            return true;
        }

        private void CommEventThread()
        {
            byte[] buffer = new byte[this.rxBufferSize];
            int cbRead = 0;
            new AutoResetEvent(false);
            if (CommAPI.FullFramework)
            {
                this.m_CommAPI.SetCommMask(this.hPort, CommEventFlags.ALLPC);
            }
            else
            {
                this.m_CommAPI.SetCommMask(this.hPort, CommEventFlags.ALLCE);
            }
            this.threadStarted.Set();
            StringBuilder builder = new StringBuilder("", 80);
            while (this.hPort != ((IntPtr) (-1)))
            {
                try
                {
                    CommErrorFlags flags = 0;
                    CommStat stat = new CommStat();
                    if (!this.m_CommAPI.ClearCommError(this.hPort, ref flags, stat))
                    {
                        Thread.Sleep(20);
                    }
                    else if (stat.cbInQue == 0)
                    {
                        Thread.Sleep(20);
                    }
                    else
                    {
                        builder.Append("UART Error: ");
                        if ((flags & CommErrorFlags.FRAME) != 0)
                        {
                            builder = builder.Append("Framing,");
                        }
                        if ((flags & CommErrorFlags.IOE) != 0)
                        {
                            builder = builder.Append("IO,");
                        }
                        if ((flags & CommErrorFlags.OVERRUN) != 0)
                        {
                            builder = builder.Append("Overrun,");
                        }
                        if ((flags & CommErrorFlags.RXOVER) != 0)
                        {
                            builder = builder.Append("Receive Overflow,");
                        }
                        if ((flags & CommErrorFlags.RXPARITY) != 0)
                        {
                            builder = builder.Append("Parity,");
                        }
                        if ((flags & CommErrorFlags.TXFULL) != 0)
                        {
                            builder = builder.Append("Transmit Overflow,");
                        }
                        if ((flags & CommErrorFlags.BREAK) != 0)
                        {
                            builder = builder.Append("Break,");
                        }
                        if (builder.Length == 12)
                        {
                            builder = builder.Append("Unknown");
                        }
                        if ((this.OnError != null) && (flags != 0))
                        {
                            this.OnError(builder.ToString());
                        }
                        if (stat.cbInQue >= this.rxBufferSize)
                        {
                            this.m_CommAPI.ReadFile(this.hPort, buffer, this.rxBufferSize, ref cbRead, IntPtr.Zero);
                        }
                        else
                        {
                            this.m_CommAPI.ReadFile(this.hPort, buffer, (int) stat.cbInQue, ref cbRead, IntPtr.Zero);
                        }
                        if (cbRead >= 1)
                        {
                            this.rxBufferBusy.WaitOne();
                            for (int i = 0; i < cbRead; i++)
                            {
                                this.rxFIFO.Enqueue(buffer[i]);
                            }
                            int count = this.rxFIFO.Count;
                            this.rxBufferBusy.ReleaseMutex();
                            if (((this.DataReceived != null) && (this.rthreshold != 0)) && (count >= this.rthreshold))
                            {
                                this.DataReceived();
                            }
                        }
                        builder.Remove(0, builder.Length);
                        Thread.Sleep(1);
                    }
                    continue;
                }
                catch (Exception exception)
                {
                    if (this.rxOverlapped != IntPtr.Zero)
                    {
                        LocalFree(this.rxOverlapped);
                    }
                    if (this.OnError != null)
                    {
                        this.OnError(exception.Message);
                    }
                    continue;
                }
            }
        }

        public void Dispose()
        {
            if (this.isOpen)
            {
                this.Close();
            }
        }

        ~Port()
        {
            if (this.isOpen)
            {
                this.Close();
            }
        }

        private bool GetPortProperties()
        {
            return this.m_CommAPI.GetCommProperties(this.hPort, this.Capabilities);
        }

        private void Init()
        {
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
            {
                this.m_CommAPI = new WinCommAPI();
            }
            else
            {
                this.m_CommAPI = new CECommAPI();
            }
            this.closeEvent = this.m_CommAPI.CreateEvent(true, false, this.closeEventName);
            this.rxFIFO = new Queue(this.rxBufferSize);
            this.txBuffer = new byte[this.txBufferSize];
            this.portSettings = new DetailedPortSettings();
        }

        [DllImport("kernel32", SetLastError=true)]
        internal static extern IntPtr LocalAlloc(int uFlags, int uBytes);
        [DllImport("kernel32", SetLastError=true)]
        internal static extern IntPtr LocalFree(IntPtr hMem);
        public bool Open()
        {
            if (this.isOpen)
            {
                return false;
            }
            bool fullFramework = CommAPI.FullFramework;
            string fileName = string.Format(@"\\.\{0}", this.portName);
            this.hPort = this.m_CommAPI.CreateFile(fileName);
            if (this.hPort == ((IntPtr) (-1)))
            {
                int num = Marshal.GetLastWin32Error();
                throw new CommPortException(string.Format("Error open port: {0}", (APIErrors) num));
            }
            this.m_CommAPI.PurgeComm(this.hPort, 0);
            this.m_CommAPI.GetCommState(this.hPort, this.dcb);
            this.dcb.BaudRate = (uint) this.portSettings.BasicSettings.BaudRate;
            this.dcb.ByteSize = this.portSettings.BasicSettings.ByteSize;
            this.dcb.fOutxCtsFlow = this.portSettings.OutCTS;
            this.dcb.fParity = true;
            this.dcb.fBinary = true;
            this.dcb.fRtsControl = (byte) this.portSettings.RTSControl;
            this.dcb.Parity = (byte) this.portSettings.BasicSettings.Parity;
            this.dcb.StopBits = (byte) this.portSettings.BasicSettings.StopBits;
            this.brk = 0;
            this.dtr = (this.dcb.fDtrControl == 1) ? 1 : 0;
            this.rts = (this.dcb.fRtsControl == 1) ? 1 : 0;
            CommTimeouts timeouts = new CommTimeouts();
            timeouts.ReadIntervalTimeout = uint.MaxValue;
            timeouts.ReadTotalTimeoutMultiplier = 0;
            timeouts.ReadTotalTimeoutConstant = 0x3e8;
            timeouts.WriteTotalTimeoutConstant = 0;
            timeouts.WriteTotalTimeoutMultiplier = 0x4b00 / this.dcb.BaudRate;
            this.m_CommAPI.SetCommState(this.hPort, this.dcb);
            this.m_CommAPI.SetCommTimeouts(this.hPort, timeouts);
            this.m_CommAPI.SetupComm(this.hPort, 0x1000, 0x1000);
            this.m_CommAPI.SetupComm(this.hPort, this.rxBufferSize, this.txBufferSize);
            this.GetPortProperties();
            Thread.Sleep(100);
            this.isOpen = true;
            this.eventThread = new Thread(new ThreadStart(this.CommEventThread));
            this.eventThread.IsBackground = true;
            this.eventThread.Priority = ThreadPriority.Highest;
            this.eventThread.Start();
            this.threadStarted.WaitOne();
            return true;
        }

        public bool Query()
        {
            if (this.isOpen)
            {
                return false;
            }
            this.hPort = this.m_CommAPI.QueryFile(this.portName);
            if (this.hPort == ((IntPtr) (-1)))
            {
                int num = Marshal.GetLastWin32Error();
                if (num != 5)
                {
                    throw new CommPortException(string.Format("CreateFile Failed: {0}", num));
                }
                return false;
            }
            this.GetPortProperties();
            return true;
        }

        public bool ToggleRTS()
        {
            this.m_CommAPI.PurgeComm(this.hPort, 0);
            bool commState = this.m_CommAPI.GetCommState(this.hPort, this.dcb);
            byte fRtsControl = this.dcb.fRtsControl;
            byte num2 = (fRtsControl == 1) ? ((byte) 0) : ((byte) 1);
            this.dcb.fRtsControl = num2;
            commState = this.m_CommAPI.SetCommState(this.hPort, this.dcb);
            Thread.Sleep(10);
            this.dcb.fRtsControl = fRtsControl;
            commState = this.m_CommAPI.SetCommState(this.hPort, this.dcb);
            this.m_CommAPI.PurgeComm(this.hPort, 0);
            return commState;
        }

        public bool UpdateBaud(uint baud)
        {
            this.m_CommAPI.PurgeComm(this.hPort, 0);
            bool commState = this.m_CommAPI.GetCommState(this.hPort, this.dcb);
            this.dcb.BaudRate = baud;
            commState = this.m_CommAPI.SetCommState(this.hPort, this.dcb);
            this.m_CommAPI.PurgeComm(this.hPort, 0);
            this.rxBufferBusy.WaitOne();
            this.rxFIFO.Clear();
            this.rxBufferBusy.ReleaseMutex();
            return commState;
        }

        public void UpdatePortSettings(DetailedPortSettings mySettings)
        {
            this.portSettings = mySettings;
        }

        public bool UpdateSettings()
        {
            if (!this.isOpen)
            {
                return false;
            }
            this.m_CommAPI.PurgeComm(this.hPort, 0);
            bool commState = this.m_CommAPI.GetCommState(this.hPort, this.dcb);
            this.dcb.BaudRate = (uint) this.portSettings.BasicSettings.BaudRate;
            this.dcb.ByteSize = this.portSettings.BasicSettings.ByteSize;
            this.dcb.fBinary = true;
            this.dcb.fOutxCtsFlow = this.portSettings.OutCTS;
            this.dcb.fParity = true;
            this.dcb.fRtsControl = (byte) this.portSettings.RTSControl;
            this.dcb.Parity = (byte) this.portSettings.BasicSettings.Parity;
            this.dcb.StopBits = (byte) this.portSettings.BasicSettings.StopBits;
            commState = this.m_CommAPI.SetCommState(this.hPort, this.dcb);
            this.m_CommAPI.PurgeComm(this.hPort, 0);
            this.rxBufferBusy.WaitOne();
            this.rxFIFO.Clear();
            this.rxBufferBusy.ReleaseMutex();
            return commState;
        }

        public bool Break
        {
            get
            {
                if (!this.isOpen)
                {
                    return false;
                }
                return (this.brk == 1);
            }
            set
            {
                if ((this.isOpen && (this.brk >= 0)) && (this.hPort != ((IntPtr) (-1))))
                {
                    if (value)
                    {
                        if (!this.m_CommAPI.EscapeCommFunction(this.hPort, CommEscapes.SETBREAK))
                        {
                            throw new CommPortException("Failed to set break!");
                        }
                        this.brk = 1;
                    }
                    else
                    {
                        if (!this.m_CommAPI.EscapeCommFunction(this.hPort, CommEscapes.CLRBREAK))
                        {
                            throw new CommPortException("Failed to clear break!");
                        }
                        this.brk = 0;
                    }
                }
            }
        }

        public DetailedPortSettings DetailedSettings
        {
            get
            {
                return this.portSettings;
            }
            set
            {
                this.portSettings = value;
                this.UpdateSettings();
            }
        }

        public bool DTRAvailable
        {
            get
            {
                return this.dtravail;
            }
        }

        public bool DTREnable
        {
            get
            {
                return (this.dtr == 1);
            }
            set
            {
                if ((this.dtr >= 0) && (this.hPort != ((IntPtr) (-1))))
                {
                    if (value)
                    {
                        if (!this.m_CommAPI.EscapeCommFunction(this.hPort, CommEscapes.SETDTR))
                        {
                            throw new CommPortException("Failed to set DTR!");
                        }
                        this.dtr = 1;
                    }
                    else
                    {
                        if (!this.m_CommAPI.EscapeCommFunction(this.hPort, CommEscapes.CLRDTR))
                        {
                            throw new CommPortException("Failed to clear DTR!");
                        }
                        this.dtr = 0;
                    }
                }
            }
        }

        public int InBufferCount
        {
            get
            {
                if (!this.isOpen)
                {
                    return 0;
                }
                return this.rxFIFO.Count;
            }
        }

        public byte[] Input
        {
            get
            {
                if (!this.isOpen)
                {
                    return null;
                }
                int count = 0;
                this.rxBufferBusy.WaitOne();
                if (this.inputLength == 0)
                {
                    count = this.rxFIFO.Count;
                }
                else
                {
                    count = (this.inputLength < this.rxFIFO.Count) ? this.inputLength : this.rxFIFO.Count;
                }
                byte[] buffer = new byte[count];
                for (int i = 0; i < count; i++)
                {
                    buffer[i] = (byte) this.rxFIFO.Dequeue();
                }
                this.rxBufferBusy.ReleaseMutex();
                return buffer;
            }
        }

        public int InputLen
        {
            get
            {
                return this.inputLength;
            }
            set
            {
                this.inputLength = value;
            }
        }

        public bool IREnable
        {
            get
            {
                return (this.setir == 1);
            }
            set
            {
                if ((this.setir >= 0) && (this.hPort != ((IntPtr) (-1))))
                {
                    if (value)
                    {
                        if (!this.m_CommAPI.EscapeCommFunction(this.hPort, CommEscapes.SETIR))
                        {
                            throw new CommPortException("Failed to set IR!");
                        }
                        this.setir = 1;
                    }
                    else
                    {
                        if (!this.m_CommAPI.EscapeCommFunction(this.hPort, CommEscapes.CLRIR))
                        {
                            throw new CommPortException("Failed to clear IR!");
                        }
                        this.setir = 0;
                    }
                }
            }
        }

        public bool IsOpen
        {
            get
            {
                return this.isOpen;
            }
        }

        public int OutBufferCount
        {
            get
            {
                if (!this.isOpen)
                {
                    return 0;
                }
                return this.ptxBuffer;
            }
        }

        public byte[] Output
        {
            set
            {
                if (!this.isOpen)
                {
                    throw new CommPortException("Port not open");
                }
                int cbWritten = 0;
                if (value.GetLength(0) > this.sthreshold)
                {
                    if (this.ptxBuffer > 0)
                    {
                        this.m_CommAPI.WriteFile(this.hPort, this.txBuffer, this.ptxBuffer, ref cbWritten, this.txOverlapped);
                        this.ptxBuffer = 0;
                    }
                    this.m_CommAPI.WriteFile(this.hPort, value, value.GetLength(0), ref cbWritten, this.txOverlapped);
                }
                else
                {
                    value.CopyTo(this.txBuffer, this.ptxBuffer);
                    this.ptxBuffer += value.Length;
                    if (this.ptxBuffer >= this.sthreshold)
                    {
                        this.m_CommAPI.WriteFile(this.hPort, this.txBuffer, this.ptxBuffer, ref cbWritten, this.txOverlapped);
                        this.ptxBuffer = 0;
                    }
                }
            }
        }

        public string PortName
        {
            get
            {
                return this.portName;
            }
            set
            {
                if (!CommAPI.FullFramework && !value.EndsWith(":"))
                {
                    this.portName = value + ":";
                }
                else
                {
                    this.portName = value;
                }
            }
        }

        public int RThreshold
        {
            get
            {
                return this.rthreshold;
            }
            set
            {
                this.rthreshold = value;
            }
        }

        public bool RTSAvailable
        {
            get
            {
                return this.rtsavail;
            }
        }

        public bool RTSEnable
        {
            get
            {
                return (this.rts == 1);
            }
            set
            {
                if ((this.rts >= 0) && (this.hPort != ((IntPtr) (-1))))
                {
                    if (value)
                    {
                        if (!this.m_CommAPI.EscapeCommFunction(this.hPort, CommEscapes.SETRTS))
                        {
                            throw new CommPortException("Failed to set RTS!");
                        }
                        this.rts = 1;
                    }
                    else
                    {
                        if (!this.m_CommAPI.EscapeCommFunction(this.hPort, CommEscapes.CLRRTS))
                        {
                            throw new CommPortException("Failed to clear RTS!");
                        }
                        this.rts = 0;
                    }
                }
            }
        }

        public BasicPortSettings Settings
        {
            get
            {
                return this.portSettings.BasicSettings;
            }
            set
            {
                this.portSettings.BasicSettings = value;
                this.UpdateSettings();
            }
        }

        public int SThreshold
        {
            get
            {
                return this.sthreshold;
            }
            set
            {
                this.sthreshold = value;
            }
        }

        public delegate void CommChangeEvent(bool NewState);

        public delegate void CommErrorEvent(string Description);

        public delegate void CommEvent();
    }
}

