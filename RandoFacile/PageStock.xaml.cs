using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
namespace RandoFacile;

public partial class PageStock : ContentPage
{

    bool croissant = true;
    string idCategorie = "0";
    public PageStock()
	{
        InitializeComponent();

    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        radioButton1.IsChecked = true;

        try
        {

            this.colViewProduits.ItemsSource = await Contexte.GetProduits(this.idCategorie, this.croissant, false);


            //Récupère les catégories en BDD.
            List<Categorie> lesCateg = await Contexte.GetCategories();

            //Ajoute la categorie "toutes" en première position de la liste.
            lesCateg.Insert(0,new Categorie("0", "Toutes"));

            categoriePicker.ItemsSource = lesCateg;

            categoriePicker.ItemsSource = categoriePicker.GetItemsAsList();

            categoriePicker.SelectedIndex = 0;


        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", ex.Message, "OK");
        }
    }



    //private void stockOrdre_CheckedChanged(object sender, SelectionChangedEventArgs e)
    //{


    //    //bool croissant = false;

    //    if (radioButton1.IsChecked)
    //    {
    //        lblErreur2.IsVisible = false;
    //    }

    //    //this.colViewProduits.ItemsSource = await Contexte.GetProduits("0", croissant);
    //}


    private async void stockOrdre_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        RadioButton radioButton = sender as RadioButton;

        if (radioButton != null && radioButton.IsChecked)
        {
            if (radioButton.Content.Equals("Croissant"))
            {
                
                this.croissant = true;

                //Si je ne met pas d'alerte le colview genere une erreur !!!
                await DisplayAlert("Information", "Croissant!", "OK");
            }
            else if (radioButton.Content.Equals("Décroissant"))
            {
                
                this.croissant = false;

                //Si je ne met pas d'alerte le colview genere une erreur !!!
                await DisplayAlert("Information", "Décroissant!", "OK");
            }

            colViewProduits.ItemsSource = await Contexte.GetProduits(this.idCategorie, this.croissant, false);
        }
    }

    private async void categoriePicker_SelectedIndexChanged(object sender, EventArgs e)
    {

        var selectedItem = categoriePicker.SelectedItem;


        this.idCategorie = ((Categorie)selectedItem).CodeCateg;
        colViewProduits.ItemsSource = await Contexte.GetProduits(this.idCategorie, croissant, false);

    }

    private async void colViewProduits_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Produit produitSelect = (Produit)e.CurrentSelection.FirstOrDefault();
        await Navigation.PushAsync(new PageProduit(await Contexte.GetProduit(produitSelect.Id)));
    }
}