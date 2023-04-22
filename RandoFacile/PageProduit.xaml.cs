using Microsoft.Maui.ApplicationModel.Communication;

namespace RandoFacile;

public partial class PageProduit : ContentPage
{
    Produit leProduit;
	public PageProduit(Produit leProduit)
    {
		InitializeComponent();
        this.leProduit = leProduit;
        this.Title = this.leProduit.Libelle;
        monLayout.BindingContext = this.leProduit;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {

        
    }

    private async void btnAlerte_Clicked(object sender, EventArgs e)
    {

        string seuil = entryAlerte.Text;

        if(string.IsNullOrWhiteSpace(seuil))
        {
            await DisplayAlert("Erreur", "Vous devez saisir un seuil d'alerte.", "OK");

        }else if(seuil == this.leProduit.AlerteStock)
        {

            try
            {

                bool setSeuil = await Contexte.SetSeuilAlerte(this.leProduit, seuil);

                if(setSeuil)
                {
                    await DisplayAlert("Information", "Seuil modifié !", "OK");
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", ex.Message, "OK");
            }


        }

    }

}