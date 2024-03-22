using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;

namespace ProjectAspEShop2024.Helpers
{
    [HtmlTargetElement("Pagination")]
    public class PaginationTagHelper : TagHelper
    {
        [HtmlAttributeName("ActiveCategory")]
        public string ActiveCategory { get; set; }

        [HtmlAttributeName("ActivePage")]
        public int ActivePage { get; set; }


        [HtmlAttributeName("TotalPages")]
        public int TotalPages { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.AddClass("pagination", HtmlEncoder.Default);

            int interval = 5;

            int left = ActivePage - interval;
            int right = ActivePage + interval;

            left = left < 1 ? 1 : left;
            right = right > TotalPages ? TotalPages : right;

            var tagUl = new TagBuilder("ul");
            for (int i = left; i <= right; i++)
            {
                var tagLi = new TagBuilder("li");

                string active =
                    i == ActivePage ? "active" : string.Empty;

                tagLi.AddCssClass("page-item " + active);

                var tagA = new TagBuilder("a");// <a></a>


                if (String.IsNullOrEmpty(ActiveCategory))
                    tagA.MergeAttribute("href",
                        // дефолтный паттерн controller/action/id
                        $"/Product/Index/?page={i}");
                else tagA.MergeAttribute("href",
                    // паттерн из системы Маршрутизации MapControllerRoute
                    $"/Product/Index/?categoryName={ActiveCategory}&page={i}");


                tagA.InnerHtml.SetHtmlContent(i.ToString());
                tagA.AddCssClass("page-link");
                tagLi.InnerHtml.AppendHtml(tagA);

                tagUl.InnerHtml.AppendHtml(tagLi);
            }

            var writer = new System.IO.StringWriter();
            tagUl.InnerHtml.WriteTo(writer, HtmlEncoder.Default);

            output.Content.SetHtmlContent(writer.ToString());
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
