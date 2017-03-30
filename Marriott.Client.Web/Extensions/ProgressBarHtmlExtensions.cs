using System.Text;
using System.Web.Mvc;

namespace Marriott.Client.Web.Extensions
{
    public static class ProgressBarHtmlExtensions
    {
        public static MvcHtmlString RenderProgressBar(this HtmlHelper htmlHelper)
        {
            var sb = new StringBuilder();
            if (htmlHelper.ViewBag.ProgressBar != null)
            {
                string[] array = htmlHelper.ViewBag.ProgressBar.ToString().Split(',');
                var progressBar = new TagBuilder("div");
                progressBar.AddCssClass("btn-group");

                for (var i = 0; i < array.Length; i++)
                {
                    var innerHtml = i == array.Length-1 ? array[i] : array[i] + " &gt;";
                    var element = new TagBuilder("button") { InnerHtml = innerHtml };
                    element.MergeAttribute("type", "button");

                    if ((htmlHelper.ViewBag.ProgressBarActiveNode == null && i == 0) || i == htmlHelper.ViewBag.ProgressBarActiveNode)
                    {
                        element.AddCssClass("btn btn-primary active");
                    }
                    else
                    {
                        element.AddCssClass("btn btn-primary disabled");
                    }
                    progressBar.InnerHtml += element.ToString();
                }
                sb.Append(progressBar);
            }
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}