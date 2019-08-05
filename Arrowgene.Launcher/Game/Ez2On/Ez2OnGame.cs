using System.IO;

namespace Arrowgene.Launcher.Game.Ez2On
{
    public abstract class Ez2OnGame : GameBase
    {
        public const bool DEFAULT_WINDOW_MODE = false;

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
            if (!SupportWindowMode)
            {
                App.Logger.Log("Not Supported", "Ez2OnGame::SetWindowMode");
                return;
            }
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

    }
}
