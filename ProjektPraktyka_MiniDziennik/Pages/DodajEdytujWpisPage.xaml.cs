using Microsoft.Extensions.DependencyInjection;
using ProjektPraktyka_MiniDziennik.ViewModels;

namespace ProjektPraktyka_MiniDziennik.Pages;

public partial class DodajEdytujWpisPage : ContentPage
{
    public DodajEdytujWpisPage()
    {
        InitializeComponent();

        var uslugi = Application.Current?.Handler?.MauiContext?.Services;

        if (uslugi == null)
            throw new InvalidOperationException("Nie udało się pobrać usług aplikacji.");

        BindingContext = uslugi.GetRequiredService<DodajEdytujWpisViewModel>();
    }
}