using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GuildManager.Converters;

[ValueConversion(typeof(Nullable), typeof(string))]
public class NullValueConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return parameter as string ?? "No Data";

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}