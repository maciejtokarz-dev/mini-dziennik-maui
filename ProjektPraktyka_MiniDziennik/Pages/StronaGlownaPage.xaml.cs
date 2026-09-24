using Microsoft.Extensions.DependencyInjection;
using ProjektPraktyka_MiniDziennik.ViewModels;

namespace ProjektPraktyka_MiniDziennik.Pages;

public partial class StronaGlownaPage : ContentPage
{
    private readonly StronaGlownaViewModel _viewModel;

    public StronaGlownaPage()
    {
        InitializeComponent();

        var uslugi = Application.Current?.Handler?.MauiContext?.Services;

        if (uslugi == null)
            throw new InvalidOperationException(
                "Nie udało się pobrać usług aplikacji.");

        _viewModel = uslugi.GetRequiredService<StronaGlownaViewModel>();

        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.Odswiez();
    }
}