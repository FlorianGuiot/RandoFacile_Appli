namespace RandoFacile;

public partial class PageConnexion : ContentPage
{
	public PageConnexion()
	{
		InitializeComponent();
	}

    private async void btConnexion_Clicked(object sender, EventArgs e)
    {
        try
        {
            Utilisateur unUser = await Contexte.GetUserAuthentification(txtEmailUser.Text, txtPassword.Text);

            if (unUser != null)
            {
                //Permet d'accéder à la page passée en paramètre
                await Navigation.PushAsync(new PageMenu(unUser));
                //Supprime la page courante de la navigation afin de ne pas pouvoir revenir dessus
                Navigation.RemovePage(this);
            }
            else
            {
                lblErreur.IsVisible = true;
                
            }
        }
        catch (ArgumentNullException)
        {
            await DisplayAlert("Erreur", "Vous devez saisir une adresse email ET un mot de passe", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", ex.Message, "OK");
        }
    }
}