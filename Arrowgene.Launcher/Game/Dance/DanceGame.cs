using Arrowgene.Launcher.Translation;
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
            App.Logger.Log("Trace", "DanceGame::Start");
            if (!ExecutableExists)
            {
                App.DisplayError(string.Format(Translator.Instance.Translate("can_not_find_executable"), ExecutablePath), "DanceGame::Start");
                return;
            }
            IPAddress ipAddress = GetIpAddress();
            if (ipAddress == null)
            {

                App.DisplayError(string.Format(Translator.Instance.Translate("failed_to_resolve_ip"), Host), "DanceGame::Start");
                return;
            }
            string arguments = CreateGameStartArguments(ipAddress);
            if (string.IsNullOrEmpty(arguments))
            {

                App.DisplayError(Translator.Instance.Translate("failed_to_create_args"), "DanceGame::Start");
                return;
            }
            StartProcess(Executable.FullName, arguments, Executable.DirectoryName);
        }

        private string CreateGameStartArguments(IPAddress ipAddress)
        {
            App.Logger.Log("Trace", "DanceGame::CreateGameStartArguments");
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
