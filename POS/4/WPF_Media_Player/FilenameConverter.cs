using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace WPF_Media_Player {
    public class FilenameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is string path)
                return Path.GetFileName(path);
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
