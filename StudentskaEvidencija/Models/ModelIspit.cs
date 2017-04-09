using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentskaEvidencija.Models
{
    public class ModelIspit
    {
        public string studentid;
        public string predmetid;
        public string predmet;
        public string datum;
        public string profesorime;
        public string profesorid;
        public string ocena;

        public ModelIspit(string studentid, string predmetid, string predmet, string datum, string profesorime, string ocena, string profesorid)
        {
            this.studentid = studentid;
            this.predmetid = predmetid;
            this.predmet = predmet;
            this.datum = datum;
            this.profesorime = profesorime;
            this.ocena = ocena;
            this.profesorid = profesorid;
        }
    }
}