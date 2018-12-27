using System.Windows;
using System.Windows.Controls;


namespace Arrowgene.Launcher.Windows
{
    public partial class LauncherWindow : Window, ILauncherWindow
    {
        public LauncherWindow()
        {
            InitializeComponent(); 
        }

        Window ILauncherWindow.window => this;
        Label ILauncherWindow.labelGamePath => this.labelGamePath;
        Label ILauncherWindow.labelClientVersion => this.labelClientVersion;
        Label ILauncherWindow.labelLatestClientVersion => this.labelLatestClientVersion;
        Label ILauncherWindow.labelStatus => this.labelStatus;
        Label ILauncherWindow.labelWebsite => this.labelWebsite;
        Label ILauncherWindow.labelLauncherVersion => this.labelLauncherVersion;
        Button ILauncherWindow.buttonSetGameLocation => this.buttonSetGameLocation;
        Button ILauncherWindow.buttonDownloadClient => this.buttonDownloadClient;
        Button ILauncherWindow.buttonStart => this.buttonStart;
        Button ILauncherWindow.buttonClose => this.buttonClose;
        Button ILauncherWindow.buttonCheckUpdates => this.buttonCheckUpdates;
        CheckBox ILauncherWindow.checkBoxRememberLogin => this.checkBoxRememberLogin;
        CheckBox ILauncherWindow.checkBoxWindowMode => this.checkBoxWindowMode;
        TextBox ILauncherWindow.textBoxAccount => this.textBoxAccount;
        PasswordBox ILauncherWindow.passwordBoxPassword => this.passwordBoxPassword;
        ProgressBar ILauncherWindow.progressBarDownload => this.progressBarDownload;
        StackPanel ILauncherWindow.stackPanelGames => this.stackPanelGames;
        StackPanel ILauncherWindow.stackPanelLanguages => this.stackPanelLanguages;
    }
}
