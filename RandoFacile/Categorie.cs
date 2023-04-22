using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoFacile
{
    public class Categorie
    {
        public Categorie(string codecateg, string libelle)
        {
            CodeCateg = codecateg;
            Libelle = libelle;
        }

        public string CodeCateg { get; set; }
        public string Libelle { get; set; }
    }
}
