using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace RandoFacile
{
    /// <summary>
    /// Permet d'accéder aux informations de la base de données via un web service.
    /// </summary>
    public class Contexte
    {
        public static string urlServiceWeb = "http://localhost/ServiceWebRandoFacile"; //Windows : localhost Android: 10.0.2.2

        public static HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Obtient les informations de l'utilisateur correspondant au nom d'utilisateur et au mot de passe
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<Utilisateur> GetUserAuthentification(string email, string password)
        {

            Utilisateur leUser = null;

            string urlAPI = Contexte.urlServiceWeb + "/connexion/";

            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(email), name: "mail");
            form.Add(new StringContent(password), name: "mdp");

            HttpResponseMessage reponse = await Contexte.httpClient.PostAsync(new Uri(urlAPI), form);
            if (reponse.IsSuccessStatusCode)
            {
                string resultat = await reponse.Content.ReadAsStringAsync();
                if (resultat.Contains("false") == false) //Si le résultat ne contient pas la chaine False : il existe un utilisateur
                {
                    JsonSerializerOptions optionJson = new JsonSerializerOptions();
                    optionJson.PropertyNameCaseInsensitive = true;
                    leUser = JsonSerializer.Deserialize<Utilisateur>(resultat, optionJson);
                }
                //Sinon le login et le mot de passe n'existe pas, alors leUser reste null
            }
            else
            {
                throw new Exception("Problème de connexion. Merci de réessayer ultérieurement."); //Lève une exception si la connexion n'a pas aboutie
            }
            return leUser;
        }





        /// <summary>
        /// Obtient la liste des categories
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        /// <returns>Collection des Categories retournés par le WebService</returns>
        public static async Task<List<Categorie>> GetCategories()
        {
            string urlAPI = urlServiceWeb + "/categories/";


            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                List<Categorie> lesCategories = JsonSerializer.Deserialize<List<Categorie>>(contenu, optionJson);

                return lesCategories;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
            }
        }

        /// <summary>
        /// Obtient tous les produits
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        /// <returns>Collection des produits retournés par le WebService</returns>
        public static async Task<List<Produit>> GetProduits(string idCategorie, bool asc, bool aCommander)
        {
            string urlAPI = urlServiceWeb + "/produits/0/20/"+idCategorie+"/";

            //Ajoute le filtre croissant ou décroissant
            if (asc)
            {
                urlAPI += "1/";
            }
            else
            {
                urlAPI += "0/";
            }

            //Ajoute le filtre a commander
            if (aCommander)
            {
                urlAPI += "1/";
            }
            else
            {
                urlAPI += "0/";
            }


            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                List<Produit> lesProduits = JsonSerializer.Deserialize<List<Produit>>(contenu, optionJson);

                //Affecte à chaque produit la categorie correspondante
                List<Categorie> lesCateg = await Contexte.GetCategories();
                foreach (Produit unP in lesProduits)
                {
                    unP.laCateg = (from uneC in lesCateg
                                   where uneC.CodeCateg == unP.CodeCateg
                                   select uneC).FirstOrDefault();
                }
                return lesProduits;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
            }
        }



        /// <summary>
        /// Obtient le produit correspondant à l'id en paramètre.
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        /// <returns>Produit retournés par le WebService</returns>
        public static async Task<Produit> GetProduit(string idProduit)
        {
            string urlAPI = urlServiceWeb + "/produit/" + idProduit + "/";


            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                Produit leProduit = JsonSerializer.Deserialize<Produit>(contenu, optionJson);

                //Affecte à chaque produit la categorie correspondante
                List<Categorie> lesCateg = await Contexte.GetCategories();
                leProduit.laCateg = (from uneC in lesCateg
                                      where uneC.CodeCateg == leProduit.CodeCateg
                                      select uneC).FirstOrDefault();
                return leProduit;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
            }
        }



        /// <summary>
        /// Modification du seuil d'alerte d'un produit
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        public static async Task<bool> SetSeuilAlerte(Produit unProduit, string seuil)
        {

            string urlAPI = urlServiceWeb + "/produit/seuil/";

            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(unProduit.Id), name: "idProduit");
            form.Add(new StringContent(seuil), name: "seuil");

            HttpResponseMessage reponse = await Contexte.httpClient.PostAsync(new Uri(urlAPI), form);
            if (reponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(reponse.StatusCode.ToString());
            }
        }




        /// <summary>
        /// Modification du statut actuel d'une commande
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        public static async Task<bool> SetStatut(Commande laCommande, Statut leStatut)
        {

            string urlAPI = urlServiceWeb + "/commande/statut/";

            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(laCommande.Id), name: "idCommande");
            form.Add(new StringContent(leStatut.Id), name: "idStatut");

            HttpResponseMessage reponse = await Contexte.httpClient.PostAsync(new Uri(urlAPI), form);
            if (reponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(reponse.StatusCode.ToString());
            }
        }




        /// <summary>
        /// Obtient la liste des pays
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        /// <returns>Collection des Pays retournés par le WebService</returns>
        public static async Task<List<Pays>> GetPays()
        {
            string urlAPI = urlServiceWeb + "/pays/";


            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                List<Pays> lesPays = JsonSerializer.Deserialize<List<Pays>>(contenu, optionJson);

                return lesPays;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
            }
        }


        /// <summary>
        /// Obtient la liste des statuts
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        /// <returns>Collection des statuts retournés par le WebService</returns>
        public static async Task<List<Statut>> GetLesStatuts()
        {
            string urlAPI = urlServiceWeb + "/statuts/";


            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                List<Statut> lesStatuts = JsonSerializer.Deserialize<List<Statut>>(contenu, optionJson);

                return lesStatuts;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
            }
        }


        /// <summary>
        /// Obtient un dictionnaire contenant les statuts et leurs date pour une commande.
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        /// <returns>Dictionnaire retournés par le WebService</returns>
        public static async Task<Dictionary<Statut, DateTime>> GetLesStatutsDetails(string idCommande)
        {
            string urlAPI = urlServiceWeb + "/statuts/" + idCommande + "/";


            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                //Dictionnaire contenant uniquement des string
                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                List<Dictionary<string, string>> lesStatutsString = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(contenu, optionJson);

                //Dictionnaire a retourner
                Dictionary<Statut, DateTime> lesStatutsDetails = new Dictionary<Statut, DateTime>();

                List<Statut> lesStatuts = await Contexte.GetLesStatuts();

                //Remplace chaque string par un objet statut et un DateTime
                foreach(Dictionary<string, string> unDico in lesStatutsString)
                {

                    foreach (string key in unDico.Keys)
                    {

                        //Affecte à chaque statutDetail le statut correspondant
                        Statut leStatut = (from unS in lesStatuts
                                           where unS.Id == key
                                           select unS).FirstOrDefault();


                        lesStatutsDetails.Add(leStatut, DateTime.Parse(unDico[key]));
                    }

                }
               

                return lesStatutsDetails;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
            }
        }




        /// <summary>
        /// Obtient un dictionnaire contenant le detail de la commande une commande.
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        /// <returns>Dictionnaire retournés par le WebService</returns>
        public static async Task<Dictionary<Produit, int>> GetDetailsCommande(string idCommande)
        {
            string urlAPI = urlServiceWeb + "/commande/detail/" + idCommande + "/";


            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                //Dictionnaire contenant uniquement des string
                List< Dictionary<string, string>>leDetailString = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(contenu);

                //Dictionnaire a retourner
                Dictionary<Produit, int> leDetail = new Dictionary<Produit, int>();

                

                //Remplace chaque string par un objet Produit et un int
                foreach (Dictionary<string, string> unDico in leDetailString)
                {

                    foreach (string key in unDico.Keys)
                    {

                        leDetail.Add(await GetProduit(key), int.Parse(unDico[key]));
                    }

                }

                return leDetail;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
            }
        }


        /// <summary>
        /// Obtient l'utilisateur correspondant à l'id en paramètre.
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        /// <returns>Utilisateur retournés par le WebService</returns>
        public static async Task<Utilisateur> GetUser(string idUser)
        {
            string urlAPI = urlServiceWeb + "/utilisateur/" + idUser + "/";


            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                Utilisateur leUser = JsonSerializer.Deserialize<Utilisateur>(contenu, optionJson);

                return leUser;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
            }
        }



        /// <summary>
        /// Obtient la commande correspondant à l'id en paramètre.
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        /// <returns>Commande retournés par le WebService</returns>
        public static async Task<Commande> GetCommande(string idCommande)
        {
            string urlAPI = urlServiceWeb + "/commande/" + idCommande + "/";


            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                Commande laCommande = JsonSerializer.Deserialize<Commande>(contenu, optionJson);

                //Affecte à la commande le pays correspondant
                List<Pays> lesPays = await Contexte.GetPays();
                laCommande.lePays = (from unP in lesPays
                                     where unP.Id == laCommande.IdPays
                                     select unP).FirstOrDefault();

                laCommande.detailsStatuts = await GetLesStatutsDetails(idCommande);

                laCommande.detailsCommande = await GetDetailsCommande(idCommande);

                laCommande.Utilisateur = await GetUser(laCommande.IdUser);

                return laCommande;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
            }
        }



        /// <summary>
        /// Retourne true si la commande existe.
        /// </summary>
        /// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        /// <returns>True ou false retournés par le WebService</returns>
        public static async Task<bool> CommandeExiste(string idCommande)
        {
            string urlAPI = urlServiceWeb + "/commande/existe/" + idCommande + "/";


            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {

                bool commandeExiste = false;

                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                int existe = JsonSerializer.Deserialize<int>(contenu);

                if(existe == 1)
                {
                    commandeExiste = true;
                }

                return commandeExiste;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
            }
        }



        ///// <summary>
        ///// Obtient les offres de covoiturages pour un evenement donné
        ///// </summary>
        ///// <param name="unEven">Evénement pour lequel on obtient la liste des coVoiturages</param>
        ///// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        ///// <returns>Collection des CoVoiturages retournés par le WebService</returns>
        //public static async void GetCoVoiturages(Evenement unEven)
        //{
        //    string urlAPI = urlServiceWeb + "/coVoiturages/" + unEven.Id + "/";

        //    HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
        //    if (resultatRequete.IsSuccessStatusCode)
        //    {
        //        string contenu = await resultatRequete.Content.ReadAsStringAsync();

        //        JsonSerializerOptions optionJson = new JsonSerializerOptions();
        //        optionJson.PropertyNameCaseInsensitive = true;
        //        List<CoVoiturage> lesCoV = JsonSerializer.Deserialize<List<CoVoiturage>>(contenu, optionJson);

        //        unEven.lesCov = lesCoV;

        //        //Pas obligatoire, car pas d'utilité... enfin pour l'instant !
        //        foreach (CoVoiturage coV in lesCoV)
        //        {
        //            coV.leEvenement = unEven;
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
        //    }
        //}

        ///// <summary>
        ///// Obtient les informations de l'utilisateur correspondant à l'identifiant
        ///// </summary>
        ///// <param name="idPers">Identifiant de l'utilisateur</param>
        ///// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        ///// <returns>Un objet Personne contenant les données de l'utilisateur</returns>
        //public static async Task<Personne> GetPersonne(string idPers)
        //{
        //    string urlAPI = urlServiceWeb + "/personne/" + idPers + "/";

        //    HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
        //    if (resultatRequete.IsSuccessStatusCode)
        //    {
        //        string contenu = await resultatRequete.Content.ReadAsStringAsync();

        //        JsonSerializerOptions optionJson = new JsonSerializerOptions();
        //        optionJson.PropertyNameCaseInsensitive = true;
        //        Personne laPersonne = JsonSerializer.Deserialize<Personne>(contenu, optionJson);

        //        return laPersonne;
        //    }
        //    else
        //    {
        //        throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
        //    }
        //}

        ///// <summary> Obtient la liste des événements (tous ou futurs) avec les covoiturages proposés pour chacun d'eux </summary>
        ///// <param name="futur">False : tous les événements, True : seulement ceux à venir</param>
        ///// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        ///// <returns>Collection des Evenements retournés par le WebService</returns>
        //public static async Task<List<Evenement>> GetEvenements(bool futur)
        //{
        //    string urlAPI = urlServiceWeb + "/evenements";
        //    if (futur)
        //    {
        //        urlAPI += "Futurs";
        //    }
        //    urlAPI += "/";

        //    HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
        //    if (resultatRequete.IsSuccessStatusCode)
        //    {
        //        string contenu = await resultatRequete.Content.ReadAsStringAsync();

        //        JsonSerializerOptions optionJson = new JsonSerializerOptions();
        //        optionJson.PropertyNameCaseInsensitive = true;
        //        List<Evenement> lesEvenements = JsonSerializer.Deserialize<List<Evenement>>(contenu, optionJson);

        //        foreach (Evenement unEv in lesEvenements)
        //        {
        //            GetCoVoiturages(unEv);
        //        }

        //        return lesEvenements;
        //    }
        //    else
        //    {
        //        throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
        //    }
        //}

        ///// <summary>
        ///// Obtient les offres de covoiturages d'un utilisateur donné
        ///// </summary>
        ///// <param name="idUser">Identifiant de l'utilisateur</param>
        ///// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        ///// <returns>Liste d'instances de covoiturages</returns>
        //public static async Task<List<CoVoiturage>> GetMesOffres(string idUser)
        //{
        //    string urlAPI = urlServiceWeb + "/mesCoVoiturages/" + idUser + "/";

        //    HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
        //    if (resultatRequete.IsSuccessStatusCode)
        //    {
        //        string contenu = await resultatRequete.Content.ReadAsStringAsync();
        //        JsonSerializerOptions optionJson = new JsonSerializerOptions();
        //        optionJson.PropertyNameCaseInsensitive = true;
        //        List<CoVoiturage> mesCoV = JsonSerializer.Deserialize<List<CoVoiturage>>(contenu, optionJson);

        //        //Affecte à chaque coVoiturage l'événement correspondant
        //        List<Evenement> lesEv = await Contexte.GetEvenements(false);
        //        foreach (CoVoiturage coV in mesCoV)
        //        {
        //            coV.leEvenement = (from unV in lesEv
        //                               where unV.Id == coV.IdEven
        //                               select unV).FirstOrDefault();
        //        }
        //        return mesCoV;
        //    }
        //    else
        //    {
        //        throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode.ToString());
        //    }
        //}

        ///// <summary>
        ///// Ajout d'un nouvel utilisateur de l'application
        ///// </summary>
        ///// <param name="unNouvelUser">Objet Personne contenant les données de l'utilisateur</param>
        ///// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        //public static async Task<bool> AddUser(Personne unNouvelUser)
        //{
        //    string modif = unNouvelUser.Nom[0].ToString().ToUpper() + unNouvelUser.Nom.Substring(1).ToLower();
        //    unNouvelUser.Nom = modif;
        //    modif = unNouvelUser.Prenom[0].ToString().ToUpper() + unNouvelUser.Prenom.Substring(1).ToLower();
        //    unNouvelUser.Prenom = modif;

        //    unNouvelUser.User = unNouvelUser.Nom.ToLower() + unNouvelUser.Prenom.ToUpper()[0].ToString();

        //    string urlAPI = urlServiceWeb + "/personne/insert/";

        //    MultipartFormDataContent form = new MultipartFormDataContent();
        //    form.Add(new StringContent(unNouvelUser.Nom), name: "nom");
        //    form.Add(new StringContent(unNouvelUser.Prenom), name: "prenom");
        //    form.Add(new StringContent(unNouvelUser.Tel), name: "tel");
        //    form.Add(new StringContent(unNouvelUser.Mail), name: "mail");
        //    form.Add(new StringContent(unNouvelUser.User), name: "user");
        //    form.Add(new StringContent(unNouvelUser.Mdp), name: "mdp");

        //    HttpResponseMessage reponse = await Contexte.httpClient.PostAsync(new Uri(urlAPI), form);
        //    if (reponse.IsSuccessStatusCode)
        //    {
        //        int nouvelID = int.Parse(await reponse.Content.ReadAsStringAsync()); //Parse en int car si autre chose qu'un int lève une exception, par ex un message d'erreur sur une contrainte non respectée
        //        unNouvelUser.Id = nouvelID.ToString();
        //        return true;
        //    }
        //    else
        //    {
        //        throw new Exception(reponse.StatusCode.ToString());
        //    }
        //}


        ///// <summary>
        ///// Ajout d'une nouvelle reservation
        ///// </summary>
        ///// <param name="unUser">Objet Personne contenant les données de l'utilisateur</param>
        ///// <param name="unCoVoiturage">Objet CoVoiturage contenant les données du covoiturage</param>
        ///// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        //public static async Task<bool> AddReservation(Personne unUser, CoVoiturage unCoVoiturage)
        //{
        //    string urlAPI = urlServiceWeb + "/reservation/insert/";

        //    MultipartFormDataContent form = new MultipartFormDataContent();
        //    form.Add(new StringContent(unUser.Id), name: "pers");
        //    form.Add(new StringContent(unCoVoiturage.Id), name: "cov");


        //    HttpResponseMessage reponse = await Contexte.httpClient.PostAsync(new Uri(urlAPI), form);
        //    if (reponse.IsSuccessStatusCode)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        throw new Exception(reponse.StatusCode.ToString());
        //    }
        //}


        ///// <summary>
        ///// Supprime le covoiturage passé en paramètre
        ///// </summary>
        ///// <param name="unCoVoiturage">Covoiturage à supprimer</param>
        ///// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        //public static async Task<bool> DeleteCoVoiturage(CoVoiturage unCoVoiturage)
        //{
        //    string urlAPI = urlServiceWeb + "/coVoiturage/delete/" + unCoVoiturage.Id + "/";

        //    HttpResponseMessage resultatRequete = await Contexte.httpClient.DeleteAsync(new Uri(urlAPI));
        //    if (resultatRequete.IsSuccessStatusCode)
        //    {
        //        string contenu = await resultatRequete.Content.ReadAsStringAsync();
        //        if (contenu[0] == '1')  //Cela signifie qu'une ligne a bien été supprimée !
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            throw new Exception("Erreur à la suppression de l'offre : " + contenu);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Erreur à la suppression de l'offre : " + resultatRequete.StatusCode.ToString());
        //    }
        //}

        ///// <summary>
        ///// Ajout d'un nouveau covoiturage
        ///// </summary>
        ///// <param name="nouveauCv">Objet Covoiturage contenant les informations à ajouter</param>
        ///// <exception cref="Exception">Retourne le message d'erreur du webservice</exception>
        //public static async Task<bool> AddCoVoiturage(CoVoiturage nouveauCv)
        //{
        //    string urlAPI = urlServiceWeb + "/coVoiturage/insert/";

        //    MultipartFormDataContent form = new MultipartFormDataContent();
        //    form.Add(new StringContent(nouveauCv.IdCond.ToString()), name: "conducteur");
        //    form.Add(new StringContent(nouveauCv.Heure), name: "heure");
        //    form.Add(new StringContent(nouveauCv.LieuDepart), name: "lieuDepart");
        //    form.Add(new StringContent(nouveauCv.Voiture), name: "voiture");
        //    form.Add(new StringContent(nouveauCv.IdEven.ToString()), name: "evenement");

        //    HttpResponseMessage reponse = await Contexte.httpClient.PostAsync(new Uri(urlAPI), form);
        //    if (reponse.IsSuccessStatusCode)
        //    {
        //        int nouvelID = int.Parse(await reponse.Content.ReadAsStringAsync()); //Parse en int car si autre chose qu'un int lève une exception, par ex un message d'erreur sur une contrainte non respectée
        //        nouveauCv.Id = nouvelID.ToString();
        //        return true;
        //    }
        //    else
        //    {
        //        throw new Exception(reponse.StatusCode.ToString());
        //    }
        //}
    }
}
