using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RandoFacile
{
    public class Commande
    {
        public const float TVA = 0.20F;

        public const float FRAIS_LIVRAISON = 10F;

        public const float MONTANT_FRAIS_LIVRAISON_OFFERT = 50F;

        public string Id { get; set; }
        public string Adresse { get; set; }
        public string Ville { get; set; }
        public string Cp { get; set; }
        public string IdPays { get; set; }

        public Pays lePays { get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string IdUser { get; set; }

        public Utilisateur Utilisateur { get; set; }

        public Dictionary<Produit, int> detailsCommande = new Dictionary<Produit, int>();

        public Dictionary<Statut, DateTime> detailsStatuts = new Dictionary<Statut, DateTime>();

        public float GetMontantTotalTTC()
        {
            float montant = 0;

            //Calcul le montant total : (Prix + (Prix * TVA)) * qte
            foreach (Produit key in detailsCommande.Keys)
            {

                montant += (float.Parse(key.PrixVenteUHT) + (float.Parse(key.PrixVenteUHT) * TVA)) * detailsCommande[key];
            }

            //Ajoute les frais de livraison
            if (montant <= MONTANT_FRAIS_LIVRAISON_OFFERT)
            {
                montant += FRAIS_LIVRAISON;
            }


            //Ajoute les frais de livraison pays hors france
            montant += float.Parse(lePays.Frais);

            return montant;
        }
    }
}
