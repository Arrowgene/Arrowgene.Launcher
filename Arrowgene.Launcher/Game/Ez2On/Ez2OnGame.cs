using System.Net;

namespace Arrowgene.Launcher.Game.Ez2On
{
    public class Ez2OnGame : GameBase
    {
        public override GameType Type => GameType.Ez2on;
        public override string SelectGameImage => "pack://application:,,,/ez2on.png";
        public override string SelectExecutablePattern => "*.exe|*.exe";

        public override void SetDefaultValues()
        {
            Host = "server.arrowgene.net";
            Port = 9350;
            Account = "";
            Hash = "";
            RememberLogin = false;
        }

        public override void Start()
        {
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
            if (ExecutableExists)
            {
                Ez2OnPatcher patcher = new Ez2OnPatcher(Executable);
                IPAddress ip = GetIpAddress();
                App.Logger.Log(string.Format("Patching - IP: {0} Port: {1}", ip, Port), "Ez2OnGame::Launch");
                patcher.SavePatches(ip.ToString(), Port, true);
                string arguments = string.Format("{0}|{1}|{2}|{3}", session, account, hash, 9999);
                StartProcess(Executable.FullName, arguments, Executable.DirectoryName);
            }
            else
            {
                App.DisplayError(string.Format("Can not find executable. ({0})", ExecutablePath), "Ez2OnGame::Launch");
            }
        }
    }
}
