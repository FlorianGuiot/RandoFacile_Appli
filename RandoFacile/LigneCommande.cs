using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoFacile
{
    public class LigneCommande
    {
        public LigneCommande(Produit unP, int qte)
        {
            this.unP = unP;
            this.qte = qte;
        }

        public Produit unP { get; set; }
        public int qte { get; set; }
    }
}
