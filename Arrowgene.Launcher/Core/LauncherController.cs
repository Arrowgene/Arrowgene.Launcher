namespace Arrowgene.Launcher.Core
{
    using Arrowgene.Launcher.Api;
    using Arrowgene.Launcher.Download;
    using Arrowgene.Launcher.Game;
    using Arrowgene.Launcher.Translation;
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

        public LauncherController(ILauncherWindow launcherWindow, Configuration config)
        {
            _window = launcherWindow;
            _window.window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window.window.Loaded += Window_Loaded;
            _config = config;
        }

        private void Game_SelectedGame(object sender, SelectedGameEventArgs e)
        {
            App.Logger.Log("Trace", "LauncherController::Game_SelectedGame");
            ChangeGame(e.Game);
        }

        private void Exit(ExitCode exitCode)
        {
            App.Logger.Log("Trace", "LauncherController::Exit");
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
            App.Logger.Log("Trace", "LauncherController::ChangeGame");
            if (game == null)
            {
                App.DisplayError(Translate("could_not_select_game"), "LauncherController::ChangeGame");
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

        private string Translate(string key)
        {
            return Translator.Instance.Translate(key);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.Logger.Log("Trace", "LauncherController::Window_Loaded");
            _api = new ArrowgeneApi();
            _downloader = new Downloader();

            // Init Controls
            _window.labelWebsite.Content = _config.WebUrl;
            _window.buttonDownloadClient.IsEnabled = false;
            UpdateProgress(null);

            // Setup Languages
            foreach (Language language in Translator.Instance.Languages)
            {
                _window.stackPanelLanguages.Children.Add(language.SelectLanguageButton);
            }

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
                    App.DisplayError(Translate("no_games"), "LauncherController::Window_Loaded");
                    Exit(ExitCode.NO_GAMES);
                }
            }
            ChangeGame(_config.SelectedGame);
            UpdateRememberLogin();

            // Bind Events
            _window.buttonClose.Click += ButtonClose_Click;
            _window.buttonStart.Click += ButtonStart_Click;
            _window.buttonSetGameLocation.Click += ButtonSetGameLocation_Click;
            _window.buttonDownloadClient.Click += ButtonDownloadClient_Click;
            _window.buttonCheckUpdates.Click += ButtonCheckUpdates_Click;
            _window.labelWebsite.PreviewMouseDown += LabelWebsite_MouseUp;
            _window.window.MouseDown += Window_MouseDown;
            _window.checkBoxRememberLogin.Checked += CheckBoxRememberLogin_CheckedChanged;
            _window.checkBoxRememberLogin.Unchecked += CheckBoxRememberLogin_CheckedChanged;
            _window.passwordBoxPassword.GotFocus += PasswordBoxPassword_GotFocus;
            _window.passwordBoxPassword.LostFocus += PasswordBoxPassword_LostFocus;
            _window.textBoxAccount.LostFocus += TextBoxAccount_LostFocus;

            // Request Current Version
            _api.Version(UpdateVersion, true);
        }

        private void ButtonCheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            App.Logger.Log("Trace", "LauncherController::ButtonCheckUpdates_Click");
            _api.Version(UpdateVersion);
        }

        private void ButtonSetGameLocation_Click(object sender, RoutedEventArgs e)
        {
            App.Logger.Log("Trace", "LauncherController::ButtonSetGameLocation_Click");
            ChooseGameLocation();
        }

        private void ChooseGameLocation()
        {
            App.Logger.Log("Trace", "LauncherController::ChooseGameLocation");
            GameBase game = _config.SelectedGame;
            OpenFileDialog chooseFile = new OpenFileDialog();
            chooseFile.Filter = game.SelectExecutablePattern;
            chooseFile.Multiselect = false;
            chooseFile.Title = string.Format(Translate("select_game_executable"), game.Name);
            if (game.ExecutableExists)
            {
                chooseFile.InitialDirectory = game.Executable.DirectoryName;
            }
            if (chooseFile.ShowDialog() == true)
            {
                FileInfo executable = App.CreateFileInfo(chooseFile.FileName);
                int? version = GameBase.GetClientVersion(executable);
                if (version == null)
                {
                    App.DisplayMessage(Translate("unknown_client"), Translate("notice"));

                    return;
                }
                if (executable != null)
                {
                    game.Executable = executable;
                    _window.labelGamePath.Content = game.ExecutablePath;
                }
                else
                {
                    App.DisplayError(Translate("could_not_find_selected_file"), "LauncherController::ChooseGameLocation");
                }
            }
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            App.Logger.Log("Trace", "LauncherController::ButtonStart_Click");
            _config.SelectedGame.Start();
        }

        private void ButtonDownloadClient_Click(object sender, RoutedEventArgs e)
        {
            App.Logger.Log("Trace", "LauncherController::ButtonDownloadClient_Click");
            string url = _config.SelectedGame.GetDownloadUrl();
            if (!string.IsNullOrEmpty(url))
            {
                Process.Start(url);
            }
        }

        private void CheckBoxRememberLogin_CheckedChanged(object sender, RoutedEventArgs e)
        {
            App.Logger.Log("Trace", "LauncherController::CheckBoxRememberLogin_CheckedChanged");
            UpdateRememberLogin();
        }

        private void UpdateRememberLogin()
        {
            App.Logger.Log("Trace", "LauncherController::UpdateRememberLogin");
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
            App.Logger.Log("Trace", "LauncherController::TextBoxAccount_LostFocus");
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
            App.Logger.Log("Trace", "LauncherController::PasswordBoxPassword_LostFocus");
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
            App.Logger.Log("Trace", "LauncherController::PasswordBoxPassword_GotFocus");
            _window.passwordBoxPassword.Password = String.Empty;
        }

        private void LabelWebsite_MouseUp(object sender, MouseButtonEventArgs e)
        {
            App.Logger.Log("Trace", "LauncherController::LabelWebsite_MouseUp");
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
            App.Logger.Log("Trace", "LauncherController::UpdateProgress");
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

        private void UpdateVersion(ApiVersion version, object state)
        {
            App.Logger.Log("Trace", "LauncherController::UpdateVersion");
            foreach (GameBase game in _config.Games)
            {
                game.SetVersion(version);
            }
            GameBase selected = _config.SelectedGame;
            int? selectedVersion = selected.GetClientVersion();
            App.Dispatch(() =>
            {
                _window.labelLauncherVersion.Content = version.LauncherVersion;
                _window.buttonDownloadClient.IsEnabled = true;
                _window.labelLatestClientVersion.Content = selected.GetLatestClientVersion();
                _window.labelClientVersion.Content = selectedVersion == null ? "-" : selectedVersion.ToString();
            });
            if (App.VERSION < version.LauncherVersion)
            {
                App.Dispatch(() =>
                {
                    DialogBox dialogBox = new DialogBox(_window.window, Translate("new_launcher_download_now"), Translate("notice"), DialogBox.DialogButton.YES_NO);
                    if (dialogBox.ShowDialog() == true)
                    {
                        if (String.IsNullOrEmpty(version.LauncherUrl))
                        {
                            App.DisplayError(Translate("could_not_find_url"), "LauncherController::UpdateVersion:Launcher");
                            return;
                        }
                        Process.Start(version.LauncherUrl);
                    }
                });
            }
            if (state is bool)
            {
                if ((bool)state)
                {
                    return;
                }
            }
            if (selectedVersion == null)
            {
                App.DisplayMessage(Translate("unknown_client"), Translate("notice"));
            }
            else if (
              selected.GetLatestClientVersion() != null
              && selected.GetLatestClientVersion() > selectedVersion
              )
            {
                App.Dispatch(() =>
                {
                    DialogBox dialogBox = new DialogBox(_window.window, Translate("new_client_download_now"), Translate("notice"), DialogBox.DialogButton.YES_NO);
                    if (dialogBox.ShowDialog() == true)
                    {
                        if (String.IsNullOrEmpty(selected.GetDownloadUrl()))
                        {
                            App.DisplayError(Translate("could_not_find_url"), "LauncherController::UpdateVersion:Game");
                            return;
                        }
                        Process.Start(selected.GetDownloadUrl());
                    }
                });
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            App.Logger.Log("Trace", "LauncherController::ButtonClose_Click");
            Exit(ExitCode.CLOSE_BUTTON);
        }
    }
}