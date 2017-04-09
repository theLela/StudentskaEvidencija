using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentskaEvidencija.Models
{
    public class ModelSmer
    {
        public string smerid;
        public string nazivsmera;

        public ModelSmer(string smerid, string nazivsmera)
        {
            this.smerid = smerid;
            this.nazivsmera = nazivsmera;
        }
    }
}