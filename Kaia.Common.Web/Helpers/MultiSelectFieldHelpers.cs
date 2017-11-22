using Kaia.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Kaia.Common.Web.Helpers
{
    public static class MultiSelectFieldHelpers
    {
        // TODO: Add L10N, apply HTML custom attributes
        public const string MultiSelectMessage = "(Multiple values)";

        public static MvcHtmlString MultiSelectTextBoxFor<TModel, TProp>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, UpdatableField<TProp>>> updatableField,
            dynamic htmlAttributes = null)
        {
            if (updatableField.NodeType != ExpressionType.Lambda || 
                updatableField.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException(nameof(updatableField));
            }
            var node = updatableField.Body as MemberExpression;
            var fieldName = node.Member.Name;
            var fieldValue = updatableField.Compile()(helper.ViewData.Model);

            var indeterminateFlagElement = new TagBuilder("input");
            indeterminateFlagElement.MergeAttribute("name", 
                string.Format("{0}_IsIndeterminate", fieldName));
            indeterminateFlagElement.MergeAttribute("type", "hidden");
            indeterminateFlagElement.MergeAttribute("class", "kaia-ms-flag");
            indeterminateFlagElement.MergeAttribute("value", 
                fieldValue.IsIndeterminate.ToString());

            var oldValueElement = new TagBuilder("input");
            oldValueElement.MergeAttribute("name",
                string.Format("{0}_OldValue", fieldName));
            oldValueElement.MergeAttribute("type", "hidden");
            oldValueElement.MergeAttribute("class", "kaia-ms-old");
            if (fieldValue.IsIndeterminate)
            {
                oldValueElement.MergeAttribute("value", MultiSelectMessage);
            }
            else
            {
                oldValueElement.MergeAttribute("value", fieldValue.Value.ToString());
            }

            var newValueElement = new TagBuilder("input");
            // TODO: Add better intelligent input types, e.g., based on 
            // DataTypeAttribute
            newValueElement.MergeAttribute("name",
                string.Format("{0}_NewValue", fieldName));
            newValueElement.MergeAttribute("type", "text");
            newValueElement.MergeAttribute("class", "kaia-ms-new form-control");
            if (!fieldValue.IsUpdatable)
            {
                newValueElement.MergeAttribute("readonly", "readonly");
            }
            if (fieldValue.IsUpdated || !fieldValue.IsIndeterminate)
            {
                newValueElement.MergeAttribute("value", fieldValue.Value.ToString());
            }
            else
            {
                newValueElement.MergeAttribute("value", MultiSelectMessage);
            }
            IDictionary<string, string> properties =
                TagBuilderExtensions.DynamicToProperties(htmlAttributes);
            newValueElement.AddCustomHtmlAttributes(properties);

            var controlElement = new TagBuilder("span");
            controlElement.MergeAttribute("class", "kaia-ms-ctl");
            controlElement.MergeAttribute("data-kaia-ms-name", fieldName);
            controlElement.InnerHtml = string.Join(Environment.NewLine,
                new string[]
                {
                    indeterminateFlagElement.ToString(),
                    oldValueElement.ToString(),
                    newValueElement.ToString()
                });

            var result = MvcHtmlString.Create(controlElement.ToString());
            return result;
        }
    }
}
