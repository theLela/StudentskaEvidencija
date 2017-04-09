using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentskaEvidencija.Models
{
    public class ModelStudent
    {
        public int id;
        public string ime;
        public string indeks;
        public string smer;
        public string finansiranje;
        public string slika;

        public ModelStudent(int id, string ime, string indeks, string finansiranje, string smer, string slika)
        {
            this.id = id;
            this.ime = ime;
            this.indeks = indeks;
            this.finansiranje = finansiranje;
            this.smer = smer;
            this.slika = slika;
        }
    }
}