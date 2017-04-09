using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using StudentskaEvidencija.Models;
using System.IO;

namespace StudentskaEvidencija.Controllers
{
    public class StudentiController : Controller
    {
        //
        // GET: /Studenti/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Prikazi()
        {
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();
            List<ModelStudent> listaStudenata = new List<ModelStudent>();

            foreach(var student in entiteti.Students)
            {
                listaStudenata.Add(new ModelStudent(student.StudentID,
                                                    student.ImePrezime,
                                                    student.BrojIndeksa,
                                                    student.Finansiranje,
                                                    student.Smer.NazivSmera,
                                                    student.Slika));
            }

            string json = new JavaScriptSerializer().Serialize(listaStudenata);

            StreamWriter _testData = new StreamWriter(Server.MapPath(@"\Content\studenti.json"), false);
            _testData.WriteLine(json);
            _testData.Flush();
            _testData.Close(); 
            _testData.Dispose(); 

            return View();
        }

        
    }
}
