using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentskaEvidencija.Models
{
    public class ModelPredmetProfesori
    {
        public ModelPredmet predmet;
        public List<ModelProfesor> profesori;

        public ModelPredmetProfesori(ModelPredmet predmet)
        {
            this.predmet = predmet;
            this.profesori = new List<ModelProfesor>();
        }

        public void dodajProfesora(ModelProfesor profesor)
        {
            profesori.Add(profesor);
        }
    }
}