using Arrowgene.Launcher.Api;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Arrowgene.Launcher.Game
{
    public abstract class GameBase
    {
        public static int? GetClientVersion(FileInfo executable)
        {
            if (executable != null && executable.Exists)
            {
                string versionPath = Path.Combine(executable.DirectoryName, VERSION_FILE_NAME);
                FileInfo version = App.CreateFileInfo(versionPath);
                if (version != null && version.Exists)
                {
                    string file = null;
                    using (StreamReader sr = new StreamReader(version.FullName))
                    {
                        file = sr.ReadToEnd();
                    }
                    int.TryParse(file, out int v);
                    return v;
                }
            }
            return null;
        }

        private const string VERSION_FILE_NAME = "ag.version";

        private ApiVersion _version;

        #region INI_SETTINGS

        public FileInfo Executable { get; set; }
        public String Host { get; set; }
        public ushort Port { get; set; }
        public string Account { get; set; }
        public string Hash { get; set; }
        public bool RememberLogin { get; set; }

        #endregion INI_SETTINGS

        public GameBase()
        {
            SelectGameButton = new Button();
            SelectGameButton.Style = App.Window.Resources["CustomGameButtonStyle"] as Style;
            SelectGameButton.Background = new ImageBrush(new BitmapImage(new Uri(SelectGameImage)));
            SelectGameButton.Click += SelectGameButton_Click;
        }

        public event EventHandler<SelectedGameEventArgs> SelectedGame;

        public abstract GameType Type { get; }
        public abstract string SelectGameImage { get; }
        public abstract string SelectExecutablePattern { get; }
        public virtual string Name => Type.ToString();
        public Button SelectGameButton { get; }
        public bool ExecutableExists => Executable != null && Executable.Exists;
        public string ExecutablePath => Executable == null ? "" : Executable.FullName;

        public abstract void Start();

        public abstract void SetDefaultValues();

        public virtual IPAddress GetIpAddress()
        {
            return App.IPAddressLookup(Host, AddressFamily.InterNetwork);
        }

        public int? GetLatestClientVersion()
        {
            if (_version != null)
            {
                if (Type == GameType.Dance)
                {
                    return _version.DanceVersion;
                }
                if (Type == GameType.Ez2on)
                {
                    return _version.Ez2OnVersion;
                }
            }
            return null;
        }

        public string GetDownloadUrl()
        {
            if (_version != null)
            {
                if (Type == GameType.Dance)
                {
                    return _version.DanceUrl;
                }
                if (Type == GameType.Ez2on)
                {
                    return _version.Ez2OnUrl;
                }
            }
            return string.Empty;
        }
        public string GetDownloadUrlMirror()
        {
            if (_version != null)
            {
                if (Type == GameType.Dance)
                {
                    return _version.DanceUrlMirror;
                }
                if (Type == GameType.Ez2on)
                {
                    return _version.Ez2OnUrlMirror;
                }
            }
            return string.Empty;
        }

        public int? GetClientVersion()
        {
            return GetClientVersion(Executable);
        }

        public void SetVersion(ApiVersion version)
        {
            _version = version;
        }

        protected void StartProcess(string executablePath, string arguments, string workDir)
        {
            App.Logger.Log(string.Format("Executable: {0} Arguments: {1} WorkDir: {2}", executablePath, arguments, workDir), "GameBase::StartProcess");
            ProcessStartInfo game = new ProcessStartInfo();
            game.FileName = executablePath;
            game.WorkingDirectory = workDir;
            game.Arguments = arguments;
            game.UseShellExecute = false;
            Process process = Process.Start(game);
        }

        private void SelectGameButton_Click(object sender, RoutedEventArgs e)
        {
            OnSelectedGame();
        }

        private void OnSelectedGame()
        {
            EventHandler<SelectedGameEventArgs> selectedGame = this.SelectedGame;
            if (selectedGame != null)
            {
                SelectedGameEventArgs selectedGameEventArgs = new SelectedGameEventArgs(this);
                selectedGame(this, selectedGameEventArgs);
            }
        }
    }
}
