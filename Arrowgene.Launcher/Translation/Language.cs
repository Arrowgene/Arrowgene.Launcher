using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Arrowgene.Launcher.Translation
{
    public abstract class Language
    {
        protected Dictionary<string, string> _translations;

        public Button SelectLanguageButton { get; }

        public abstract string FlagRessource { get; }
        public abstract LanguageType LanguageType { get; }

        protected Language()
        {
            _translations = new Dictionary<string, string>();
            SelectLanguageButton = new Button();
            SelectLanguageButton.Style = App.Window.Resources["LanguageButtonStyle"] as Style;
            SelectLanguageButton.Background = new ImageBrush(new BitmapImage(new Uri(FlagRessource)));
            SelectLanguageButton.Click += SelectLanguageButton_Click;
        }

        private void SelectLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            Translator.Instance.ChangeLanguage(LanguageType);
        }

        public string Translate(string key)
        {
            if (_translations.ContainsKey(key))
            {
                return _translations[key];
            }
            return key;
        }
    }
}
