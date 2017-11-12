using NLog;
using System.Web.Mvc;

namespace Kaia.Common.Web.Controllers
{
    /// <summary>
    /// Controller base class with some additional functionality (e.g., logging)
    /// </summary>
    public abstract class ControllerBase : Controller
    {
        private readonly ILogger _logger;

        protected ILogger Logger { get { return _logger; } }

        protected ControllerBase()
        {
            _logger = LogManager.GetLogger(GetType().FullName);
        }


        protected ActionResult ViewOrPartial()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View();
        }


        protected ActionResult ViewOrPartial(string viewName)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName);
            }
            return View(viewName);
        }


        protected ActionResult ViewOrPartial(object model)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }


        protected ActionResult ViewOrPartial(string viewName, object model)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName, model);
            }
            return View(viewName, model);
        }


        protected void SetMessageText(string messageText)
        {
            TempData["Kaia.Message"] = messageText;
        }
    }
}
