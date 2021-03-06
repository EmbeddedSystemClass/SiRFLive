﻿namespace OpenNETCF.IO.Serial
{
    using System;
    using System.Runtime.InteropServices;

    internal class WinCommAPI : CommAPI
    {
        internal override bool ClearCommError(IntPtr hPort, ref CommErrorFlags flags, CommStat stat)
        {
            return Convert.ToBoolean(WinClearCommError(hPort, ref flags, stat));
        }

        internal override bool CloseHandle(IntPtr hPort)
        {
            return Convert.ToBoolean(WinCloseHandle(hPort));
        }

        internal override IntPtr CreateEvent(bool bManualReset, bool bInitialState, string lpName)
        {
            return WinCreateEvent(IntPtr.Zero, Convert.ToInt32(bManualReset), Convert.ToInt32(bInitialState), lpName);
        }

        internal override IntPtr CreateFile(string FileName)
        {
            return WinCreateFileW(FileName, 0xc0000000, 0, IntPtr.Zero, 3, 0x80, IntPtr.Zero);
        }

        internal override bool EscapeCommFunction(IntPtr hPort, CommEscapes escape)
        {
            return Convert.ToBoolean(WinEscapeCommFunction(hPort, (uint) escape));
        }

        internal override bool GetCommModemStatus(IntPtr hPort, ref uint lpModemStat)
        {
            return Convert.ToBoolean(WinGetCommModemStatus(hPort, ref lpModemStat));
        }

        internal override bool GetCommProperties(IntPtr hPort, CommCapabilities commcap)
        {
            return Convert.ToBoolean(WinGetCommProperties(hPort, commcap));
        }

        internal override bool GetCommState(IntPtr hPort, DCB dcb)
        {
            return Convert.ToBoolean(WinGetCommState(hPort, dcb));
        }

        internal override bool GetOverlappedResult(IntPtr hPort, IntPtr lpOverlapped, out int lpNumberOfBytesTransferred, bool bWait)
        {
            return WinGetOverlappedResult(hPort, lpOverlapped, out lpNumberOfBytesTransferred, bWait);
        }

        internal override bool PulseEvent(IntPtr hEvent)
        {
            return Convert.ToBoolean(WinPulseEvent(hEvent));
        }

        internal override int PurgeComm(IntPtr hPort, int dwFlags)
        {
            return WinPurgeComm(hPort, 15);
        }

        internal override IntPtr QueryFile(string FileName)
        {
            return WinCreateFileW(FileName, 0, 0, IntPtr.Zero, 3, 0x40000000, IntPtr.Zero);
        }

        internal override bool ReadFile(IntPtr hPort, byte[] buffer, int cbToRead, ref int cbRead, IntPtr lpOverlapped)
        {
            return Convert.ToBoolean(WinReadFile(hPort, buffer, cbToRead, ref cbRead, lpOverlapped));
        }

        internal override bool ResetEvent(IntPtr hEvent)
        {
            return Convert.ToBoolean(WinResetEvent(hEvent));
        }

        internal override bool SetCommMask(IntPtr hPort, CommEventFlags dwEvtMask)
        {
            return Convert.ToBoolean(WinSetCommMask(hPort, dwEvtMask));
        }

        internal override bool SetCommState(IntPtr hPort, DCB dcb)
        {
            return Convert.ToBoolean(WinSetCommState(hPort, dcb));
        }

        internal override bool SetCommTimeouts(IntPtr hPort, CommTimeouts timeouts)
        {
            return Convert.ToBoolean(WinSetCommTimeouts(hPort, timeouts));
        }

        internal override bool SetEvent(IntPtr hEvent)
        {
            return Convert.ToBoolean(WinSetEvent(hEvent));
        }

        internal override bool SetupComm(IntPtr hPort, int dwInQueue, int dwOutQueue)
        {
            return Convert.ToBoolean(WinSetupComm(hPort, dwInQueue, dwOutQueue));
        }

        internal override bool WaitCommEvent(IntPtr hPort, ref CommEventFlags flags)
        {
            return Convert.ToBoolean(WinWaitCommEvent(hPort, ref flags, IntPtr.Zero));
        }

        internal override int WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds)
        {
            return WinWaitForSingleObject(hHandle, dwMilliseconds);
        }

        [DllImport("kernel32.dll", EntryPoint="ClearCommError", SetLastError=true)]
        private static extern int WinClearCommError(IntPtr hFile, ref CommErrorFlags lpErrors, CommStat lpStat);
        [DllImport("kernel32.dll", EntryPoint="CloseHandle", SetLastError=true)]
        private static extern int WinCloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", EntryPoint="CreateEvent", SetLastError=true)]
        private static extern IntPtr WinCreateEvent(IntPtr lpEventAttributes, int bManualReset, int bInitialState, string lpName);
        [DllImport("kernel32.dll", EntryPoint="CreateFileW", CharSet=CharSet.Unicode, SetLastError=true)]
        private static extern IntPtr WinCreateFileW(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        [DllImport("kernel32.dll", EntryPoint="EscapeCommFunction", SetLastError=true)]
        private static extern int WinEscapeCommFunction(IntPtr hFile, uint dwFunc);
        [DllImport("kernel32.dll", EntryPoint="GetCommModemStatus", SetLastError=true)]
        private static extern int WinGetCommModemStatus(IntPtr hFile, ref uint lpModemStat);
        [DllImport("kernel32.dll", EntryPoint="GetCommProperties", SetLastError=true)]
        private static extern int WinGetCommProperties(IntPtr hFile, CommCapabilities commcap);
        [DllImport("kernel32.dll", EntryPoint="GetCommState", SetLastError=true)]
        private static extern int WinGetCommState(IntPtr hFile, DCB dcb);
        [DllImport("kernel32.dll", EntryPoint="GetOverlappedResult", SetLastError=true)]
        private static extern bool WinGetOverlappedResult(IntPtr hFile, IntPtr lpOverlapped, out int lpNumberOfBytesTransferred, bool bWait);
        [DllImport("kernel32.dll", EntryPoint="PulseEvent", SetLastError=true)]
        private static extern int WinPulseEvent(IntPtr hEvent);
        [DllImport("kernel32.dll", EntryPoint="PurgeComm", SetLastError=true)]
        private static extern int WinPurgeComm(IntPtr hFile, int dwFlags);
        [DllImport("kernel32.dll", EntryPoint="ReadFile", SetLastError=true)]
        private static extern int WinReadFile(IntPtr hFile, byte[] lpBuffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", EntryPoint="ResetEvent", SetLastError=true)]
        private static extern int WinResetEvent(IntPtr hEvent);
        [DllImport("kernel32.dll", EntryPoint="SetCommMask", SetLastError=true)]
        private static extern int WinSetCommMask(IntPtr handle, CommEventFlags dwEvtMask);
        [DllImport("kernel32.dll", EntryPoint="SetCommState", SetLastError=true)]
        private static extern int WinSetCommState(IntPtr hFile, DCB dcb);
        [DllImport("kernel32.dll", EntryPoint="SetCommTimeouts", SetLastError=true)]
        private static extern int WinSetCommTimeouts(IntPtr hFile, CommTimeouts timeouts);
        [DllImport("kernel32.dll", EntryPoint="SetEvent", SetLastError=true)]
        private static extern int WinSetEvent(IntPtr hEvent);
        [DllImport("kernel32.dll", EntryPoint="SetupComm", SetLastError=true)]
        private static extern int WinSetupComm(IntPtr hFile, int dwInQueue, int dwOutQueue);
        [DllImport("kernel32.dll", EntryPoint="WaitCommEvent", SetLastError=true)]
        private static extern int WinWaitCommEvent(IntPtr hFile, ref CommEventFlags lpEvtMask, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", EntryPoint="WaitForSingleObject", SetLastError=true)]
        private static extern int WinWaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);
        [DllImport("kernel32.dll", EntryPoint="WriteFile", SetLastError=true)]
        private static extern int WinWriteFile(IntPtr hFile, byte[] lpBuffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, IntPtr lpOverlapped);
        internal override bool WriteFile(IntPtr hPort, byte[] buffer, int cbToWrite, ref int cbWritten, IntPtr lpOverlapped)
        {
            return Convert.ToBoolean(WinWriteFile(hPort, buffer, cbToWrite, ref cbWritten, lpOverlapped));
        }
    }
}

