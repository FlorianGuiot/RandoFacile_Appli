using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RandoFacile
{
    public class Produit
    {
        public string Id { get; set; }
        public string Libelle { get; set; }
        public string Resume { get; set; }
        public string Description { get; set; }
        public string CodeCateg { get; set; }

        public Categorie laCateg { get; set; }

        [JsonPropertyName("path_photo")]
        public string PathPhoto { get; set; }

        [JsonPropertyName("qte_stock")]
        public string QteStock { get; set; }

        [JsonPropertyName("alerteStock")]
        public string AlerteStock { get; set; }

        [JsonPropertyName("prix_vente_uht")]
        public string PrixVenteUHT { get; set; }

        [JsonPropertyName("dateajout")]
        public string DateAjout { get; set; }

    }
}
