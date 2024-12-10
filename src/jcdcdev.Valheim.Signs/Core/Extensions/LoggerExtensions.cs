using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Logging;
using jcdcdev.Valheim.Signs;

namespace jcdcdev.Valheim.Core.Extensions;

public static class LoggerExtensions
{
    public static void LogIssue(Exception ex, string? title = null) { Jotunn.Logger.LogError(GetReportText(ex, title)); }

    public static void LogIssue(this ManualLogSource logger, Exception ex, string? title = null) { logger.LogError(GetReportText(ex, title)); }

    private static string GetReportText(Exception ex, string? titleText = null)
    {
        var title = ex.Message ?? "An error occurred";
        if (!string.IsNullOrWhiteSpace(titleText) && titleText != null)
        {
            title = titleText;
        }

        var summaryBuilder = new StringBuilder();

        summaryBuilder.AppendLine(title);
        summaryBuilder.AppendLine();
        summaryBuilder.AppendHeader("EXCEPTION");
        summaryBuilder.AppendLine();

        summaryBuilder.AppendLine(ex.ToString());
        var summary = summaryBuilder.ToString();

        var searchUrl = GetSearchIssueUrl(title);
        var createIssueUrl = GetCreateIssueUrl(title);
        var report = new StringBuilder();

        report.AppendLine();
        report.AppendHeader("ERROR REPORT", bold: true);
        report.AppendLine();
        report.AppendLine(summary);
        report.AppendLine();
        report.AppendHeader("HELP & SUPPORT", bold: true);
        report.AppendLine();
        report.AppendLine("Please check for existing issues or create a new issue on GitHub.");
        report.AppendLine();
        report.AppendHeader("SEARCH FOR EXISTING ISSUES");
        report.AppendLine($"\n{searchUrl}\n");
        report.AppendHeader("CREATE A NEW ISSUE");
        report.AppendLine($"\n{createIssueUrl}\n");
        return report.ToString();
    }

    private static string GetSearchIssueUrl(string? title)
    {
        return new UriBuilder($"{GitHubUrl}/issues")
            .AddQuery("q", Uri.EscapeUriString($"is:issue+is:open+{title}"))
            .ToUriString();
    }

    private static string GetCreateIssueUrl(string title = "An error occurred")
    {
        var query = new Dictionary<string, string>
        {
            { "title", Uri.EscapeUriString(title) },
            { "PackageVersion", Uri.EscapeUriString(VersionInfo.Version) },
            { "labels", "bug" },
            { "template", "bug.yml" }
        };

        return new UriBuilder($"{GitHubUrl}/issues/new")
            .AddQuery(query)
            .ToUriString();
    }

    private const string GitHubUrl = "https://github.com/jcdcdev/jcdcdev.Valheim.Signs";
}
