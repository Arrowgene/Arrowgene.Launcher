using Arrowgene.Launcher.Game;
using Arrowgene.Launcher.Translation;
using Arrowgene.Launcher.Windows;
using System.Diagnostics;

namespace Arrowgene.Launcher.Download
{
    public class Downloader
    {
        public Downloader()
        {

        }

        public void Download(GameBase game)
        {
            string url;
            DialogBox dialogBox = new DialogBox(App.Window, Translator.Instance.Translate("download_from_mirror"), Translator.Instance.Translate("notice"), DialogBox.DialogButton.YES_NO);
            if (dialogBox.ShowDialog() == true)
            {
                url = game.GetDownloadUrlMirror();
                if (!string.IsNullOrEmpty(url))
                {
                    Process.Start(url);
                    App.DisplayMessage(Translator.Instance.Translate("after_download_instruction"), Translator.Instance.Translate("notice"));
                    return;
                }
            }
            url = game.GetDownloadUrl();
            if (!string.IsNullOrEmpty(url))
            {
                Process.Start(url);
                App.DisplayMessage(Translator.Instance.Translate("after_download_instruction"), Translator.Instance.Translate("notice"));
                return;
            }
            App.DisplayError(Translator.Instance.Translate("could_not_find_url"), "Downloader::Download");
        }

        public void Stop()
        {

        }

    }
}
