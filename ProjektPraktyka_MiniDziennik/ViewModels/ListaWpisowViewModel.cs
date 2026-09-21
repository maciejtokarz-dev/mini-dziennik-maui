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

        // Komenda otwierania wykresu statystyk wagi
        OtworzWykresCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(Pages.StatystykiWagiPage));
        });

        OdswiezCommand = new Command(Odswiez);

        WybierzWpisCommand = new Command<WpisWidoku>(async (wpis) =>
        {
            if (wpis == null)
                return;

            await Shell.Current.GoToAsync(
                $"{nameof(Pages.SzczegolyWpisuPage)}?id={wpis.Id}");
        });         
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
            OnPropertyChanged(nameof(CzyWaga)); 
            OnPropertyChanged(nameof(Wpisy));
            OnPropertyChanged(nameof(CzyBrakWpisow));
        }
    }

    public string NazwaKategorii => Kategoria == "WAGA"
        ? "WAGA"
        : "NOTATKI";

    public bool CzyWaga => Kategoria == "WAGA";

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

    public ICommand OtworzWykresCommand { get; } // <- Nowa komenda dla przycisku wykresu

    public ICommand OdswiezCommand { get; }

    public ICommand WybierzWpisCommand { get; }

    // =========================
    // ODŚWIEŻANIE
    // =========================

    public async Task OdswiezAsync()
    {
        TypWpisu typ = Kategoria == "WAGA"
            ? TypWpisu.Waga
            : TypWpisu.Notatka;

        var wszystkieWpisy = await _wpisService.PobierzWszystkieAsync();

        var przefiltrowane = wszystkieWpisy
            .Where(x => x.Typ == typ)
            .OrderByDescending(x => x.DataUtworzenia);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Wpisy.Clear();
            foreach (var wpis in przefiltrowane)
            {
                Wpisy.Add(new WpisWidoku(wpis));
            }
            OnPropertyChanged(nameof(CzyBrakWpisow));
        });
    }

    public void Odswiez()
    {
        _ = OdswiezAsync();
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