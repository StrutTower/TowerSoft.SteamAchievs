using Microsoft.AspNetCore.Html;
using System;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;

namespace TowerSoft.SteamAchievs.Website.Utilities {
    public static class ViewUtilities {
        public static string ToLongString(this TimeSpan timeSpan) {
            string output = string.Empty;
            if (timeSpan.TotalHours >= 1) {
                int hours = (int)Math.Floor(timeSpan.TotalHours);
                output += $"{hours.ToString("N0")} hour";
                if (hours != 1) output += "s";
            } else {
                if (timeSpan.Minutes > 0) {
                    output += $"{timeSpan.Minutes} minute";
                    if (timeSpan.Minutes != 1) output += "s";
                }
            }
            return output;
        }

        /// <summary>
        /// Returns the view of the website project.
        /// </summary>
        public static string GetWebsiteVersion() {
            return Assembly.GetCallingAssembly().GetName().Version.ToString(3);
        }

        private static string ToRawString(this IHtmlContent htmlContent) {
            using StringWriter writer = new();
            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
