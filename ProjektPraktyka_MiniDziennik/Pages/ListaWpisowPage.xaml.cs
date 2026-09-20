using Microsoft.Extensions.DependencyInjection;
using ProjektPraktyka_MiniDziennik.ViewModels;

namespace ProjektPraktyka_MiniDziennik.Pages;

[QueryProperty(nameof(Kategoria), "kategoria")]
public partial class ListaWpisowPage : ContentPage
{
    private readonly ListaWpisowViewModel _viewModel;

    public string Kategoria
    {
        set
        {
            _viewModel.Kategoria = value;
        }
    }

    public ListaWpisowPage()
    {
        InitializeComponent();

        var uslugi = Application.Current?.Handler?.MauiContext?.Services;

        if (uslugi == null)
            throw new InvalidOperationException(
                "Nie udało się pobrać usług aplikacji.");

        _viewModel =
            uslugi.GetRequiredService<ListaWpisowViewModel>();

        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.Odswiez();
    }
}