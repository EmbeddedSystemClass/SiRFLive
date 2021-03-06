﻿namespace System.Windows.Forms
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public class MessageBoxEx
    {
        public const int DM_GETDEFID = 0x400;
        private static IntPtr hHook = IntPtr.Zero;
        private static string hookCaption = null;
        private static HookProc hookProc = new HookProc(MessageBoxEx.MessageBoxHookProc);
        private static uint hookTimeout = 0;
        private static TimerProc hookTimer = new TimerProc(MessageBoxEx.MessageBoxTimerProc);
        private const int TimerID = 0x2a;
        public const int WH_CALLWNDPROCRET = 12;
        public const int WM_DESTROY = 2;
        public const int WM_INITDIALOG = 0x110;
        public const int WM_TIMER = 0x113;
        public const int WM_USER = 0x400;

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(MessageBoxEx.hHook, nCode, wParam, lParam);
            }
            CWPRETSTRUCT cwpretstruct = (CWPRETSTRUCT) Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hHook = MessageBoxEx.hHook;
            if ((hookCaption != null) && (cwpretstruct.message == 0x110))
            {
                StringBuilder text = new StringBuilder(GetWindowTextLength(cwpretstruct.hwnd) + 1);
                GetWindowText(cwpretstruct.hwnd, text, text.Capacity);
                if (hookCaption == text.ToString())
                {
                    hookCaption = null;
                    SetTimer(cwpretstruct.hwnd, (UIntPtr) 0x2a, hookTimeout, hookTimer);
                    UnhookWindowsHookEx(MessageBoxEx.hHook);
                    MessageBoxEx.hHook = IntPtr.Zero;
                }
            }
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        private static void MessageBoxTimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIDEvent, uint dwTime)
        {
            if (nIDEvent == ((UIntPtr) 0x2a))
            {
                short num = (short) ((int) SendMessage(hWnd, 0x400, IntPtr.Zero, IntPtr.Zero));
                EndDialog(hWnd, (IntPtr) num);
            }
        }

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll")]
        public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerProc lpTimerFunc);
        private static void Setup(string caption, uint uTimeout)
        {
            if (hHook != IntPtr.Zero)
            {
                throw new NotSupportedException("multiple calls are not supported");
            }
            hookTimeout = uTimeout;
            hookCaption = (caption != null) ? caption : "";
            hHook = SetWindowsHookEx(12, hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        public static DialogResult Show(string text, uint uTimeout)
        {
            Setup("", uTimeout);
            return MessageBox.Show(text);
        }

        public static DialogResult Show(string text, string caption, uint uTimeout)
        {
            Setup(caption, uTimeout);
            return MessageBox.Show(text, caption);
        }

        public static DialogResult Show(IWin32Window owner, string text, uint uTimeout)
        {
            Setup("", uTimeout);
            return MessageBox.Show(owner, text);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, uint uTimeout)
        {
            Setup(caption, uTimeout);
            return MessageBox.Show(text, caption, buttons);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, uint uTimeout)
        {
            Setup(caption, uTimeout);
            return MessageBox.Show(owner, text, caption);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, uint uTimeout)
        {
            Setup(caption, uTimeout);
            return MessageBox.Show(text, caption, buttons, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, uint uTimeout)
        {
            Setup(caption, uTimeout);
            return MessageBox.Show(owner, text, caption, buttons);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, uint uTimeout)
        {
            Setup(caption, uTimeout);
            return MessageBox.Show(text, caption, buttons, icon, defButton);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, uint uTimeout)
        {
            Setup(caption, uTimeout);
            return MessageBox.Show(owner, text, caption, buttons, icon);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options, uint uTimeout)
        {
            Setup(caption, uTimeout);
            return MessageBox.Show(text, caption, buttons, icon, defButton, options);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, uint uTimeout)
        {
            Setup(caption, uTimeout);
            return MessageBox.Show(owner, text, caption, buttons, icon, defButton);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options, uint uTimeout)
        {
            Setup(caption, uTimeout);
            return MessageBox.Show(owner, text, caption, buttons, icon, defButton, options);
        }

        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr idHook);

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            public IntPtr lResult;
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        }

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIDEvent, uint dwTime);
    }
}

