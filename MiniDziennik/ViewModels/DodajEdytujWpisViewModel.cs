using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MiniDziennik.Models;
using MiniDziennik.Services;

namespace MiniDziennik.ViewModels;

public class DodajEdytujWpisViewModel : INotifyPropertyChanged
{
    private readonly WpisService _wpisService;

    private Wpis? _edytowanyWpis;

    private bool _czyWaga = true;
    private string _wartoscWagi = string.Empty;
    private string _trescNotatki = string.Empty;
    private DateTime _dataWpisu = DateTime.Today;
    private TimeSpan _godzinaWpisu = DateTime.Now.TimeOfDay;
    private string _komunikatBledu = string.Empty;

    public DodajEdytujWpisViewModel(WpisService wpisService)
    {
        _wpisService = wpisService;

        WybierzWageCommand = new Command(() =>
        {
            CzyWaga = true;
            WyczyscBlad();
        });

        WybierzNotatkeCommand = new Command(() =>
        {
            CzyWaga = false;
            WyczyscBlad();
        });

        ZapiszCommand = new Command(async () => await ZapiszAsync());

        AnulujCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync("..");
        });

        WsteczCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync("..");
        });
    }

    public DodajEdytujWpisViewModel(
    WpisService wpisService,
    Wpis wpis) : this(wpisService)
    {
        _edytowanyWpis = wpis;

        WczytajDaneWpisu();
    }

    private void WczytajDaneWpisu()
    {
        if (_edytowanyWpis == null)
            return;

        CzyWaga = _edytowanyWpis.Typ == TypWpisu.Waga;

        if (CzyWaga)
        {
            WartoscWagi = _edytowanyWpis.WartoscWagi?
                .ToString("0.0", CultureInfo.InvariantCulture)
                ?? string.Empty;
        }
        else
        {
            TrescNotatki = _edytowanyWpis.TrescNotatki
                ?? string.Empty;
        }

        DataWpisu = _edytowanyWpis.DataUtworzenia.Date;
        GodzinaWpisu = _edytowanyWpis.DataUtworzenia.TimeOfDay;
    }

    // =========================
    // WYBÓR TYPU WPISU
    // =========================

    public bool CzyWaga
    {
        get => _czyWaga;
        set
        {
            if (_czyWaga == value)
                return;

            _czyWaga = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(CzyNotatka));
        }
    }

    public bool CzyNotatka => !CzyWaga;

    // =========================
    // WAGA
    // =========================

    public string WartoscWagi
    {
        get => _wartoscWagi;
        set
        {
            if (_wartoscWagi == value)
                return;

            _wartoscWagi = value;

            OnPropertyChanged();
        }
    }

    // =========================
    // NOTATKA
    // =========================

    public string TrescNotatki
    {
        get => _trescNotatki;
        set
        {
            if (_trescNotatki == value)
                return;

            _trescNotatki = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(LiczbaZnakow));
        }
    }

    public string LiczbaZnakow => $"{TrescNotatki.Length}/500";

    // =========================
    // DATA I GODZINA
    // =========================

    public DateTime DataWpisu
    {
        get => _dataWpisu;
        set
        {
            if (_dataWpisu == value)
                return;

            _dataWpisu = value;

            OnPropertyChanged();
        }
    }

    public TimeSpan GodzinaWpisu
    {
        get => _godzinaWpisu;
        set
        {
            if (_godzinaWpisu == value)
                return;

            _godzinaWpisu = value;

            OnPropertyChanged();
        }
    }

    // =========================
    // BŁĘDY
    // =========================

    public string KomunikatBledu
    {
        get => _komunikatBledu;
        set
        {
            if (_komunikatBledu == value)
                return;

            _komunikatBledu = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(CzyJestBlad));
        }
    }

    public bool CzyJestBlad => !string.IsNullOrWhiteSpace(KomunikatBledu);

    // =========================
    // COMMANDY
    // =========================

    public ICommand WybierzWageCommand { get; }

    public ICommand WybierzNotatkeCommand { get; }

    public ICommand ZapiszCommand { get; }

    public ICommand AnulujCommand { get; }

    public ICommand WsteczCommand { get; }

    // =========================
    // ZAPIS
    // =========================

    private async Task ZapiszAsync()
    {
        WyczyscBlad();

        if (CzyWaga)
        {
            await ZapiszWageAsync();
        }
        else
        {
            await ZapiszNotatkeAsync();
        }

        if (CzyJestBlad)
            return;

        await Shell.Current.GoToAsync("..");
    }

    private async Task ZapiszWageAsync()
    {
        if (string.IsNullOrWhiteSpace(WartoscWagi))
        {
            KomunikatBledu = "Podaj wartość wagi.";
            return;
        }

        string tekstWagi = WartoscWagi
            .Trim()
            .Replace(',', '.');

        bool poprawnaWaga = double.TryParse(
            tekstWagi,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double waga);

        if (!poprawnaWaga)
        {
            KomunikatBledu = "Podaj poprawną wartość wagi.";
            return;
        }

        if (waga < 20 || waga > 300)
        {
            KomunikatBledu = "Waga musi znajdować się w zakresie 20–300 kg.";
            return;
        }

        if (_edytowanyWpis == null)
        {
            await _wpisService.DodajAsync(new Wpis
            {
                Typ = TypWpisu.Waga,
                WartoscWagi = waga,
                DataUtworzenia = PolaczDateIGodzine()
            });
        }
        else
        {
            _edytowanyWpis.Typ = TypWpisu.Waga;
            _edytowanyWpis.WartoscWagi = waga;
            _edytowanyWpis.TrescNotatki = null;
            _edytowanyWpis.DataUtworzenia = PolaczDateIGodzine();

            await _wpisService.AktualizujAsync(_edytowanyWpis);
        }
    }

    private async Task ZapiszNotatkeAsync()
    {
        if (string.IsNullOrWhiteSpace(TrescNotatki))
        {
            KomunikatBledu = "Wpisz treść notatki.";
            return;
        }

        if (TrescNotatki.Length > 500)
        {
            KomunikatBledu = "Notatka może mieć maksymalnie 500 znaków.";
            return;
        }

        if (_edytowanyWpis == null)
        {
            await _wpisService.DodajAsync(new Wpis
            {
                Typ = TypWpisu.Notatka,
                TrescNotatki = TrescNotatki.Trim(),
                DataUtworzenia = PolaczDateIGodzine()
            });
        }
        else
        {
            _edytowanyWpis.Typ = TypWpisu.Notatka;
            _edytowanyWpis.WartoscWagi = null;
            _edytowanyWpis.TrescNotatki = TrescNotatki.Trim();
            _edytowanyWpis.DataUtworzenia = PolaczDateIGodzine();

            await _wpisService.AktualizujAsync(_edytowanyWpis);
        }
    }

    private DateTime PolaczDateIGodzine()
    {
        return DataWpisu.Date.Add(GodzinaWpisu);
    }

    private void WyczyscBlad()
    {
        KomunikatBledu = string.Empty;
    }

    // =========================
    // INotifyPropertyChanged
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