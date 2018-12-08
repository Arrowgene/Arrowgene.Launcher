using System.Collections.Generic;
using System.IO;

namespace Arrowgene.Launcher.Game.Ez2On
{
    public class Ez2OnPatcher
    {
        private readonly List<GamePatch> _xTrap;
        private readonly FileInfo _file;

        private List<GamePatch> _xTrapIp1;
        private List<GamePatch> _xTrapIp2;
        private List<GamePatch> _xTrapIp3;
        private List<GamePatch> _loginIp;
        private List<GamePatch> _loginPort;
        private GamePatcher _patcher;

        public Ez2OnPatcher(FileInfo path)
        {
            _file = path;
            _xTrap = new List<GamePatch>();
            _xTrapIp1 = new List<GamePatch>();
            _xTrapIp2 = new List<GamePatch>();
            _xTrapIp3 = new List<GamePatch>();
            _loginIp = new List<GamePatch>();
            _loginPort = new List<GamePatch>();
        }

        public void SavePatches(string ip, ushort port, bool removeXTrap)
        {
            byte[] file = App.ReadFile(_file.FullName);
            if (file == null)
            {
                return;
            }
            _patcher = new GamePatcher(file);

            _xTrap.Add(new GamePatch(0x8628F, 0x52, 0x90));
            _xTrap.Add(new GamePatch(0x86290, 0xFF, 0x90));
            _xTrap.Add(new GamePatch(0x86291, 0x15, 0x90));
            _xTrap.Add(new GamePatch(0x86292, 0x74, 0x90));
            _xTrap.Add(new GamePatch(0x86293, 0xF2, 0x90));
            _xTrap.Add(new GamePatch(0x86294, 0x54, 0x90));
            _xTrap.Add(new GamePatch(0x86295, 0x00, 0x90));

            _xTrap.Add(new GamePatch(0x11B7C8, 0x55, 0xC3));

            _xTrap.Add(new GamePatch(0x11DAF0, 0x0F, 0xE9));
            _xTrap.Add(new GamePatch(0x11DAF1, 0x84, 0xD3));
            _xTrap.Add(new GamePatch(0x11DAF2, 0xD2, 0x01));
            _xTrap.Add(new GamePatch(0x11DAF3, 0x01, 0x00));
            _xTrap.Add(new GamePatch(0x11DAF5, 0x00, 0x90));

            _xTrap.Add(new GamePatch(0x11F9F5, 0x74, 0x90));
            _xTrap.Add(new GamePatch(0x11F9F6, 0x18, 0x90));

            _xTrap.Add(new GamePatch(0x11FE9C, 0x75, 0xEB));

            _xTrap.Add(new GamePatch(0x11FEF9, 0x90, 0xFF));
            _xTrap.Add(new GamePatch(0x11FEFA, 0x5F, 0xFF));
            _xTrap.Add(new GamePatch(0x11FEFB, 0x01, 0xFF));
            _xTrap.Add(new GamePatch(0x11FEFC, 0x00, 0xFF));

            _xTrap.Add(new GamePatch(0x120D60, 0x30, 0xFF));
            _xTrap.Add(new GamePatch(0x120D61, 0x75, 0xFF));
            _xTrap.Add(new GamePatch(0x120D62, 0x00, 0xFF));
            _xTrap.Add(new GamePatch(0x120D63, 0x00, 0xFF));
            
            _xTrapIp1 = GamePatcher.CreateIpPatch("127.0.0.1", 0x196714);
            _xTrapIp2 = GamePatcher.CreateIpPatch("127.0.0.1", 0x196750);
            _xTrapIp3 = GamePatcher.CreateIpPatch("127.0.0.1", 0x1967B4);

            SetIp(ip);
            SetPort(port);
            if (removeXTrap)
            {
                RemoveXtrap();
            }
            
            _patcher.Patch();
            byte[] patched = _patcher.GetFile();
            App.WriteFile(patched, _file.FullName);
        }

        private void RemoveXtrap()
        {
            if (!_patcher.IsPatched(_xTrap))
            {
                _patcher.AddPatch(_xTrap);
            }

            if (!_patcher.IsPatched(_xTrapIp1))
            {
                _patcher.AddPatch(_xTrapIp1);
            }

            if (!_patcher.IsPatched(_xTrapIp2))
            {
                _patcher.AddPatch(_xTrapIp2);
            }

            if (!_patcher.IsPatched(_xTrapIp3))
            {
                _patcher.AddPatch(_xTrapIp3);
            }
        }

        private void SetIp(string ip)
        {
            _loginIp = GamePatcher.CreateIpPatch(ip, 0x152B88);
            if (!_patcher.IsPatched(_loginIp))
            {
                _patcher.AddPatch(_loginIp);
            }
        }

        private void SetPort(ushort port)
        {
            _loginPort.AddRange(CreatePortPatch(port, 0x44FF5));
            _loginPort.AddRange(CreatePortPatch(port, 0x45036));
            if (!_patcher.IsPatched(_loginPort))
            {
                _patcher.AddPatch(_loginPort);
            }
        }

        private List<GamePatch> CreatePortPatch(ushort port, int offset)
        {
            byte lo = (byte)(port & 0xff);
            byte hi = (byte)((port >> 8) & 0xff);

            List<GamePatch> patches = new List<GamePatch>();
            patches.Add(new GamePatch(offset, lo));
            patches.Add(new GamePatch(offset + 1, hi));

            return patches;
        }
    }
}