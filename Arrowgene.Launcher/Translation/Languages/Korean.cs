namespace Arrowgene.Launcher.Translation.Languages
{
    public class Korean : Language
    {
        public Korean()
        {
            _translations.Add("language_name", "한국어");
            _translations.Add("yes", "예");
            _translations.Add("no", "아니오");
            _translations.Add("start", "Start");
            _translations.Add("remember_login", "아이디 기억하기");
            _translations.Add("file_path", "경로:");
            _translations.Add("client_version", "클라이언트 버전:");
            _translations.Add("latest_client_version", "최신:");
            _translations.Add("set_game_location", "게임 위치 설정");
            _translations.Add("download_client", "클라이언트 다운로드");
            _translations.Add("check_for_updates", "업데이트 확인");
            _translations.Add("close", "닫기");
            _translations.Add("status", "상태");
            _translations.Add("title", "제목");
            _translations.Add("message", "메시지");
            _translations.Add("change_language", "언어 변경");
            _translations.Add("could_not_select_game", "게임을 선택할 수 없습니다.");
            _translations.Add("no_games", "사용 가능한 게임이 없습니다.");
            _translations.Add("could_not_find_selected_file", "선택된 파일을 찾을 수 없습니다.");
            _translations.Add("new_launcher_download_now", "실행기의 새로운 버전이 있습니다. 다운로드 하시겠습니까?");
            _translations.Add("launcher_update", "실행기 업데이트");
            _translations.Add("select_game_executable", "{0} 게임 폴더에 있는 exe를 선택하세요.");  // {0} = Ez2On | Dance!
            _translations.Add("please_restart", "적용하려면 응용 프로그램을 다시 시작하십시오.");
            _translations.Add("notice", "정보");
        }

        public override string FlagRessource => "pack://application:,,,/kr.png";

        public override LanguageType LanguageType => LanguageType.Korean;
    }
}
