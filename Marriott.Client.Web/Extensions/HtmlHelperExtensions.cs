using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Marriott.Client.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString TextBoxWithBlankDefaultIntegerValue(this HtmlHelper htmlHelper, string name, int integerValue)
        {
            return htmlHelper.TextBox(name, integerValue == 0 ? "" : integerValue.ToString());
        }

        public static MvcHtmlString TextBoxWithBlankDefaultIntegerValue(this HtmlHelper htmlHelper, string name, int integerValue, object htmlAttributes)
        {
            return htmlHelper.TextBox(name, integerValue == 0 ? "" : integerValue.ToString(), htmlAttributes);
        }

        public static MvcHtmlString LabelWithColonFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            return helper.LabelFor(expression, $"{helper.DisplayNameFor(expression)}:");
        }

        public static MvcHtmlString LabelWithColonFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return helper.LabelFor(expression, $"{helper.DisplayNameFor(expression)}:", htmlAttributes);
        }

        public static MvcHtmlString CheckBoxFor<T>(this HtmlHelper<T> html, Expression<Func<T, bool>> expression, string labelText)
        {
            var completeHtml = new StringBuilder();

            var result = html.CheckBoxFor(expression).ToString();
            const string pattern = @"<input name=""[^""]+"" type=""hidden"" value=""false"" />";
            var cBox = Regex.Replace(result, pattern, "");

            completeHtml.AppendLine(cBox);

            var label = string.IsNullOrEmpty(labelText) ? html.LabelFor(expression).ToString() : html.LabelFor(expression, labelText).ToString();
            completeHtml.AppendLine(label);

            return MvcHtmlString.Create(completeHtml.ToString());
        }

        public static MvcHtmlString CheckBoxFor<T>(this HtmlHelper<T> html, Expression<Func<T, bool>> expression, string labelText, object htmlAttributes)
        {
            var completeHtml = new StringBuilder();

            var result = html.CheckBoxFor(expression, htmlAttributes).ToString();
            const string pattern = @"<input name=""[^""]+"" type=""hidden"" value=""false"" />";
            var cBox = Regex.Replace(result, pattern, "");

            completeHtml.AppendLine(cBox);

            var label = string.IsNullOrEmpty(labelText) ? html.LabelFor(expression).ToString() : html.LabelFor(expression, labelText).ToString();
            completeHtml.AppendLine(label);

            return MvcHtmlString.Create(completeHtml.ToString());
        }

        public static MvcHtmlString CheckBoxFor<T>(this HtmlHelper<T> html, Expression<Func<T, bool>> expression, string labelText, object htmlAttributes, bool hideLabelText)
        {
            var completeHtml = new StringBuilder();
            var modelName = ExpressionHelper.GetExpressionText(expression);

            var updatedName = modelName.Replace("[", "_").Replace("].", "__");

            var result = html.CheckBoxFor(expression, htmlAttributes).ToString();
            const string pattern = @"<input name=""[^""]+"" type=""hidden"" value=""false"" />";
            var cBox = Regex.Replace(result, pattern, "");

            completeHtml.AppendLine(cBox);

            if (hideLabelText)
            {
                var htmlBuilder = new StringBuilder();
                var tag = new TagBuilder("label");
                tag.Attributes.Add("for", updatedName);
                tag.Attributes.Add("style", "color: transparent; text-indent: 0px !important;");
                htmlBuilder.Append("|");
                tag.InnerHtml = htmlBuilder.ToString();
                completeHtml.AppendLine(tag.ToString());
            }
            else
            {
                var label = string.IsNullOrEmpty(labelText) ? html.LabelFor(expression).ToString() : html.LabelFor(expression, labelText).ToString();
                completeHtml.AppendLine(label);
            }

            return MvcHtmlString.Create(completeHtml.ToString());
        }
    }
}