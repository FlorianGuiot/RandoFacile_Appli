using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RandoFacile
{
    public class Utilisateur
    {

        [JsonPropertyName("iduser")]
        public string Id { get; set; }

        [JsonPropertyName("nom")]
        public string Nom { get; set; }

        [JsonPropertyName("prenom")]
        public string Prenom { get; set; }

        [JsonPropertyName("mail")]
        public string Email { get; set; }

        [JsonPropertyName("mdp")]
        public string Password { get; set; }



    }
}
