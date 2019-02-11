namespace Arrowgene.Launcher.Core
{
    using Arrowgene.Launcher.Game;
    using Arrowgene.Launcher.Game.Dance;
    using Arrowgene.Launcher.Game.Ez2On;
    using Arrowgene.Launcher.Translation;
    using Arrowgene.Launcher.Windows;
    using Nini.Config;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    public class Configuration
    {
        private const string DEFAULT_HOST = "server.arrowgene.net";

        private const string SECTION_LAUNCHER = "Launcher";
        private const string LAUNCHER_GAME = "Game";
        private const string LAUNCHER_LANGUAGE = "Language";

        private const string GAME_EXE = "Exe";
        private const string GAME_REMEMBER_LOGIN = "SaveLogin";
        private const string GAME_ACCOUNT = "Account";
        private const string GAME_HASH = "Hash";
        private const string GAME_HOST = "Host";
        private const string GAME_PORT = "Port";
        private const string GAME_WINDOW_MODE = "Window";

        private IniConfigSource _iniFile;

        public Configuration(string configurationPath)
        {
            ConfigPath = configurationPath;
            WebUrl = "https://www.arrowgene.net";
            Games = new List<GameBase>();
        }

        public string ConfigPath { get; }
        public string WebUrl { get; }
        public List<GameBase> Games { get; }
        public GameBase SelectedGame { get; set; }
        public LanguageType SelectedLanguage { get; set; }

        public bool Load()
        {
            try
            {
                FileInfo configPath = App.CreateFileInfo(ConfigPath);
                if (configPath.Exists)
                {
                    _iniFile = new IniConfigSource(configPath.FullName);
                }
                else
                {
                    // Default Settings
                    _iniFile = new IniConfigSource();
                    _iniFile.Save(configPath.FullName);
                    SelectedLanguage = LanguageType.English;
                }
                foreach (GameType gameType in Enum.GetValues(typeof(GameType)))
                {
                    string section = GetString(gameType);
                    GameBase game;
                    switch (gameType)
                    {
                        case GameType.Dance:
                            game = new DanceGame();
                            break;
                        case GameType.Ez2OnR13:
                            game = new Ez2OnR13Game();
                            break;
                        case GameType.Ez2OnR14:
                            game = new Ez2OnR14Game();
                            break;
                        default: continue;
                    }
                    IConfig gameSection = _iniFile.Configs[section];
                    if (gameSection != null)
                    {
                        game.Executable = App.CreateFileInfo(gameSection.Get(GAME_EXE));
                        game.RememberLogin = gameSection.GetBoolean(GAME_REMEMBER_LOGIN);
                        game.WindowMode = gameSection.GetBoolean(GAME_WINDOW_MODE);
                        game.Account = gameSection.Get(GAME_ACCOUNT);
                        game.Hash = gameSection.Get(GAME_HASH);
                        game.Host = gameSection.Get(GAME_HOST);
                        game.Port = (ushort)gameSection.GetInt(GAME_PORT);
                    }
                    else
                    {
                        game.SetDefaultValues();
                    }
                    Games.Add(game);
                }
                IConfig launcherSection = _iniFile.Configs[SECTION_LAUNCHER];
                if (launcherSection != null)
                {
                    GameType selectedGameType = GetGameType(launcherSection.Get(LAUNCHER_GAME));
                    SelectedGame = GetGame(selectedGameType);
                    SelectedLanguage = GetLanguageType(launcherSection.Get(LAUNCHER_LANGUAGE));
                }
            }
            catch (Exception ex)
            {
                App.Logger.Log(ex, "LauncherConfig::Load");
                if (File.Exists(ConfigPath))
                {
                    DialogBox dBox = new DialogBox(App.Window, "Configuration file was corrupted, delete and try again?", "Configuration File Error");
                    bool? result = dBox.ShowDialog();
                    if (result == true)
                    {
                        Delete();
                        Thread.Sleep(100);
                        return Load();
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool Save()
        {
            try
            {
                IConfig launcherSection = _iniFile.Configs[SECTION_LAUNCHER];
                if (launcherSection == null)
                {
                    launcherSection = _iniFile.AddConfig(SECTION_LAUNCHER);
                }
                launcherSection.Set(LAUNCHER_LANGUAGE, GetString(SelectedLanguage));
                if (SelectedGame != null)
                {
                    launcherSection.Set(LAUNCHER_GAME, GetString(SelectedGame.Type));
                }
                foreach (GameBase game in Games)
                {
                    string section = GetString(game.Type);
                    IConfig gameSection = _iniFile.Configs[section];
                    if (gameSection == null)
                    {
                        gameSection = _iniFile.AddConfig(section);
                    }
                    gameSection.Set(GAME_EXE, game.ExecutablePath);
                    gameSection.Set(GAME_REMEMBER_LOGIN, game.RememberLogin);
                    gameSection.Set(GAME_WINDOW_MODE, game.WindowMode);
                    gameSection.Set(GAME_ACCOUNT, game.Account);
                    gameSection.Set(GAME_HASH, game.Hash);
                    gameSection.Set(GAME_HOST, game.Host);
                    gameSection.Set(GAME_PORT, game.Port);
                }
                _iniFile.Save();
            }
            catch (Exception ex)
            {
                App.Logger.Log(ex, "LauncherConfig::Sve");
                if (File.Exists(ConfigPath))
                {
                    DialogBox dBox = new DialogBox(App.Window, "Configuration file was corrupted, delete and try again?", "Configuration File Error");
                    bool? result = dBox.ShowDialog();
                    if (result == true)
                    {
                        Delete();
                        Thread.Sleep(100);
                        return Save();
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Delete()
        {
            File.Delete(ConfigPath);
        }

        private GameBase GetGame(GameType gameType)
        {
            foreach (GameBase game in Games)
            {
                if (game.Type == gameType)
                {
                    return game;
                }
            }
            return null;
        }

        private GameType GetGameType(string value)
        {
            return (GameType)Enum.Parse(typeof(GameType), value);
        }

        private LanguageType GetLanguageType(string value)
        {
            return (LanguageType)Enum.Parse(typeof(LanguageType), value);
        }

        private string GetString(GameType value)
        {
            return value.ToString();
        }

        private string GetString(LanguageType value)
        {
            return value.ToString();
        }
    }
}