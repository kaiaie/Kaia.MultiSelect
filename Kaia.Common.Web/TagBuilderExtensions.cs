using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace Kaia.Common.Web
{
    public static class TagBuilderExtensions
    {
        public static IDictionary<string, string> DynamicToProperties(dynamic properties)
        {
            if (properties == null) return null;

            var result = new Dictionary<string, string>();
            foreach (var prop in properties.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                result.Add(prop.Name,
                    prop.GetValue(properties).ToString());
            }

            return result;
        }

        public static void AddCustomHtmlAttributes(this TagBuilder builder,
            IDictionary<string, string> properties = null)
        {
            if (properties == null) return;

            foreach (var property in properties)
            {
                builder.MergeAttribute(property.Key.Replace("_", "-"),
                    property.Value);
            }
        }
    }
}
