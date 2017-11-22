using Kaia.Common.DataAccess.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kaia.Common.Web.Binders
{
    public class EntityModifierBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.RequestContext.HttpContext.Request;
            var len = request.ContentLength;
            var buffer = new byte[len];
            request.InputStream.Read(buffer, 0, len);
            var payload = HttpUtility.ParseQueryString(Encoding.UTF8.GetString(buffer));

            var ids = new List<long>();
            ids.AddRange(
                payload["id"].Split(new char[] { ',' })
                    .Select(i => long.Parse(i)));

            var result = Activator.CreateInstance(bindingContext.ModelType, 
                ids);
            ((IEntityModifier)result).ModificationType = 
                (DataAccess.ModificationType)Enum.Parse(typeof(DataAccess.ModificationType), 
                    payload["ModificationType"]);
            foreach (var prop in result.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => pi.PropertyType.IsGenericType && 
                    pi.PropertyType.GetGenericTypeDefinition() == 
                        typeof(DataAccess.UpdatableField<>)))
            {
                var propertyName = prop.Name;
                var entityPropType = prop.PropertyType.GenericTypeArguments[0];
                var isIndeterminate = bool.Parse(payload[string.Concat(propertyName, "_IsIndeterminate")]);
                var oldString = payload[string.Concat(propertyName, "_OldValue")];
                var newString = payload[string.Concat(propertyName, "_NewValue")];
                object field = null;
                if (isIndeterminate)
                {
                    field = Activator.CreateInstance(prop.PropertyType, 
                        new object[] { true });
                }
                else
                {
                    field = Activator.CreateInstance(prop.PropertyType, 
                        new object[]
                        {
                            TypeDescriptor.GetConverter(entityPropType).ConvertFromString(oldString),
                            true
                        });
                }
                if (oldString != newString)
                {
                    var newValueProp = prop.PropertyType.GetProperty("Value", 
                        BindingFlags.Public | BindingFlags.Instance);
                    newValueProp.SetValue(field, 
                        TypeDescriptor.GetConverter(entityPropType).ConvertFromString(newString));
                }
                prop.SetValue(result, field);
            }
            return result;
        }
    }
}
