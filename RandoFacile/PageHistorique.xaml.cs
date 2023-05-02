namespace RandoFacile;

public partial class PageHistorique : ContentPage
{
	private Commande laCommande;
	public PageHistorique(Commande laCommande)
	{
        this.laCommande = laCommande;
        InitializeComponent();
        


    }



    private async void ContentPage_Loaded(object sender, EventArgs e)
    {

        try
        {

            //Details status
            List<DetailStatut> detailStatuts = new List<DetailStatut>();

            foreach (Statut key in laCommande.detailsStatuts.Keys)
            {

                detailStatuts.Add(new DetailStatut(key, laCommande.detailsStatuts[key]));

            }
            
            test.BindingContext = detailStatuts[0];
            colViewStatut.ItemsSource = detailStatuts;



        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", ex.Message, "OK");
        }
    }
}