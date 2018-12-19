using System;
using System.Windows.Data;

namespace Arrowgene.Launcher.Translation
{
    public class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null)
                return string.Empty;

            if (parameter is string)
                return Translator.Instance.Translate((string)parameter);
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
