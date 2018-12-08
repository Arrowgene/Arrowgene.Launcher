using System;
using System.Net;
using System.Text;

namespace Arrowgene.Launcher.Game.Dance
{
    public class DanceGame : GameBase
    {
        public DanceGame()
        {

        }

        public override GameType Type => GameType.Dance;
        public override string SelectGameImage => "pack://application:,,,/dance.png";
        public override string SelectExecutablePattern => "client.bin|client.bin";

        public override void SetDefaultValues()
        {
            Host = "server.arrowgene.net";
            Port = 2345;
            Account = "";
            Hash = "";
            RememberLogin = false;
        }

        public override void Start()
        {
            if (ExecutableExists)
            {
                IPAddress ipAddress = GetIpAddress();
                if (ipAddress != null)
                {
                    string arguments = CreateGameStartArguments(ipAddress);
                    if (!string.IsNullOrEmpty(arguments))
                    {
                        StartProcess(Executable.FullName, arguments, Executable.DirectoryName);
                    }
                    else
                    {
                        App.DisplayError("Failed to create startup arguments", "LauncherController::buttonStart_Click");
                    }
                }
                else
                {
                    App.DisplayError(String.Format("Can not find IPAdress of '{0}'", Host), "LauncherController::buttonStart_Click");
                }
            }
            else
            {
                App.DisplayError(String.Format("Can not find file '{0}'", ExecutablePath), "LauncherController::buttonStart_Click");
            }
        }

        private string CreateGameStartArguments(IPAddress ipAddress)
        {
            string arguments = null;
            if (ipAddress != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(ipAddress.ToString());
                sb.Append(" ");
                sb.Append(Port);
                sb.Append(" 1 0");
                if (RememberLogin)
                {
                    sb.Append(" ");
                    sb.Append(Account);
                    sb.Append(" ");
                    sb.Append(Hash);
                }
                arguments = sb.ToString();
            }
            return arguments;
        }

    }
}
