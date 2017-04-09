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
    public class IspitiController : Controller
    {
        //
        // GET: /Ispiti/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrikaziIspite(int studentId)
        {
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();
            List<ModelIspit> listaIspita = new List<ModelIspit>();

            //punjenje student.json
           var pronadjeniStudenti =  entiteti.Students.Where(it => it.StudentID == studentId);
            if (pronadjeniStudenti.Count() > 0)
            {
                Student student = pronadjeniStudenti.FirstOrDefault();
                ModelStudent modelStudent = new ModelStudent(student.StudentID, student.ImePrezime, student.BrojIndeksa,
                    student.Finansiranje, student.Smer.NazivSmera, student.Slika);

                string studentjson = new JavaScriptSerializer().Serialize(modelStudent);

                StreamWriter _testDataStudent = new StreamWriter(Server.MapPath(@"\Content\student.json"), false);
                _testDataStudent.WriteLine(studentjson);
                _testDataStudent.Flush();
                _testDataStudent.Close();
                _testDataStudent.Dispose();
            }

           //punjenje ispiti.json

            foreach (var ispit in entiteti.Ispits.Where(it => it.StudentID == studentId))
            {
                listaIspita.Add(new ModelIspit(studentId + "",
                                                ispit.PredmetID + "",
                                                ispit.Predmet.NazivPredmeta + "",
                                                datumUStringMDG(ispit.Datum.Value) + "",
                                                ispit.Profesor.ImePrezime + "",
                                                ispit.Ocena.Value + "",
                                                ispit.ProfesorID + ""));
            }

            string json = new JavaScriptSerializer().Serialize(listaIspita);

            StreamWriter _testData = new StreamWriter(Server.MapPath(@"\Content\ispiti.json"), false);
            _testData.WriteLine(json);
            _testData.Flush();
            _testData.Close();
            _testData.Dispose();

            ViewBag.studentId = studentId;
            return View();
        }

        public ActionResult PrikaziIspitePoIndeksu(string index)
        {
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();
            var pronadjeniStudenti = entiteti.Students.Where(it => it.BrojIndeksa == index);
            if (pronadjeniStudenti.Count() > 0)
            {
                Student s = pronadjeniStudenti.FirstOrDefault();
                return RedirectToAction("PrikaziIspite", new { studentId = s.StudentID });
            }

            else
            {
                return RedirectToAction("Prikazi", "Greske", new
                {
                    porukaGreske = "Uneli ste nepostojeći broj indeksa.",
                    povratniLink = "/Studenti/Prikazi"
                });
            }
        }

        private string datumUStringMDG(DateTime datum)
        {
            string s = "";
            if (datum.Month < 10)
                s = s + "0" + datum.Month;
            else s = s + datum.Month;
            s += "-";
            if (datum.Day < 10)
                s = s + "0" + datum.Day;
            else s = s + datum.Day;
            s = s + "-" + datum.Year;
            return s;
        }

        private string datumUStringGMD(DateTime datum)
        {
            string s = "";
            s += datum.Year;
            s += "-";
            if (datum.Month < 10)
                s = s + "0" + datum.Month;
            else s = s + datum.Month;
            s += "-";
            if (datum.Day < 10)
                s = s + "0" + datum.Day;
            else s = s + datum.Day;
            return s;
        }

        public ActionResult DodajIspit(int studentId)
        {
            //popunjavanje predmeti.json - svi predmeti koje student moze da polaze
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();
            List<ModelPredmetProfesori> listaPP = new List<ModelPredmetProfesori>();
            Student student = entiteti.Students.Where(it => it.StudentID == studentId).FirstOrDefault();

            foreach (var predmet in entiteti.Predmets)
            {
                if (predmet.SmerID != student.SmerID) // filtriranje predmeta sa smera 
                    continue;

                // filtriranje polozenih predmeta - pocetak
                bool predmetPolozen = false;
                foreach (Ispit polozenIspit in student.Ispits)
                {
                    if (polozenIspit.PredmetID == predmet.PredmetID)
                    {
                        predmetPolozen = true;
                        break;
                    }
                }
                if (predmetPolozen)
                    continue;
                // filtriranje polozenih predmeta - kraj

                ModelPredmet modelPredmet = new ModelPredmet(predmet.PredmetID + "",
                                                             predmet.NazivPredmeta,
                                                             predmet.Smer.NazivSmera,
                                                             predmet.Poeni + "");
             
                ModelPredmetProfesori modelPP = new ModelPredmetProfesori(modelPredmet);

                foreach (var profesor in predmet.Profesors)
                {
                    modelPP.dodajProfesora(new ModelProfesor(profesor.ProfesorID + "",
                                                             profesor.ImePrezime,
                                                             profesor.GodinaZaposlenja + ""));
                }
                listaPP.Add(modelPP);
            }

            string json = new JavaScriptSerializer().Serialize(listaPP);

            StreamWriter _testData = new StreamWriter(Server.MapPath(@"\Content\predmeti.json"), false);
            _testData.WriteLine(json);
            _testData.Flush();
            _testData.Close();
            _testData.Dispose();

            //popunjavanje ispit.json
            ModelIspit modelIspit = new ModelIspit(studentId + "", "", "", "", "", "", "");

            string jsonIspit = new JavaScriptSerializer().Serialize(modelIspit);

            StreamWriter _testDataIspit = new StreamWriter(Server.MapPath(@"\Content\ispit.json"), false);
            _testDataIspit.WriteLine(jsonIspit);
            _testDataIspit.Flush();
            _testDataIspit.Close();
            _testDataIspit.Dispose();
            return View();
        }
        
        public ActionResult SacuvajIspit(int studentId, int predmetId, int ocena, string datum, int profesorId)
        {
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();
            var pronadjeniStudenti = entiteti.Students.Where(it => it.StudentID == studentId);
            if (pronadjeniStudenti.Count() > 0)
            {
                Student s = pronadjeniStudenti.FirstOrDefault();
                Ispit i = new Ispit();
                i.StudentID = studentId;
                i.PredmetID = predmetId;
                i.Ocena = ocena;
                i.Datum = DateTime.Parse(datum);
                i.ProfesorID = profesorId;
                s.Ispits.Add(i);
                entiteti.SaveChanges();
            }
            return RedirectToAction("PrikaziIspite", new { studentId = studentId });
        }

        public ActionResult ObrisiIspit(int studentId, int predmetId)
        {
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();
            var pronadjeniStudenti = entiteti.Students.Where(it => it.StudentID == studentId);
            if (pronadjeniStudenti.Count() > 0)
            {
                Student s = pronadjeniStudenti.FirstOrDefault();
                s.Ispits.Remove(s.Ispits.Where(it => it.StudentID == studentId && it.PredmetID == predmetId).FirstOrDefault());
                entiteti.SaveChanges();
            }
            return RedirectToAction("PrikaziIspite", new { studentId = studentId });
        }

        public ActionResult IzmeniIspit(int studentId, int predmetId)
        {
            //popunjavanje predmeti.json
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();
            List<ModelPredmetProfesori> listaPP = new List<ModelPredmetProfesori>();
            Student student = entiteti.Students.Where(it => it.StudentID == studentId).FirstOrDefault();

            foreach (var predmet in entiteti.Predmets)
            {
                if (predmet.PredmetID != predmetId)
                    continue;
                if (predmet.SmerID != student.SmerID)
                    continue;

                ModelPredmet modelPredmet = new ModelPredmet(predmet.PredmetID + "",
                                                             predmet.NazivPredmeta,
                                                             predmet.Smer.NazivSmera,
                                                             predmet.Poeni + "");

                ModelPredmetProfesori modelPP = new ModelPredmetProfesori(modelPredmet);

                foreach (var profesor in predmet.Profesors)
                {
                    modelPP.dodajProfesora(new ModelProfesor(profesor.ProfesorID + "",
                                                             profesor.ImePrezime,
                                                             profesor.GodinaZaposlenja + ""));
                }
                listaPP.Add(modelPP);
            }

            string json = new JavaScriptSerializer().Serialize(listaPP);

            StreamWriter _testData = new StreamWriter(Server.MapPath(@"\Content\predmeti.json"), false);
            _testData.WriteLine(json);
            _testData.Flush();
            _testData.Close();
            _testData.Dispose();

            //popunjavanja ispit.json
            var pronadjeniIspiti = entiteti.Ispits.Where(it => it.StudentID == studentId && it.PredmetID == predmetId);
            if (pronadjeniIspiti.Count() > 0)
            {
                Ispit i = pronadjeniIspiti.FirstOrDefault();
                ModelIspit mi = new ModelIspit(i.StudentID + "",
                                                i.PredmetID + "",
                                                i.Predmet.NazivPredmeta + "",
                                                datumUStringGMD(i.Datum.Value),
                                                i.Profesor.ImePrezime,
                                                i.Ocena.Value + "",
                                                i.ProfesorID + "");

                string jsonIspit = new JavaScriptSerializer().Serialize(mi);

                StreamWriter _testDataIspit = new StreamWriter(Server.MapPath(@"\Content\ispit.json"), false);
                _testDataIspit.WriteLine(jsonIspit);
                _testDataIspit.Flush();
                _testDataIspit.Close();
                _testDataIspit.Dispose();
            }

            return View();
        }

        public ActionResult SacuvajIzmene(int studentId, int predmetId, int ocena, string datum, int profesorId)
        {
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();

            var pronadjeniIspiti = entiteti.Ispits.Where(it => it.PredmetID == predmetId && it.StudentID == studentId);
            if (pronadjeniIspiti.Count() > 0)
            {
                Ispit i = pronadjeniIspiti.FirstOrDefault();
                i.Ocena = ocena;
                i.Datum = DateTime.Parse(datum);
                i.ProfesorID = profesorId;
                entiteti.SaveChanges();
            }
            
            return RedirectToAction("PrikaziIspite", new { studentId = studentId });
        }
    }
}
