using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoinFill.Helpers.Extensions
{
    public static class HtmlHelperExtensions
    {
        private const string _partialViewResourceItemPrefix = "resources_";
        private const string _partialViewScriptItemPrefix = "scripts_";

        public static async Task RenderMenu(this IHtmlHelper html)
        {
            await html.RenderPartialAsync("Menues/_ClientMenu");
        }

        public static async Task RenderUnauthMenu(this IHtmlHelper html)
        {
            await html.RenderPartialAsync("Menues/_UnauthMenu");
        }

        public static IHtmlContent PartialSectionScripts(this IHtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            try
            {
                htmlHelper.ViewContext.HttpContext.Items[_partialViewScriptItemPrefix + Guid.NewGuid()] = template;
                return new HtmlContentBuilder();
            }
            catch (Exception e)
            {
                return new HtmlContentBuilder();
            }
        }

        public static IHtmlContent RenderPartialSectionScripts(this IHtmlHelper htmlHelper)
        {
            try
            {
                var partialSectionScripts = htmlHelper.ViewContext.HttpContext.Items.Keys
                    .Where(k => Regex.IsMatch(
                        k.ToString(),
                        "^" + _partialViewScriptItemPrefix + "([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})$"));
                var contentBuilder = new HtmlContentBuilder();
                foreach (var key in partialSectionScripts)
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null)
                    {
                        var writer = new StringWriter();
                        template(null).WriteTo(writer, HtmlEncoder.Default);
                        contentBuilder.AppendHtml(writer.ToString());
                    }
                }
                return contentBuilder;
            }
            catch (Exception e)
            {
                return new HtmlContentBuilder();
            }
        }
    }
}
