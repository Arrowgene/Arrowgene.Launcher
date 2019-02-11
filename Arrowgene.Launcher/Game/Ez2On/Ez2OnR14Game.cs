using Arrowgene.Launcher.Translation;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Arrowgene.Launcher.Game.Ez2On
{
    public class Ez2OnR14Game : Ez2OnGame
    {
        [Flags]
        private enum FileMapProtection : uint
        {
            PageReadonly = 0x2,
            PageReadWrite = 0x4,
            PageWriteCopy = 0x8,
            PageExecuteRead = 0x20,
            PageExecuteReadWrite = 0x40,
            SectionCommit = 0x8000000,
            SectionImage = 0x1000000,
            SectionNoCache = 0x10000000,
            SectionReserve = 0x4000000
        }

        [Flags]
        private enum FileMapAccess : uint
        {
            FileMapCopy = 0x1,
            FileMapWrite = 0x2,
            FileMapRead = 0x4,
            FileMapAllAccess = 0x1F,
            FileMapExecute = 0x20
        }

        private const int MAP_SIZE = 1062;
        private const string MAP_NAME = "EZTOSHR";

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFileMapping(IntPtr hFile, object lpFileMappingAttributes, FileMapProtection flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, [MarshalAs(UnmanagedType.LPStr)] string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, FileMapAccess dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

        [DllImport("Kernel32.dll")]
        private static extern bool UnmapViewOfFile(IntPtr map);

        [DllImport("kernel32.dll")]
        private static extern int CloseHandle(IntPtr hObject);

        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        private IntPtr mapFile;
        private IntPtr mapBuffer;

        public override GameType Type => GameType.Ez2OnR14;

        public override void Start()
        {
            App.Logger.Log("Trace", "Ez2OnR14Game::Start");
            Launch(base.Account, base.Hash);
        }

        private void Launch(string account, string hash)
        {
            App.Logger.Log("Trace", "Ez2OnR14Game::Launch");
            if (!base.ExecutableExists)
            {
                App.DisplayError(string.Format(Translator.Instance.Translate("can_not_find_executable"), base.ExecutablePath), "Ez2OnR14Game::Launch");
                return;
            }
            IPAddress ipAddress = GetIpAddress();
            if (ipAddress == null)
            {
                App.DisplayError(string.Format(Translator.Instance.Translate("failed_to_resolve_ip"), base.Host), "Ez2OnR14Game::Launch");
                return;
            }
            App.Logger.Log($"Mapping - IP: {ipAddress} Port: {base.Port}", "Ez2OnR14Game::Launch");
            SetWindowMode(base.WindowMode);
            App.Logger.Log($"Starting Account: {account}", "Ez2OnR14Game::Launch");
            MapNative(ipAddress.ToString(), base.Port.ToString(), account, hash);
            StartProcess(base.Executable.FullName, "", base.Executable.DirectoryName);
        }

        private void MapNative(string ipAddress, string port, string account, string hash)
        {
            App.Logger.Log("Trace", "Ez2OnR14Game::MapNative");
            byte[] mapData = new byte[MAP_SIZE];

            byte[] bIp = Encoding.UTF8.GetBytes(ipAddress);
            byte[] bPort = Encoding.UTF8.GetBytes(port);
            byte[] bParameters = Encoding.UTF8.GetBytes($"{account}|{hash}");

            Buffer.BlockCopy(bParameters, 0, mapData, 0, bParameters.Length);
            Buffer.BlockCopy(bIp, 0, mapData, 512, bIp.Length);
            Buffer.BlockCopy(bPort, 0, mapData, 532, bPort.Length);

            int errorCode;
            mapFile = CreateFileMapping(INVALID_HANDLE_VALUE, null, FileMapProtection.PageExecuteReadWrite, 0, MAP_SIZE, MAP_NAME);
            errorCode = Marshal.GetLastWin32Error();
            if(errorCode != 0)
            {
                App.Logger.Log($"Error: {errorCode}", "Ez2OnR14Game::MapNative:CreateFileMapping");
            }
            mapBuffer = MapViewOfFile(mapFile, FileMapAccess.FileMapAllAccess, 0, 0, MAP_SIZE);
            errorCode = Marshal.GetLastWin32Error();
            if (errorCode != 0)
            {
                App.Logger.Log($"Error: {errorCode}", "Ez2OnR14Game::MapNative:MapViewOfFile");
            }
            Marshal.Copy(mapData, 0, mapBuffer, mapData.Length);
        }

        private void UnMapNative()
        {
            App.Logger.Log("Trace", "Ez2OnR14Game::UnMapNative");
            if (mapBuffer != IntPtr.Zero)
            {
                UnmapViewOfFile(mapBuffer);
            }
            if (mapFile != IntPtr.Zero)
            {
                CloseHandle(mapFile);
            }
            mapBuffer = IntPtr.Zero;
            mapFile = IntPtr.Zero;
        }
    }
}