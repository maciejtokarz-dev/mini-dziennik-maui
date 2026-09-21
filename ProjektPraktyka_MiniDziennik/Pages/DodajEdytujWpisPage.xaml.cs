using Microsoft.Extensions.DependencyInjection;
using ProjektPraktyka_MiniDziennik.Services;
using ProjektPraktyka_MiniDziennik.ViewModels;

namespace ProjektPraktyka_MiniDziennik.Pages;

[QueryProperty(nameof(IdWp), "id")]
public partial class DodajEdytujWpisPage : ContentPage
{
    private readonly WpisService _wpisService;

    public int IdWp { get; set; }

    public DodajEdytujWpisPage()
    {
        InitializeComponent();

        var uslugi = Application.Current?.Handler?.MauiContext?.Services;

        if (uslugi == null)
            throw new InvalidOperationException(
                "Nie udało się pobrać usług aplikacji.");

        _wpisService = uslugi.GetRequiredService<WpisService>();

        BindingContext = uslugi.GetRequiredService<DodajEdytujWpisViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (IdWp <= 0)
            return;

        var wpis = await _wpisService.PobierzPoIdAsync(IdWp);

        if (wpis == null)
            return;

        BindingContext = new DodajEdytujWpisViewModel(
            _wpisService,
            wpis);
    }
}