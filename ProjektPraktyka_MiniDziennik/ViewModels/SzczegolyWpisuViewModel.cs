using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ProjektPraktyka_MiniDziennik.Models;
using ProjektPraktyka_MiniDziennik.Services;

namespace ProjektPraktyka_MiniDziennik.ViewModels;

public class SzczegolyWpisuViewModel : INotifyPropertyChanged
{
    private readonly WpisService _wpisService;

    private Wpis? _wpis;

    public SzczegolyWpisuViewModel(WpisService wpisService, int id)
    {
        _wpisService = wpisService;

        _wpis = _wpisService.PobierzPoId(id);

        WsteczCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync("..");
        });

        EdytujCommand = new Command(async () =>
        {
            if (_wpis == null)
                return;

            await Shell.Current.GoToAsync(
                $"{nameof(Pages.DodajEdytujWpisPage)}?id={_wpis.Id}");
        });

        UsunCommand = new Command(async () =>
        {
            if (_wpis == null)
                return;

            bool potwierdzenie = await Application.Current!.Windows[0].Page!
                .DisplayAlert(
                    "Usuń wpis",
                    "Czy na pewno chcesz usunąć ten wpis?",
                    "Usuń",
                    "Anuluj");

            if (!potwierdzenie)
                return;

            _wpisService.Usun(_wpis.Id);

            await Shell.Current.Navigation.PopToRootAsync();
        });
    }

    // =========================
    // DANE WPISU
    // =========================

    public string TypWp
    {
        get
        {
            if (_wpis == null)
                return string.Empty;

            return _wpis.Typ == TypWpisu.Waga
                ? "WAGA"
                : "NOTATKA";
        }
    }

    public string Tytul
    {
        get
        {
            if (_wpis == null)
                return string.Empty;

            return _wpis.Typ == TypWpisu.Waga
                ? "Twój pomiar"
                : "Twoja notatka";
        }
    }

    public string Wartosc
    {
        get
        {
            if (_wpis == null)
                return string.Empty;

            if (_wpis.Typ == TypWpisu.Waga)
                return $"{_wpis.WartoscWagi:0.0} kg";

            return _wpis.TrescNotatki ?? string.Empty;
        }
    }

    public string DataWpisu
    {
        get
        {
            if (_wpis == null)
                return string.Empty;

            return _wpis.DataUtworzenia.ToString(
                "d MMMM yyyy, HH:mm",
                new System.Globalization.CultureInfo("pl-PL"));
        }
    }

    public bool CzyWaga =>
        _wpis?.Typ == TypWpisu.Waga;

    public bool CzyNotatka =>
        _wpis?.Typ == TypWpisu.Notatka;

    public int Id =>
        _wpis?.Id ?? 0;

    // =========================
    // COMMANDY
    // =========================

    public ICommand WsteczCommand { get; }
    public ICommand EdytujCommand { get; }
    public ICommand UsunCommand { get; }

    // =========================
    // POWIADOMIENIA
    // =========================

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? nazwa = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(nazwa));
    }
}
