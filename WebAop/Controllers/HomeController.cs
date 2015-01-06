using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cache;

namespace WebAop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ITalk talk = ServiceLocator.Instance.GetService<ITalk>();
            var list= talk.GetData();
            return View(list);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}