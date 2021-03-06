﻿namespace SiRFLive.Utilities
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public class sysCmdExec
    {
        public static Hashtable RunProgramList = new Hashtable();
        private const int WM_CLOSE = 0x10;

        public static void CloseWin(string winName)
        {
            IntPtr handle = FindWindow(null, winName);
            if (handle != IntPtr.Zero)
            {
                SendMessage(new HandleRef(null, handle), 0x10, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void CloseWinByProcId(int procId)
        {
            if (RunProgramList.ContainsKey(procId))
            {
                ((Process) RunProgramList[procId]).CloseMainWindow();
            }
            RunProgramList.Remove(procId);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        public static int IsExistingWin(string winName)
        {
            if (FindWindow(null, winName) == IntPtr.Zero)
            {
                return 0;
            }
            return 1;
        }

        public static Process OpenWin(string progName, string progArgs)
        {
            Process process = Process.Start(progName, progArgs);
            if (!RunProgramList.ContainsKey(process.Id))
            {
                RunProgramList.Add(process.Id, process);
            }
            return process;
        }

        [DllImport("user32.dll", SetLastError=true)]
        private static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        public static void SetWinSize(string winName, int size)
        {
            IntPtr hWnd = FindWindow(null, winName);
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, size);
            }
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}

