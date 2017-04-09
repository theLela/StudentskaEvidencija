using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentskaEvidencija.Models
{
    public class ModelPredmet
    {
        public string predmetid;
        public string naziv;
        public string smer;
        public string poeni;

        public ModelPredmet(string predmetid, string naziv, string smer, string poeni)
        {
            this.predmetid = predmetid;
            this.naziv = naziv;
            this.smer = smer;
            this.poeni = poeni;
        }
    }
}