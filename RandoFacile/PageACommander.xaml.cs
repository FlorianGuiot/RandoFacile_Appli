namespace RandoFacile;

public partial class PageACommander : ContentPage
{
	public PageACommander()
	{
		InitializeComponent();
        
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        
        try
        {
            

            this.colViewProduitsACommander.ItemsSource = await Contexte.GetProduits("0", true, true);



        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", ex.Message, "OK");
        }
    }

    private async void colViewProduitsACommander_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Produit produitSelect = (Produit)e.CurrentSelection.FirstOrDefault();
        await Navigation.PushAsync(new PageProduit(await Contexte.GetProduit(produitSelect.Id)));
    }
}