using Arrowgene.Launcher.Translation;
using System.Net;

namespace Arrowgene.Launcher.Game.Ez2On
{
    public class Ez2OnR13Game : Ez2OnGame
    {
        public override GameType Type => GameType.Ez2OnR13;

        public override void Start()
        {
            App.Logger.Log("Trace", "Ez2OnR13Game::Start");
            this.Launch("NULL", base.Account, base.Hash);
        }

        private void Launch(string session, string account, string hash)
        {
            App.Logger.Log("Trace", "Ez2OnR13Game::Launch");
            if (!ExecutableExists)
            {
                App.DisplayError(string.Format(Translator.Instance.Translate("can_not_find_executable"), ExecutablePath), "Ez2OnR13Game::Launch");
                return;
            }
            IPAddress ip = GetIpAddress();
            if (ip == null)
            {
                App.DisplayError(string.Format(Translator.Instance.Translate("failed_to_resolve_ip"), Host), "Ez2OnR13Game::Launch");
                return;
            }
            App.Logger.Log(string.Format("Patching - IP: {0} Port: {1}", ip, Port), "Ez2OnR13Game::Launch");
            Ez2OnPatcher patcher = new Ez2OnPatcher(Executable);
            patcher.SavePatches(ip.ToString(), Port, true);
            SetWindowMode(WindowMode);
            App.Logger.Log(string.Format("Starting Session: {0} Account: {1}", session, account), "Ez2OnR13Game::Launch");
            string arguments = string.Format("{0}|{1}|{2}|{3}", session, account, hash, 9999);
            StartProcess(Executable.FullName, arguments, Executable.DirectoryName);
        }
    }
}
