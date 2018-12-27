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
            _translations.Add("could_not_find_url", "실행기 URL을 찾을 수 없습니다.");
            _translations.Add("unknown_client", "클라이언트 버전 확인에 실패했습니다. 클라이언트가 지원되지 않으며 업데이트를 확인할 수 없습니다.");
            _translations.Add("new_client_download_now", "새로운 클라이언트가 발견되었습니다. 지금 다운로드 하시겠습니까?");
            _translations.Add("can_not_find_executable", "실행 파일을 찾을 수 없습니다. 게임 위치 ({0})를 설정하십시오."); // {0} = C://game/game.exe
            _translations.Add("failed_to_create_args", "시작 인수를 만들지 못했습니다.");
            _translations.Add("failed_to_resolve_ip", "'{0}'의 IP 주소를 찾을 수 없습니다."); // {0} = https://domain.tld
            _translations.Add("download_from_mirror", "파일 서버에서 다운로드 하시겠습니까? (더 빠르게)");
            _translations.Add("after_download_instruction", "다운로드가 완료되면 아카이브를 추출하고 '게임 위치 설정'버튼을 사용하십시오");
            _translations.Add("window_mode", "창모드");
        }

        public override string FlagRessource => "pack://application:,,,/kr.png";

        public override LanguageType LanguageType => LanguageType.Korean;
    }
}
