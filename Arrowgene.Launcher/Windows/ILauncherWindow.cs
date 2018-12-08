namespace Arrowgene.Launcher.Windows
{
    using System.Windows;
    using System.Windows.Controls;

    public interface ILauncherWindow
    {
        Window window { get; }
        Label labelGamePath { get; }
        Label labelClientVersion { get; }
        Label labelLatestClientVersion { get; }
        Label labelLauncherVersion { get; }
        Label labelStatus { get; }
        Label labelWebsite { get; }
        Button buttonSetGameLocation { get; }
        Button buttonDownloadClient { get; }
        Button buttonStart { get; }
        Button buttonClose { get; }
        Button buttonCheckUpdates { get; }
        ProgressBar progressBarDownload { get; }
        CheckBox checkBoxRememberLogin { get; }
        TextBox textBoxAccount { get; }
        PasswordBox passwordBoxPassword { get; }
        StackPanel stackPanelGames { get; }
    }
}
