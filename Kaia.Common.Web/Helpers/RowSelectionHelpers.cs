using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kaia.Common.Web.Helpers
{
    public static class RowSelectionHelpers
    {
        private static long controlId;
        /// <summary>
        /// Builds a checkbox that allows multiple rows in a table to be 
        /// selected
        /// </summary>
        public static MvcHtmlString RowCheckboxFor<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, long>> idProperty, dynamic htmlAttributes = null)
        {
            if (idProperty.NodeType != ExpressionType.Lambda || 
                idProperty.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException(nameof(idProperty));
            }
            var value = idProperty.Compile()(helper.ViewData.Model).ToString();

            var inputTag = new TagBuilder("input");
            inputTag.MergeAttribute("name", "ids");
            inputTag.MergeAttribute("type", "checkbox");
            inputTag.MergeAttribute("value", value);
            inputTag.MergeAttribute("id", string.Format("id{0}", controlId++));
            IDictionary<string, string> properties = 
                TagBuilderExtensions.DynamicToProperties(htmlAttributes);
            inputTag.AddCustomHtmlAttributes(properties);
            return MvcHtmlString.Create(inputTag.ToString());
        }
    }
}
