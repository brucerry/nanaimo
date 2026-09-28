using System.ComponentModel;
using System.Globalization;

namespace FlightIslandServer.Desktop.Models;

public sealed class TraditionalBooleanConverter : BooleanConverter
{
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => destinationType == typeof(string) && value is bool flag
            ? flag ? "是" : "否"
            : base.ConvertTo(context, culture, value, destinationType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value is string text && text is "是" or "否"
            ? text == "是"
            : base.ConvertFrom(context, culture, value);
}
