using GuildManager.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GuildManager.Converters;

[ValueConversion(typeof(Enum), typeof(IEnumerable<EnumExtensions.ValueDescription>))]
public class EnumToCollectionConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        EnumExtensions.GetValuesAndDescriptions(value.GetType());

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}