using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ProjectAspEShop2024.Models;
using System.Text.Encodings.Web;

namespace ProjectAspEShop2024.Helpers
{
    public static class HelperPagination
    {
        public static HtmlString SimplePagination(this IHtmlHelper html, ProductsPageViewModel model)
        {
            int interval = 5;

            int left = model.ActivePage - interval;
            int right = model.ActivePage + interval;

            left = left < 1 ? 1 : left;
            right = right > model.PageQuantity ? model.PageQuantity : right;

            var tagDiv = new TagBuilder("div");
            for (int i = left; i <= right; i++)
            {
                var tagA = new TagBuilder("a");// <a></a>
                tagA.MergeAttribute("href", $"/Product/?page={i}");
                tagA.InnerHtml.SetHtmlContent(i.ToString());

                tagDiv.InnerHtml.AppendHtml(tagA);
            }

            var writer = new System.IO.StringWriter();
            tagDiv.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }

        public static HtmlString BootstrapPagination(this IHtmlHelper html, ProductsPageViewModel model)
        {
            int interval = 5;

            int left = model.ActivePage - interval;
            int right = model.ActivePage + interval;

            left = left < 1 ? 1 : left;
            right = right > model.PageQuantity ? model.PageQuantity : right;

            var tagUl = new TagBuilder("ul");
            tagUl.AddCssClass("pagination");
            for (int i = left; i <= right; i++)
            {
                var tagLi = new TagBuilder("li");
                
                string active = 
                    i == model.ActivePage ? "active" : string.Empty;

                tagLi.AddCssClass("page-item "+active);

                var tagA = new TagBuilder("a");// <a></a>

                if (String.IsNullOrEmpty(model.ActiveCategory))
                    tagA.MergeAttribute("href",
                        // дефолтный паттерн controller/action/id6
                        $"/Product/Index/?page={i}");
                else tagA.MergeAttribute("href",
                    // паттерн из системы Маршрутизации MapControllerRoute
                    $"/Product/Index/?categoryName={model.ActiveCategory}&page={i}");

                tagA.InnerHtml.SetHtmlContent(i.ToString());
                tagA.AddCssClass("page-link");
                tagLi.InnerHtml.AppendHtml(tagA);

                tagUl.InnerHtml.AppendHtml(tagLi);
            }

            var writer = new System.IO.StringWriter();
            tagUl.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }
    }
}
