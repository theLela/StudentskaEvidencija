using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentskaEvidencija.Models;
using System.Web.Script.Serialization;
using System.IO;

namespace StudentskaEvidencija.Controllers
{
    public class PredmetiController : Controller
    {
        //
        // GET: /Predmeti/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrikaziPredmete()
        {
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();
        
            // pravljenje predmetiprofesori.json
            List<ModelPredmetProfesori> listaPP = new List<ModelPredmetProfesori>();
            foreach (var predmet in entiteti.Predmets)
            {
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

            string jsonPP = new JavaScriptSerializer().Serialize(listaPP);

            StreamWriter _testDataPP = new StreamWriter(Server.MapPath(@"\Content\predmetiprofesori.json"), false);
            _testDataPP.WriteLine(jsonPP);
            _testDataPP.Flush();
            _testDataPP.Close();
            _testDataPP.Dispose();

            return View();
        }

        public ActionResult ObrisiPredmet(int predmetId)
        {
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();
            Predmet p = entiteti.Predmets.Where(it => it.PredmetID == predmetId).FirstOrDefault();
            p.Profesors.Clear();
            p.Ispits.Clear();
            entiteti.SaveChanges();
            entiteti.Predmets.DeleteObject(p);
            entiteti.SaveChanges();

            return RedirectToAction("PrikaziPredmete");
        }

        public ActionResult DodajPredmet()
        {
           
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();

            //pravljenje smerovi.json
            List<ModelSmer> listaSmerova = new List<ModelSmer>();

            foreach (var smer in entiteti.Smers)
            {
                ModelSmer modelSmer = new ModelSmer(smer.SmerID + "",
                                                    smer.NazivSmera);
                listaSmerova.Add(modelSmer);
            }

            string json = new JavaScriptSerializer().Serialize(listaSmerova);

            StreamWriter _testData = new StreamWriter(Server.MapPath(@"\Content\smerovi.json"), false);
            _testData.WriteLine(json);
            _testData.Flush();
            _testData.Close();
            _testData.Dispose();

            //pravljenje profesori.json
            List<ModelProfesor> listaProfesora = new List<ModelProfesor>();

            foreach (var profesor in entiteti.Profesors)
            {
                ModelProfesor modelProf = new ModelProfesor(profesor.ProfesorID + "",
                                                            profesor.ImePrezime,
                                                            profesor.GodinaZaposlenja + "");
                listaProfesora.Add(modelProf);
            }

            string jsonProf = new JavaScriptSerializer().Serialize(listaProfesora);

            StreamWriter _testDataProf = new StreamWriter(Server.MapPath(@"\Content\profesori.json"), false);
            _testDataProf.WriteLine(jsonProf);
            _testDataProf.Flush();
            _testDataProf.Close();
            _testDataProf.Dispose();

            //pravljenje polupraznog predmet.json
            
            ModelPredmetProfesori modelPP = new ModelPredmetProfesori(new ModelPredmet("","",
                                                                        entiteti.Smers.FirstOrDefault().SmerID + "",""));
                                                                        
            string jsonModelPP = new JavaScriptSerializer().Serialize(modelPP);

            StreamWriter _testDataPP = new StreamWriter(Server.MapPath(@"\Content\predmet.json"), false);
            _testDataPP.WriteLine(jsonModelPP);
            _testDataPP.Flush();
            _testDataPP.Close();
            _testDataPP.Dispose();

            return View();
        }

        public ActionResult SacuvajPredmet(string nazivPredmeta, int smerId, int poeni, string idsProfesora)
        {
            List<string> nizIDsProfesora = new List<string>(idsProfesora.Split(new char[] { ',' }));
            if (nizIDsProfesora.Count == 1 && nizIDsProfesora.First() == "")
                nizIDsProfesora.Clear();

            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();
            Predmet p = new Predmet();
            p.PredmetID = entiteti.Predmets.Max(it => it.PredmetID) + 1;
            p.NazivPredmeta = nazivPredmeta;
            p.SmerID = smerId;
            p.Poeni = poeni;
            p.Profesors.Clear();

            foreach (string profesorIdStr in nizIDsProfesora) 
            {
                int profesorId = Int32.Parse(profesorIdStr);
                Profesor prof = entiteti.Profesors.Where(it => it.ProfesorID == profesorId).FirstOrDefault();
                p.Profesors.Add(prof);
            }

            entiteti.AddToPredmets(p);
            entiteti.SaveChanges();

            return RedirectToAction("PrikaziPredmete");
        }

        public ActionResult IzmeniPredmet(int predmetId)
        {
            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();

            //pravljenje smerovi.json
            List<ModelSmer> listaSmerova = new List<ModelSmer>();

            foreach (var smer in entiteti.Smers)
            {
                ModelSmer modelSmer = new ModelSmer(smer.SmerID + "",
                                                    smer.NazivSmera);
                listaSmerova.Add(modelSmer);
            }

            string json = new JavaScriptSerializer().Serialize(listaSmerova);

            StreamWriter _testData = new StreamWriter(Server.MapPath(@"\Content\smerovi.json"), false);
            _testData.WriteLine(json);
            _testData.Flush();
            _testData.Close();
            _testData.Dispose();

            //pravljenje profesori.json
            List<ModelProfesor> listaProfesora = new List<ModelProfesor>();

            foreach (var profesor in entiteti.Profesors)
            {
                ModelProfesor modelProf = new ModelProfesor(profesor.ProfesorID + "",
                                                            profesor.ImePrezime,
                                                            profesor.GodinaZaposlenja + "");
                listaProfesora.Add(modelProf);
            }

            string jsonProf = new JavaScriptSerializer().Serialize(listaProfesora);

            StreamWriter _testDataProf = new StreamWriter(Server.MapPath(@"\Content\profesori.json"), false);
            _testDataProf.WriteLine(jsonProf);
            _testDataProf.Flush();
            _testDataProf.Close();
            _testDataProf.Dispose();

            //pravljanje predmet.json
            Predmet p = entiteti.Predmets.Where(it => it.PredmetID == predmetId).FirstOrDefault();
            ModelPredmetProfesori modelPP = new ModelPredmetProfesori(new ModelPredmet(p.PredmetID + "",
                                                                         p.NazivPredmeta,
                                                                         p.SmerID + "",
                                                                         p.Poeni + ""));
            foreach (Profesor profesor in p.Profesors)
            {
                modelPP.dodajProfesora(new ModelProfesor(profesor.ProfesorID + "",
                                                         profesor.ImePrezime,
                                                         profesor.GodinaZaposlenja + ""));
            }

            string jsonModelPP = new JavaScriptSerializer().Serialize(modelPP);

            StreamWriter _testDataPP = new StreamWriter(Server.MapPath(@"\Content\predmet.json"), false);
            _testDataPP.WriteLine(jsonModelPP);
            _testDataPP.Flush();
            _testDataPP.Close();
            _testDataPP.Dispose();
            return View();
        }

        public ActionResult SacuvajIzmene(int predmetId, string nazivPredmeta, int smerId, int poeni, string idsProfesora)
        {
           List<string> nizIdsProf = new List<string>(idsProfesora.Split(new char[] { ',' }));
           if (nizIdsProf.Count == 1 && nizIdsProf.First() == "")
               nizIdsProf.Clear();

            StudentskaEvidencijaEntities entiteti = new StudentskaEvidencijaEntities();
            Predmet p = entiteti.Predmets.Where(it => it.PredmetID == predmetId).FirstOrDefault();
            p.NazivPredmeta = nazivPredmeta;
            p.SmerID = smerId;
            p.Poeni = poeni;
            p.Profesors.Clear();

            foreach (string profesorIdStr in nizIdsProf)
            {
                int profesorId = Int32.Parse(profesorIdStr);
                Profesor prof = entiteti.Profesors.Where(it => it.ProfesorID == profesorId).FirstOrDefault();
                p.Profesors.Add(prof);
            }

            entiteti.SaveChanges();

            return RedirectToAction("PrikaziPredmete");
        }
    }
}
