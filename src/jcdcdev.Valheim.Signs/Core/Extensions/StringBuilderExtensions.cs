using System.Text;

namespace jcdcdev.Valheim.Core.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendHeader(this StringBuilder sb, string header, bool bold = false)
    {
        if (bold)
        {
            sb.AppendLine(new string('=', header.Length));
        }

        sb.AppendLine($"{header}");
        if (bold)
        {
            sb.AppendLine(new string('=', header.Length));
        }
        else
        {
            sb.AppendLine(new string('-', header.Length));
        }

        return sb;
    }
}
