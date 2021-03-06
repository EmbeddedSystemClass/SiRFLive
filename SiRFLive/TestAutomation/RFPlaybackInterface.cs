﻿namespace SiRFLive.TestAutomation
{
    using System;
    using System.Runtime.InteropServices;

    public class RFPlaybackInterface
    {
        public int CAPTURE;
        public uint CPBK_ERROR_ID = 1;
        public string CPBK_ERROR_STRING = "NO_ERROR";
        public uint EXACQ_ERROR_ID = 1;
        public string EXACQ_ERROR_STRING = "NO_ERROR";
        public bool IsEntiredFile;
        public bool IsFinished;
        public bool IsLocal = true;
        public bool IsManual = true;
        public int PLAYBACK = 1;
        public uint TotalFileTime;
        public uint TotalPlaySize;

        public int Config(int[] configParams)
        {
            return fnRFReplayConfig(configParams);
        }

        public string ErrorToString(int errorCode)
        {
            switch (errorCode)
            {
                case -63:
                    return "E_GENERIC__INCORRECT_PARAMETERS";

                case -62:
                    return "E_GENERIC__REMOTE_DISCONNECTION";

                case -61:
                    return "E_GENERIC__LOCAL_DISCONNECTION";

                case -60:
                    return "E_GENERIC__TOO_MANY_VALUES";

                case -59:
                    return "E_GENERIC__INTERNAL_ERROR";

                case -58:
                    return "E_GENERIC__TIMED_OUT";

                case -57:
                    return "E_GENERIC__DISCONNECTION_FAILED";

                case -56:
                    return "E_GENERIC__WRITE_ERROR";

                case -55:
                    return "E_GENERIC__READ_ERROR";

                case -54:
                    return "E_GENERIC__DISCONNECTED";

                case -53:
                    return "E_GENERIC__CANCELLED";

                case -52:
                    return "E_GENERIC__ABORTED";

                case -51:
                    return "E_GENERIC__CONNECTION_FAILED";

                case -50:
                    return "E_GENERIC__SEEK_ERROR";

                case -49:
                    return "E_GENERIC__BAD_FORMAT";

                case -48:
                    return "E_GENERIC__ACCESS_REJECTED";

                case -47:
                    return "E_GENERIC__RESOURCE_CREATION_FAILED";

                case -46:
                    return "E_GENERIC__BUFFER_FULL";

                case -45:
                    return "E_GENERIC__NO_DATA";

                case -44:
                    return "E_GENERIC__NOT_CONNECTED";

                case -43:
                    return "E_GENERIC__CONNECTED";

                case -42:
                    return "E_GENERIC__RANGE_END";

                case -41:
                    return "E_GENERIC__PARSING_ERROR";

                case -40:
                    return "E_GENERIC__SIZE_MISMATCH";

                case -39:
                    return "E_GENERIC__REQD_INFO_MISSING";

                case -38:
                    return "E_GENERIC__STACK_INITIALIZATION_FAILED";

                case -37:
                    return "E_GENERIC__ALREADY_STARTED";

                case -36:
                    return "E_GENERIC__NOT_IN_USE";

                case -35:
                    return "E_GENERIC__IN_USE";

                case -34:
                    return "E_GENERIC__NOT_SUPPORTED";

                case -33:
                    return "E_GENERIC__ILLEGAL_VALUE";

                case -32:
                    return "E_GENERIC__END_OF_DATA";

                case -31:
                    return "E_GENERIC__BAD_DATA_DESC";

                case -30:
                    return "E_GENERIC__ALREADY_INITIALIZED";

                case -29:
                    return "E_GENERIC__INVALID_STATE";

                case -28:
                    return "E_GENERIC__LP_DISCONNECTION";

                case -27:
                    return "E_GENERIC__SIZE_EXCEEDED";

                case -26:
                    return "E_GENERIC__FILE_WRITE_ERRROR";

                case -25:
                    return "E_GENERIC__NOT_CACHED";

                case -24:
                    return "E_GENERIC__COULD_NOT_ADD_TO_CACHE";

                case -23:
                    return "E_GENERIC__NO_PARENT";

                case -22:
                    return "E_GENERIC__PERMISSION_DENIED";

                case -21:
                    return "E_GENERIC__ACCESS_ERROR";

                case -20:
                    return "E_GENERIC__ACCESS_DENIED";

                case -19:
                    return "E_GENERIC__FILE_READ_ERROR";

                case -18:
                    return "E_GENERIC__COULD_NOT_CREATE_TEMP_FILE";

                case -17:
                    return "E_GENERIC__MBCS_TO_UCS_FAILED";

                case -16:
                    return "E_GENERIC__UCS_TO_MBCS_FAILED";

                case -15:
                    return "E_GENERIC__ALREADY_CLOSED";

                case -14:
                    return "E_GENERIC__INITIALIZATION_FAILED";

                case -13:
                    return "E_GENERIC__NOT_INITIALIZED";

                case -12:
                    return "E_GENERIC__FILE_NOT_FOUND";

                case -11:
                    return "E_GENERIC__OPERATION_FAILED";

                case -10:
                    return "E_GENERIC__ILLEGAL_OPERATION";

                case -9:
                    return "E_GENERIC__DEVICE_UNAVAILABLE";

                case -8:
                    return "E_GENERIC__BAD_ARGUMENTS";

                case -7:
                    return "E_GENERIC__NOT_ENOUGH_MEMORY";

                case -6:
                    return "E_GENERIC__NULL_POINTER";

                case -5:
                    return "E_GENERIC__NOT_FOUND";

                case -4:
                    return "E_GENERIC__NOT_IMPLEMENTED";

                case -3:
                    return "E_GENERIC__BAD_COPY_FLAG";

                case -2:
                    return "E_GENERIC__ALREADY_EXISTS";

                case -1:
                    return "E_GENERIC__FAIL";

                case 0:
                    return "E_GENERIC__FALSE";

                case 1:
                    return "E_GENERIC__OK";

                case 2:
                    return "E_GENERIC__SKIP";

                case 3:
                    return "E_GENERIC__STOP";

                case 4:
                    return "E_GENERIC__CONTINUE";

                case 5:
                    return "E_GENERIC__END";

                case 6:
                    return "E_GENERIC__WAIT";

                case 7:
                    return "E_GENERIC_DISK_FILLED";
            }
            return "UNKNOWN_ERROR";
        }

        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRFReplayConfig([In] int[] inArray);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRFReplayDLL(int type);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRFReplayGetTotalTime([In, Out] byte[] timeInfo, [In] char[] filePath);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRFReplayInit(int device);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRFReplaySetFile([In] char[] filePath, int type);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRFReplaySetMan(int type);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRFReplaySetSpace(uint size, int type);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRFReplaySetTime([In] int[] inTime, int type);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRFReplayStart(int type);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRFReplayStop(int type);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnUnregisterAllCallbacks(int type);
        public int GetTotalTime(byte[] timeInfo, string filePath)
        {
            int num = fnRFReplayGetTotalTime(timeInfo, filePath.ToCharArray());
            this.TotalFileTime = this.GetTotalTimeInSec(timeInfo);
            return num;
        }

        public uint GetTotalTimeInSec(byte[] timeInfo)
        {
            return (uint) (((timeInfo[0] * 0xe10) + (timeInfo[1] * 60)) + timeInfo[2]);
        }

        public int Init(int deviceId)
        {
            return fnRFReplayInit(deviceId);
        }

        public static byte[] RawSerialize(object anything)
        {
            int cb = Marshal.SizeOf(anything);
            IntPtr ptr = Marshal.AllocHGlobal(cb);
            Marshal.StructureToPtr(anything, ptr, false);
            byte[] destination = new byte[cb];
            Marshal.Copy(ptr, destination, 0, cb);
            Marshal.FreeHGlobal(ptr);
            return destination;
        }

        public int SetFile(string filePath, int type)
        {
            return fnRFReplaySetFile(filePath.ToCharArray(), type);
        }

        public int SetManual(int type)
        {
            return fnRFReplaySetMan(type);
        }

        public int SetSpace(uint size, int type)
        {
            this.TotalPlaySize = size;
            return fnRFReplaySetSpace(size, type);
        }

        public int SetTime(int[] inTime, int type)
        {
            return fnRFReplaySetTime(inTime, type);
        }

        public int Start(int type)
        {
            return fnRFReplayStart(type);
        }

        public int Stop(int type)
        {
            return fnRFReplayStop(type);
        }

        public int UnregisterCallbacks(int type)
        {
            return fnUnregisterAllCallbacks(type);
        }
    }
}

