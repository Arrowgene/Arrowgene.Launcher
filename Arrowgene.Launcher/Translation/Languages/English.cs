namespace Arrowgene.Launcher.Translation.Languages
{
    public class English : Language
    {
        public English()
        {
            _translations.Add("language_name", "English");
            _translations.Add("yes", "Yes");
            _translations.Add("no", "No");
            _translations.Add("start", "Start");
            _translations.Add("remember_login", "Remember Login");
            _translations.Add("file_path", "Path:");
            _translations.Add("client_version", "Client Version:");
            _translations.Add("latest_client_version", "Latest:");
            _translations.Add("set_game_location", "Set Game Location");
            _translations.Add("download_client", "Download Client");
            _translations.Add("check_for_updates", "Check For Updates");
            _translations.Add("close", "Close");
            _translations.Add("status", "Status");
            _translations.Add("title", "Title");
            _translations.Add("message", "Message");
            _translations.Add("change_language", "Change Language");
            _translations.Add("could_not_select_game", "Couldn't select game");
            _translations.Add("no_games", "No games available");
            _translations.Add("could_not_find_selected_file", "Couldn't find selected file");
            _translations.Add("new_launcher_download_now", "A new launcher version is available, download now?");
            _translations.Add("launcher_update", "Launcher Update");
            _translations.Add("select_game_executable", "Select {0}'s executable in game folder");  // {0} = Ez2On | Dance!
            _translations.Add("please_restart", "Please restart the application to take effect");
            _translations.Add("notice", "Notice");
            _translations.Add("could_not_find_url", "Launcher URL could not be found");
            _translations.Add("unknown_client", "Client version check failed, your client is not supported and can not be checked for updates");
            _translations.Add("new_client_download_now", "New client detected, download now?");
            _translations.Add("can_not_find_executable", "Can not find executable. Please set a game location ({0})"); // {0} = C://game/game.exe
            _translations.Add("failed_to_create_args", "Unable to create startup parameters");
            _translations.Add("failed_to_resolve_ip", "Unable to find the IP address of '{0}'"); // {0} = https://domain.tld
        }

        public override string FlagRessource => "pack://application:,,,/en.png";

        public override LanguageType LanguageType => LanguageType.English;
    }
}
