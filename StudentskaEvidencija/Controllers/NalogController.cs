using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentskaEvidencija.Controllers
{
    public class NalogController : Controller
    {
        //
        // GET: /Nalog/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string poruka=null)
        {
            if (poruka != null)
                ViewBag.porukaGreske = poruka;
            else
                ViewBag.porukaGreske = "";

            return View();
        }

        public ActionResult Proveri(string username, string password)
        {
            Models.StudentskaEvidencijaEntities entiteti = new Models.StudentskaEvidencijaEntities();
            var filtrirani = entiteti.Korisniks.Where(it => it.KorisnickoIme == username && it.Lozinka == password);
            if (filtrirani.Count() > 0)
                return RedirectToAction("Prikazi", "Studenti");
            else
            {
                return RedirectToAction("Login", new {poruka = "Nepostojeci korisnik!"});
            }

        }
    }
}
