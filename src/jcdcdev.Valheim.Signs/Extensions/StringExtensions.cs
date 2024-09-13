using System;

namespace jcdcdev.Valheim.Signs.Extensions;

public static class StringExtensions
{
    public static bool InvariantEquals(this string value, string compare) => value.Equals(compare, StringComparison.InvariantCultureIgnoreCase);
    public static bool StartsWithInvariant(this string value, string compare) => value.StartsWith(compare, StringComparison.InvariantCultureIgnoreCase);
}
