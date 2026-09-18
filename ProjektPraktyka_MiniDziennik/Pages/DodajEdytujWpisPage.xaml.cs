namespace ProjektPraktyka_MiniDziennik.Pages;

public partial class DodajEdytujWpisPage : ContentPage
{
    public DodajEdytujWpisPage()
    {
        InitializeComponent();
    }

    private void Waga_Clicked(object sender, EventArgs e)
    {
        FormularzWagi.IsVisible = true;
        FormularzNotatki.IsVisible = false;
    }

    private void Notatka_Clicked(object sender, EventArgs e)
    {
        FormularzWagi.IsVisible = false;
        FormularzNotatki.IsVisible = true;
    }

    private async void OnWstecz_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}