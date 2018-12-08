namespace Arrowgene.Launcher.Core
{
    using Arrowgene.Launcher.Api;
    using Arrowgene.Launcher.Download;
    using Arrowgene.Launcher.Game;
    using Arrowgene.Launcher.Windows;
    using Microsoft.Win32;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    public class LauncherController
    {
        private ILauncherWindow _window;
        private Configuration _config;
        private ArrowgeneApi _api;
        private Downloader _downloader;

        public LauncherController(ILauncherWindow launcherWindow)
        {
            _window = launcherWindow;
            _window.window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window.window.Loaded += Window_Loaded;
        }

        private void Game_SelectedGame(object sender, SelectedGameEventArgs e)
        {
            ChangeGame(e.Game);
        }

        private void Exit(ExitCode exitCode)
        {
            _downloader.Stop();
            _config.Save();
            if (exitCode != ExitCode.CLOSE_BUTTON)
            {
                App.Logger.Log("ExitCode: " + exitCode.ToString(), "LauncherController::Exit");
            }
            Environment.Exit((int)exitCode);
        }

        private void ChangeGame(GameBase game)
        {
            if (game == null)
            {
                App.DisplayError("Couldn't select game", "LauncherController::ChangeGame");
                return;
            }
            GameBase oldGame = _config.SelectedGame;
            _config.SelectedGame = game;

            game.SelectGameButton.IsEnabled = false;
            _window.labelLatestClientVersion.Content = game.GetClientVersion();
            _window.labelGamePath.Content = game.ExecutablePath;
            _window.labelGamePath.ToolTip = game.ExecutablePath;
            _window.checkBoxRememberLogin.IsChecked = game.RememberLogin;
            _window.passwordBoxPassword.Password = game.Hash;
            _window.textBoxAccount.Text = game.Account;
            _window.labelLatestClientVersion.Content = game.GetLatestClientVersion();
            if (game != oldGame)
            {
                oldGame.SelectGameButton.IsEnabled = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string configPath = App.GetConfigPath();
            FileInfo configFile = App.CreateFileInfo(configPath);
            if (configFile == null)
            {
                App.DisplayError(string.Format("The path: {0} is invalid.", configPath), "LauncherController::Window_Loaded");
                Exit(ExitCode.CONFIG_PATH);
            }
            _config = new Configuration(configPath);
            _api = new ArrowgeneApi();
            _downloader = new Downloader();
            if (_config.Load())
            {
                // Init Controls
                _window.labelWebsite.Content = _config.WebUrl;
                _window.buttonDownloadClient.IsEnabled = false;
                UpdateProgress(null);

                // Setup Games
                foreach (GameBase game in _config.Games)
                {
                    game.SelectedGame += Game_SelectedGame;
                    _window.stackPanelGames.Children.Add(game.SelectGameButton);
                }
                if (_config.SelectedGame == null)
                {
                    if (_config.Games.Count > 0)
                    {
                        _config.SelectedGame = _config.Games[0];
                    }
                    else
                    {
                        App.DisplayError("No games available", "LauncherController::Window_Loaded");
                        Exit(ExitCode.NO_GAMES);
                    }
                }
                ChangeGame(_config.SelectedGame);
                UpdateRememberLogin();

                // Bind Events
                _window.buttonClose.Click += buttonClose_Click;
                _window.buttonStart.Click += buttonStart_Click;
                _window.buttonSetGameLocation.Click += buttonSetGameLocation_Click;
                _window.buttonDownloadClient.Click += buttonDownloadClient_Click;
                _window.buttonCheckUpdates.Click += ButtonCheckUpdates_Click;
                _window.labelWebsite.PreviewMouseDown += LabelWebsite_MouseUp;
                _window.window.MouseDown += Window_MouseDown;
                _window.checkBoxRememberLogin.Checked += CheckBoxRememberLogin_CheckedChanged;
                _window.checkBoxRememberLogin.Unchecked += CheckBoxRememberLogin_CheckedChanged;
                _window.passwordBoxPassword.GotFocus += PasswordBoxPassword_GotFocus;
                _window.passwordBoxPassword.LostFocus += PasswordBoxPassword_LostFocus;
                _window.textBoxAccount.LostFocus += TextBoxAccount_LostFocus;

                // Request Current Version
                _api.Version(UpdateVersion);
            }
            else
            {
                Exit(ExitCode.LOAD_SETTINGS);
            }
            if (!_config.SelectedGame.ExecutableExists)
            {
                ChooseGameLocation();
            }
        }

        private void ButtonCheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            _api.Version(UpdateVersion);
        }

        private void buttonSetGameLocation_Click(object sender, RoutedEventArgs e)
        {
            ChooseGameLocation();
        }

        private void ChooseGameLocation()
        {
            GameBase game = _config.SelectedGame;
            OpenFileDialog chooseFile = new OpenFileDialog();
            chooseFile.Filter = game.SelectExecutablePattern;
            chooseFile.Multiselect = false;
            chooseFile.Title = string.Format("Select {0}'s executable in game folder", game.Name);
            if (game.ExecutableExists)
            {
                chooseFile.InitialDirectory = game.Executable.DirectoryName;
            }
            if (chooseFile.ShowDialog() == true)
            {
                FileInfo executable = App.CreateFileInfo(chooseFile.FileName);
                if (executable != null)
                {
                    game.Executable = executable;
                    _window.labelGamePath.Content = game.ExecutablePath;
                }
                else
                {
                    App.DisplayError("Could not find selected file", "LauncherController::ChooseGameLocation");
                }
            }
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            _config.SelectedGame.Start();
        }

        private void buttonDownloadClient_Click(object sender, RoutedEventArgs e)
        {
            string url = _config.SelectedGame.GetDownloadUrl();
            if (!string.IsNullOrEmpty(url))
            {
                Process.Start(url);
            }
        }

        private void CheckBoxRememberLogin_CheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdateRememberLogin();
        }

        private void UpdateRememberLogin()
        {
            if (_window.checkBoxRememberLogin.IsChecked == true)
            {
                _window.textBoxAccount.IsEnabled = true;
                _window.passwordBoxPassword.IsEnabled = true;
                _config.SelectedGame.RememberLogin = true;
            }
            else
            {
                _window.passwordBoxPassword.Password = String.Empty;
                _window.textBoxAccount.Text = String.Empty;
                _window.textBoxAccount.IsEnabled = false;
                _window.passwordBoxPassword.IsEnabled = false;
                _config.SelectedGame.RememberLogin = false;
                _config.SelectedGame.Account = String.Empty;
                _config.SelectedGame.Hash = String.Empty;
            }
        }

        private void TextBoxAccount_LostFocus(object sender, RoutedEventArgs e)
        {
            String account = _window.textBoxAccount.Text;
            if (string.IsNullOrEmpty(account))
            {
                _config.SelectedGame.Account = string.Empty;
            }
            else
            {
                _config.SelectedGame.Account = account;
            }
        }

        private void PasswordBoxPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            String password = _window.passwordBoxPassword.Password;
            if (string.IsNullOrEmpty(password))
            {
                _config.SelectedGame.Hash = string.Empty;
            }
            else
            {
                String hash = App.CreateMD5(password);
                _window.passwordBoxPassword.Password = hash;
                _config.SelectedGame.Hash = hash;
            }
        }

        private void PasswordBoxPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            _window.passwordBoxPassword.Password = String.Empty;
        }

        private void LabelWebsite_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(_config.WebUrl);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
            {
                _window.window.DragMove();
            }
        }

        private void UpdateProgress(string status, bool progress = false, int value = 0, int min = 0, int max = 100)
        {
            if (!string.IsNullOrEmpty(status))
            {
                _window.labelStatus.Content = status;
                _window.labelStatus.Visibility = Visibility.Visible;
            }
            else
            {
                _window.labelStatus.Visibility = Visibility.Hidden;
                _window.labelStatus.Content = string.Empty;
            }
            if (progress)
            {
                _window.progressBarDownload.Value = value;
                _window.progressBarDownload.Maximum = max;
                _window.progressBarDownload.Minimum = min;
                _window.progressBarDownload.Visibility = Visibility.Visible;
            }
            else
            {
                _window.progressBarDownload.Visibility = Visibility.Hidden;
            }
        }

        private void UpdateVersion(ApiVersion version)
        {
            foreach (GameBase game in _config.Games)
            {
                game.SetVersion(version);
            }
            App.Dispatch(() =>
            {
                _window.labelLauncherVersion.Content = version.LauncherVersion;
                _window.buttonDownloadClient.IsEnabled = true;
                _window.labelLatestClientVersion.Content = _config.SelectedGame.GetLatestClientVersion();
            });
            if(App.VERSION < version.LauncherVersion)
            {
                DialogBox dialogBox = new DialogBox(_window.window, "A new launcher version is available, download now?", "Launcher Update");
                if (dialogBox.ShowDialog() == true)
                {
                    Process.Start(version.LauncherUrl);
                }
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Exit(ExitCode.CLOSE_BUTTON);
        }
    }
}