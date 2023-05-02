using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoFacile
{

    public class DetailStatut
    {
        public DetailStatut(Statut unS, DateTime date)
        {
            this.unS = unS;
            this.date = date;
        }

        public Statut unS { get; set; }
        public DateTime date { get; set; }
    }
}
