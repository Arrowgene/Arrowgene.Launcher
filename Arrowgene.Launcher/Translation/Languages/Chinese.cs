namespace Arrowgene.Launcher.Translation.Languages
{
    public class Chinese : Language
    {
        public Chinese()
        {
            _translations.Add("language_name", "中文");
            _translations.Add("yes", "是");
            _translations.Add("no", "没有");
            _translations.Add("start", "开始");
            _translations.Add("remember_login", "记住登录");
            _translations.Add("file_path", "路径:");
            _translations.Add("client_version", "当前版本:");
            _translations.Add("latest_client_version", "最新版本:");
            _translations.Add("set_game_location", "设置游戏位置");
            _translations.Add("download_client", "下载游戏");
            _translations.Add("check_for_updates", "检查更新");
            _translations.Add("close", "关掉");
            _translations.Add("status", "状态");
            _translations.Add("title", "标题");
            _translations.Add("message", "信息");
            _translations.Add("change_language", "改变语言");
            _translations.Add("could_not_select_game", "无法选择游戏");
            _translations.Add("no_games", "没有游戏可用");
            _translations.Add("could_not_find_selected_file", "找不到所选文件");
            _translations.Add("new_launcher_download_now", "新的发射器版本, 现在下载？");
            _translations.Add("launcher_update", "启动器更新");
            _translations.Add("select_game_executable", "在游戏文件夹中选择 {0} 的可执行文件");  // {0} = Ez2On | Dance!
            _translations.Add("please_restart", "请重新启动应用程序才能生效");
            _translations.Add("notice", "注意");
            _translations.Add("could_not_find_url", "找不到启动器URL");
        }

        public override string FlagRessource => "pack://application:,,,/cn.png";

        public override LanguageType LanguageType => LanguageType.Chinese;
    }
}
