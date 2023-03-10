using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Linq;

namespace GuildManager.Utils;

public static class EnumExtensions
{
    public class ValueDescription
    {
        public Enum? Value { get; set; }

        public string Description { get; set; }

        public ValueDescription()
        {
            Value = default;
            Description = string.Empty;
        }
    }

    public static string GetDescription(this Enum value) =>
        value.GetType()
            .GetField(value.ToString())
            ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() is DescriptionAttribute description
            ? description.Description
            : value.ToString();

    public static IEnumerable<ValueDescription> GetValuesAndDescriptions(Type? type, Func<Enum, bool>? filter = null)
    {
        if (type is not { IsEnum: true })
            return new List<ValueDescription>();

        filter ??= _ => true;

        return Enum.GetValues(type).Cast<Enum>()
            .Where(filter)
            .Select(e => new ValueDescription
            {
                Value = e,
                Description = e.GetDescription()
            }).ToList();
    }
}