using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ProjektPraktyka_MiniDziennik.Models;
using ProjektPraktyka_MiniDziennik.Services;

namespace ProjektPraktyka_MiniDziennik.ViewModels;

public class ListaWpisowViewModel : INotifyPropertyChanged
{
    private readonly WpisService _wpisService;

    private string _kategoria = "WAGA";

    public ListaWpisowViewModel(WpisService wpisService)
    {
        _wpisService = wpisService;

        WsteczCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync("..");
        });

        DodajWpisCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(Pages.DodajEdytujWpisPage));
        });

        OdswiezCommand = new Command(Odswiez);
    }

    // =========================
    // KATEGORIA
    // =========================

    public string Kategoria
    {
        get => _kategoria;
        set
        {
            if (_kategoria == value)
                return;

            _kategoria = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(NazwaKategorii));
            OnPropertyChanged(nameof(Wpisy));
            OnPropertyChanged(nameof(CzyBrakWpisow));
        }
    }

    public string NazwaKategorii => Kategoria == "WAGA"
        ? "WAGA"
        : "NOTATKI";

    // =========================
    // WPISY
    // =========================

    public ObservableCollection<WpisWidoku> Wpisy { get; } = new();

    public bool CzyBrakWpisow => Wpisy.Count == 0;

    // =========================
    // COMMANDY
    // =========================

    public ICommand WsteczCommand { get; }

    public ICommand DodajWpisCommand { get; }

    public ICommand OdswiezCommand { get; }

    // =========================
    // ODŚWIEŻANIE
    // =========================

    public void Odswiez()
    {
        Wpisy.Clear();

        TypWpisu typ = Kategoria == "WAGA"
            ? TypWpisu.Waga
            : TypWpisu.Notatka;

        var wpisy = _wpisService
            .PobierzWszystkie()
            .Where(x => x.Typ == typ)
            .OrderByDescending(x => x.DataUtworzenia);

        foreach (var wpis in wpisy)
        {
            Wpisy.Add(new WpisWidoku(wpis));
        }

        OnPropertyChanged(nameof(CzyBrakWpisow));
    }

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

public class WpisWidoku
{
    public int Id { get; }

    public string Wartosc { get; }

    public string DataWpisu { get; }

    public WpisWidoku(Wpis wpis)
    {
        Id = wpis.Id;

        if (wpis.Typ == TypWpisu.Waga)
        {
            Wartosc = $"{wpis.WartoscWagi:0.0} kg";
        }
        else
        {
            Wartosc = wpis.TrescNotatki ?? string.Empty;
        }

        DataWpisu = wpis.DataUtworzenia.ToString("dd.MM.yyyy, HH:mm");
    }
}