namespace RandoFacile;

public partial class PageTrouverCommande : ContentPage
{
	public PageTrouverCommande()
	{
		InitializeComponent();
	}


    private async void btFindCommande_Clicked(object sender, EventArgs e)
    {

        

        string idCommande = entryNCommande.Text;

        if (string.IsNullOrWhiteSpace(idCommande))
        {
            await DisplayAlert("Erreur", "Vous devez saisir un numéro de commande.", "OK");

        }
        else
        {

            try
            {

                bool existe = await Contexte.CommandeExiste(idCommande);

                if (existe)
                {
                    Commande laCommande = await Contexte.GetCommande(idCommande);
                    //Permet d'accéder à la page passée en paramètre
                    await Navigation.PushAsync(new PageCommande(laCommande));
                }
                else
                {

                    await DisplayAlert("Erreur", "Commande introuvable.", "OK");

                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", ex.Message, "OK");
            }


        }

    }
}

