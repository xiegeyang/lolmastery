using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lolmastery.Controllers
{
    public class LOLController : Controller
    {
        // GET: LOL
        public ActionResult Index()
        {
            return View();
        }
    }
}