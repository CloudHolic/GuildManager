using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;
using System;
using GuildManager.Utils;

namespace GuildManager.Converters;

[ValueConversion(typeof(Enum), typeof(string))]
public class EnumDescriptionConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is Enum enumValue ? enumValue.GetDescription() : DependencyProperty.UnsetValue;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
}