

namespace RandoFacile;

public partial class PageMenu : ContentPage
{
    //public PageMenu(Utilisateur userAuthentifie)

    public PageMenu(Utilisateur userAuthentifie)
	{
		InitializeComponent();
        Application.Current.Resources["userAuthentifie"] = userAuthentifie;

    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        
    }

    private void btProduits_Clicked(object sender, EventArgs e)
    {
        //Permet d'acc�der � la page pass�e en param�tre
        Navigation.PushAsync(new PageStock());
        

    }


    private void btProduitsACommander_Clicked(object sender, EventArgs e)
    {
        //Permet d'acc�der � la page pass�e en param�tre
        Navigation.PushAsync(new PageACommander());


    }


    private void btCommandes_Clicked(object sender, EventArgs e)
    {

        //Commande laCommande = await Contexte.GetCommande("16");
        ////Permet d'acc�der � la page pass�e en param�tre
        //Navigation.PushAsync(new PageCommande(laCommande));

        //Permet d'acc�der � la page pass�e en param�tre
        Navigation.PushAsync(new PageTrouverCommande());
    }
}