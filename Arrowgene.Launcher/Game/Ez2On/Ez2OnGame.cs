using Arrowgene.Launcher.Translation;
using System.IO;
using System.Net;

namespace Arrowgene.Launcher.Game.Ez2On
{
    public class Ez2OnGame : GameBase
    {
        public const bool DEFAULT_WINDOW_MODE = true;

        public override GameType Type => GameType.Ez2on;
        public override string SelectGameImage => "pack://application:,,,/ez2on.png";
        public override string SelectExecutablePattern => "*.exe|*.exe";
        public override bool SupportWindowMode => true;

        public override void SetDefaultValues()
        {
            Host = "server.arrowgene.net";
            Port = 9350;
            Account = "";
            Hash = "";
            RememberLogin = false;
            WindowMode = DEFAULT_WINDOW_MODE;
        }

        public void SetWindowMode(bool window)
        {
            App.Logger.Log("Trace", "Ez2OnGame::SetWindowMode");
            if (!ExecutableExists)
            {
                return;
            }
            string path = Path.Combine(Executable.DirectoryName, "EZ2ON_Online.ini");
            string content;
            if (window)
            {
                content = "\"FullScreen\" = 0";
            }
            else
            {
                content = "\"FullScreen\" = 1";
            }
            File.WriteAllText(path, content);
        }

        public bool IsWindowMode()
        {
            if (!ExecutableExists)
            {
                return DEFAULT_WINDOW_MODE;
            }
            string path = Path.Combine(Executable.DirectoryName, "EZ2ON_Online.ini");
            FileInfo fi = App.CreateFileInfo(path);
            if (!fi.Exists)
            {
                return DEFAULT_WINDOW_MODE;
            }
            string content = File.ReadAllText(fi.FullName);
            string key = "FullScreen";
            int start = content.IndexOf(key);
            if (start < 0)
            {
                return DEFAULT_WINDOW_MODE;
            }
            start = start + key.Length;
            int end = content.IndexOf("\n", start);
            if (end < 0)
            {
                return DEFAULT_WINDOW_MODE;
            }
            string value = content.Substring(start, end);
            int valStart = value.IndexOf("=");
            value = value.Substring(valStart, value.Length);
            value = value.Trim();
            int.TryParse(value, out int val);
            return val == 1;
        }

        public override void Start()
        {
            App.Logger.Log("Trace", "Ez2OnGame::Start");
            this.Launch("NULL", base.Account, base.Hash);
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="account"></param>
        /// <param name="hash"></param>
        private void Launch(string session, string account, string hash)
        {
            App.Logger.Log("Trace", "Ez2OnGame::Launch");
            if (!ExecutableExists)
            {
                App.DisplayError(string.Format(Translator.Instance.Translate("can_not_find_executable"), ExecutablePath), "Ez2OnGame::Launch");
                return;
            }
            IPAddress ip = GetIpAddress();
            if (ip == null)
            {
                App.DisplayError(string.Format(Translator.Instance.Translate("failed_to_resolve_ip"), Host), "Ez2OnGame::Launch");
                return;
            }
            App.Logger.Log(string.Format("Patching - IP: {0} Port: {1}", ip, Port), "Ez2OnGame::Launch");
            Ez2OnPatcher patcher = new Ez2OnPatcher(Executable);
            patcher.SavePatches(ip.ToString(), Port, true);
            SetWindowMode(WindowMode);
            string arguments = string.Format("{0}|{1}|{2}|{3}", session, account, hash, 9999);
            StartProcess(Executable.FullName, arguments, Executable.DirectoryName);
        }
    }
}
