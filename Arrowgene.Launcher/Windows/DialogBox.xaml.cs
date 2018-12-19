namespace Arrowgene.Launcher.Windows
{
    using System.Windows;
    using System.Windows.Input;

    public partial class DialogBox : Window
    {

        public enum DialogButton
        {
            YES,
            YES_NO
        }


        private string message;
        private string title;
        private DialogButton dialogButton;

        public DialogBox(Window owner, string message) : this(owner, message, "", DialogButton.YES)
        {

        }

        public DialogBox(Window owner, string message, string title) : this(owner, message, title, DialogButton.YES)
        {

        }

        public DialogBox(Window owner, string message, string title, DialogButton dialogButton)
        {
            if (owner.IsLoaded)
            {
                this.Owner = owner;
            }
            this.message = message;
            this.title = title;
            this.dialogButton = dialogButton;
            InitializeComponent();
            this.buildUI();
        }

        public new bool? Show()
        {
            return base.ShowDialog();
        }

        private void buildUI()
        {
            this.textBlockMessage.Text = this.message;
            this.labelTitle.Content = this.title;
            this.Title = this.title;
            if (string.IsNullOrEmpty(this.title))
            {
                this.labelTitle.Visibility = Visibility.Collapsed;
            }
            switch (this.dialogButton)
            {
                case DialogButton.YES:
                    {
                        this.buttonNo.Visibility = Visibility.Collapsed;
                        this.buttonYes.Visibility = Visibility.Visible;
                        break;
                    }
                case DialogButton.YES_NO:
                    {
                        this.buttonNo.Visibility = Visibility.Visible;
                        this.buttonYes.Visibility = Visibility.Visible;
                        break;
                    }
            }
        }

        private void buttonYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void buttonNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
        }
    }
}
