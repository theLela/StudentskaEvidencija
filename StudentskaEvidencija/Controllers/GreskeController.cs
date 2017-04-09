using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentskaEvidencija.Controllers
{
    public class GreskeController : Controller
    {
        //
        // GET: /Greske/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Prikazi(string porukaGreske, string povratniLink)
        {
            ViewBag.poruka = porukaGreske;
            ViewBag.link = povratniLink;
            return View();
        }

    }
}
