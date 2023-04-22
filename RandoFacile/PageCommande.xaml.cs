namespace RandoFacile;


public partial class PageCommande : ContentPage
{

    private Commande laCommande;
	public PageCommande(Commande laCommande)
	{
		InitializeComponent();
        this.laCommande = laCommande;
        this.Title = "Commande N°" + this.laCommande.Id;
        monLayout.BindingContext = this.laCommande;

        lblMontantTotal.Text = "Total : " + this.laCommande.GetMontantTotalTTC().ToString() + " € TTC";

    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {

        try
        {

            //Details commande

            List<LigneCommande>detailCommande = new List<LigneCommande>();

            foreach (Produit key in laCommande.detailsCommande.Keys)
            {

                detailCommande.Add(new LigneCommande(key, laCommande.detailsCommande[key]));

            }

            colViewProduits.ItemsSource = detailCommande;


            //Récupère les statuts en BDD.
            StatutPicker.ItemsSource = await Contexte.GetLesStatuts();

            StatutPicker.ItemsSource = StatutPicker.GetItemsAsList();


            //Affectation du statut sélectionné par défaut : Le plus récent

            Dictionary<Statut, DateTime> lesStatutsCommande = laCommande.detailsStatuts; // Les statuts de la commande
            Statut leStatutPlusRecent = lesStatutsCommande.Keys.FirstOrDefault(); //Par défaut le premier statut

            //Trouve le statut le plus récent
            foreach (KeyValuePair<Statut, DateTime> unStatut in lesStatutsCommande)
            {
       
                //Si le statut est plus récent, il devient le statut le plus récent
                if(unStatut.Value > lesStatutsCommande[leStatutPlusRecent])
                {
                    leStatutPlusRecent = unStatut.Key;
                }
                
            }

            //Index du statut séléctionné par défaut = au dernier statut de la commande
            StatutPicker.SelectedIndex = StatutPicker.ItemsSource.IndexOf(leStatutPlusRecent);


        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", ex.Message, "OK");
        }
    }



    private async void btModifier_Clicked(object sender, EventArgs e)
    {

        Statut leStatut = StatutPicker.SelectedItem as Statut;


        if (leStatut == null)
        {
            await DisplayAlert("Erreur", "Vous devez saisir un statut.", "OK");

        }
        else
        {

            try
            {

                bool setStatut = await Contexte.SetStatut(this.laCommande, leStatut);

                if (setStatut)
                {
                    await DisplayAlert("Information", "Statut modifié !", "OK");
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", ex.Message, "OK");
            }


        }

    }

}