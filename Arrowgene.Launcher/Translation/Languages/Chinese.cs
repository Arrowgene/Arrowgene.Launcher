namespace Arrowgene.Launcher.Translation.Languages
{
    public class Chinese : Language
    {
        public Chinese()
        {
            _translations.Add("language_name", "中文");
            _translations.Add("yes", "是");
            _translations.Add("no", "否");
            _translations.Add("start", "开始");
            _translations.Add("remember_login", "记住密码");
            _translations.Add("file_path", "路径：");
            _translations.Add("client_version", "客户端版本：");
            _translations.Add("latest_client_version", "最新版本：");
            _translations.Add("set_game_location", "设置游戏路径");
            _translations.Add("download_client", "下载客户端");
            _translations.Add("check_for_updates", "检测更新");
            _translations.Add("close", "关闭");
            _translations.Add("status", "状态");
            _translations.Add("title", "标题");
            _translations.Add("message", "信息");
            _translations.Add("change_language", "切换语言");
            _translations.Add("could_not_select_game", "不能选择游戏");
            _translations.Add("no_games", "没有游戏可用");
            _translations.Add("could_not_find_selected_file", "不能找到选择的文件");
            _translations.Add("new_launcher_download_now", "登录器有新版本，要下载吗？");
            _translations.Add("launcher_update", "登录器更新");
            _translations.Add("select_game_executable", "在游戏文件夹中选择{0}的可执行程序");  // {0} = Ez2On | Dance!
            _translations.Add("please_restart", "请重新启动应用程序才能生效");
            _translations.Add("notice", "注意");
            _translations.Add("could_not_find_url", "找不到启动器URL");
            _translations.Add("unknown_client", "客户端版本检查失败，您的客户端不受支持，无法检查更新");
            _translations.Add("new_client_download_now", "检测到新客户端，立即下载？");
            _translations.Add("can_not_find_executable", "找不到可执行文件。 请设置游戏位置（{0}"); // {0} = C://game/game.exe
            _translations.Add("failed_to_create_args", "无法创建启动参数");
            _translations.Add("failed_to_resolve_ip", "找不到“{0}”的IP地址"); // {0} = https://domain.tld
            _translations.Add("download_from_mirror", "从文件服务器下载？ (快点)");
            _translations.Add("after_download_instruction", "下载完成后：提取存档并使用“设置游戏路径”按钮");
            _translations.Add("window_mode", "窗口模式");
        }

        public override string FlagRessource => "pack://application:,,,/cn.png";

        public override LanguageType LanguageType => LanguageType.Chinese;
    }
}
