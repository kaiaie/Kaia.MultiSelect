using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kaia.MultiSelect.Web.Controllers
{
    public class HomeController : Common.Web.Controllers.ControllerBase
    {
        // GET: Home
        public ActionResult Index()
        {
            return ViewOrPartial();
        }
    }
}