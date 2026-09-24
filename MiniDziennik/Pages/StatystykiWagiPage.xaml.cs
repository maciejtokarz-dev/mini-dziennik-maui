using MiniDziennik.ViewModels;

namespace MiniDziennik.Pages;

public partial class StatystykiWagiPage : ContentPage
{
    private readonly StatystykiWagiViewModel _viewModel;

    public StatystykiWagiPage()
    {
        InitializeComponent();

        var uslugi = Application.Current?.Handler?.MauiContext?.Services;
        if (uslugi == null)
            throw new InvalidOperationException("Nie udało się pobrać usług aplikacji.");

        _viewModel = uslugi.GetRequiredService<StatystykiWagiViewModel>();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.ZaladujDaneAsync();
    }
}