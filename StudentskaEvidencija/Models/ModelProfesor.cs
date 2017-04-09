using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentskaEvidencija.Models
{
    public class ModelProfesor
    {
        public string id;
        public string ime;
        public string godZaposlenja;

        public ModelProfesor(string id, string ime, string godZaposlenja)
        {
            this.id = id;
            this.ime = ime;
            this.godZaposlenja = godZaposlenja;
        }

    }
}