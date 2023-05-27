using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;

namespace TowerSoft.SteamAchievs.Website.Utilities {
    public static class ViewUtilities {
        /// <summary>
        /// Convert the boolean to an icon.
        /// </summary>
        /// <param name="boolean">Boolean to display as an icon</param>
        public static IHtmlContent BooleanIcon(bool boolean) {
            TagBuilder tag = new("span");
            if (boolean) {
                tag.AddCssClass("mdi mdi-check text-success");
            } else {
                tag.AddCssClass("mdi mdi-cancel text-danger");
            }
            return tag;
        }

        /// <summary>
        /// Convert the boolean to an icon.
        /// </summary>
        /// <param name="boolean">Boolean to display as an icon</param>
        public static IHtmlContent BooleanIconAndText(bool? boolean) {
            TagBuilder tag = new("span");
            if (boolean.HasValue && boolean.Value) {
                tag.AddCssClass("mdi mdi-checkbox-marked-outline text-success");
                tag.Attributes.Add("title", "True");
            } else if (boolean.HasValue) {
                tag.AddCssClass("mdi mdi-checkbox-blank-off-outline text-danger");
                tag.Attributes.Add("title", "False");
            } else {
                tag.AddCssClass("mdi mdi-checkbox-blank-outline text-secondary");
                tag.Attributes.Add("title", "Unknown");
            }
            TagBuilder container = new("span");
            container.InnerHtml.AppendHtml(tag.ToRawString() + " ");
            if (boolean.HasValue && boolean.Value) {
                container.InnerHtml.AppendHtml("True");
            } else if (boolean.HasValue) {
                container.InnerHtml.AppendHtml("False");
            } else {
                container.InnerHtml.AppendHtml("Unknown");
            }
            return container;
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
