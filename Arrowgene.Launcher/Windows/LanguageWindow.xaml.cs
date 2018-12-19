namespace Arrowgene.Launcher.Windows
{
    using Arrowgene.Launcher.Translation;
    using System.Windows;
    using System.Windows.Input;

    public partial class LanguageWindow : Window
    {
        public LanguageWindow(Window owner)
        {
            if (owner.IsLoaded)
            {
                this.Owner = owner;
            }
            InitializeComponent();
            foreach (Language language in Translator.Instance.Languages)
            {
                this.stackPanelLanguages.Children.Add(language.SelectLanguageButton);
                language.SelectLanguageButton.Click += SelectLanguageButton_Click;
            }
        }

        private void SelectLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Language language in Translator.Instance.Languages)
            {
                language.SelectLanguageButton.Click -= SelectLanguageButton_Click;
            }
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

    }
}
