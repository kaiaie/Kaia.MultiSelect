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


        public ActionResult ViewOrPartial()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View();
        }


        public ActionResult ViewOrPartial(string viewName)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName);
            }
            return View(viewName);
        }


        public ActionResult ViewOrPartial(object model)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }


        public ActionResult ViewOrPartial(string viewName, object model)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName, model);
            }
            return View(viewName, model);
        }
    }
}
