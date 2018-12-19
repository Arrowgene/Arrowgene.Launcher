using Arrowgene.Launcher.Translation.Languages;
using System.Collections.Generic;

namespace Arrowgene.Launcher.Translation
{
    public class Translator
    {
        private static Translator _instance = new Translator();
        public static Translator Instance => _instance;

        private LanguageType _language;
        private List<Language> _languages;
        private Language _current;

        public Translator()
        {
            _language = LanguageType.English;
            _languages = new List<Language>();
            _languages.Add(new English());
            _languages.Add(new Chinese());
            _languages.Add(new Korean());
            _languages.Add(new Spanish());
            ChangeLanguage(LanguageType.English);
        }

        public LanguageType Current => _language;

        public List<Language> Languages => new List<Language>(_languages);

        public string Translate(string key)
        {
            return _current.Translate(key);
        }

        public void ChangeLanguage(LanguageType languageType)
        {
            foreach (Language language in _languages)
            {
                if (language.LanguageType == languageType)
                {
                    _language = languageType;
                    _current = language;
                    break;
                }
            }
        }
    }
}
