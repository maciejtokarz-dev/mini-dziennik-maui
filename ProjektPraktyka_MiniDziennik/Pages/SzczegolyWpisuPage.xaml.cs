using ProjektPraktyka_MiniDziennik.Services;
using ProjektPraktyka_MiniDziennik.ViewModels;

namespace ProjektPraktyka_MiniDziennik.Pages;

[QueryProperty(nameof(IdWp), "id")]
public partial class SzczegolyWpisuPage : ContentPage
{
    private readonly WpisService _wpisService;

    public int IdWp { get; set; }

    public SzczegolyWpisuPage(WpisService wpisService)
    {
        InitializeComponent();

        _wpisService = wpisService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BindingContext = new SzczegolyWpisuViewModel(
            _wpisService,
            IdWp);
    }
}