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
        /// <summary>
        /// Builds a checkbox that allows multiple rows in a table to be 
        /// selected
        /// </summary>
        public static MvcHtmlString RowCheckboxFor(this HtmlHelper helper,
            Expression<Func<object, long>> idProperty, dynamic htmlAttributes = null)
        {
            return MvcHtmlString.Create(@"<input type=""checkbox"">");
        }
    }
}
