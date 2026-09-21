using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ProjektPraktyka_MiniDziennik.Models;
using ProjektPraktyka_MiniDziennik.Services;
using SkiaSharp;

namespace ProjektPraktyka_MiniDziennik.ViewModels;

public class StatystykiWagiViewModel : INotifyPropertyChanged
{
    private readonly WpisService _wpisService;

    private int _wybranyZakresDni = 7; // Domyślnie 7 dni
    private string _minWaga = "0.0 kg";
    private string _sredniaWaga = "0.0 kg";
    private string _maxWaga = "0.0 kg";

    private ISeries[] _serieWykresu = Array.Empty<ISeries>();
    private Axis[] _osX = [new DateTimeAxis(TimeSpan.FromDays(1), data => data.ToString("dd.MM"))];
    private Axis[] _osY = [new Axis { Labeler = value => value.ToString("0.0") }];

    public StatystykiWagiViewModel(WpisService wpisService)
    {
        _wpisService = wpisService;

        WsteczCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync("..");
        });

        UstawZakresCommand = new Command<string>(async (dni) =>
        {
            if (int.TryParse(dni, out int iloscDni))
            {
                WybranyZakresDni = iloscDni;
                await ZaladujDaneAsync();
            }
        });
    }

    // =========================
    // WŁAŚCIWOŚCI
    // =========================

    public int WybranyZakresDni
    {
        get => _wybranyZakresDni;
        set
        {
            _wybranyZakresDni = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Is7Dni));
            OnPropertyChanged(nameof(Is30Dni));
        }
    }

    public bool Is7Dni => WybranyZakresDni == 7;
    public bool Is30Dni => WybranyZakresDni == 30;

    public string MinWaga
    {
        get => _minWaga;
        set { _minWaga = value; OnPropertyChanged(); }
    }

    public string SredniaWaga
    {
        get => _sredniaWaga;
        set { _sredniaWaga = value; OnPropertyChanged(); }
    }

    public string MaxWaga
    {
        get => _maxWaga;
        set { _maxWaga = value; OnPropertyChanged(); }
    }

    public ISeries[] SerieWykresu
    {
        get => _serieWykresu;
        set { _serieWykresu = value; OnPropertyChanged(); }
    }

    public Axis[] OsX
    {
        get => _osX;
        set { _osX = value; OnPropertyChanged(); }
    }

    public Axis[] OsY
    {
        get => _osY;
        set { _osY = value; OnPropertyChanged(); }
    }

    public ICommand WsteczCommand { get; }
    public ICommand UstawZakresCommand { get; }

    // =========================
    // ŁADOWANIE DANYCH I WYKRESU
    // =========================

    public async Task ZaladujDaneAsync()
    {
        var wszystkie = await _wpisService.PobierzWszystkieAsync();

        DateTime dataOd = DateTime.Now.Date.AddDays(-WybranyZakresDni);

        // Filtrowanie po typie waga, zakresie dni i sortowanie chronologiczne
        var wpisyWagi = wszystkie
            .Where(x => x.Typ == TypWpisu.Waga && x.WartoscWagi.HasValue && x.DataUtworzenia >= dataOd)
            .OrderBy(x => x.DataUtworzenia)
            .ToList();

        if (wpisyWagi.Count == 0)
        {
            SerieWykresu = Array.Empty<ISeries>();
            MinWaga = "0.0 kg";
            SredniaWaga = "0.0 kg";
            MaxWaga = "0.0 kg";
            return;
        }

        // 1. Statystyki pod wykresem
        double min = wpisyWagi.Min(w => w.WartoscWagi!.Value);
        double max = wpisyWagi.Max(w => w.WartoscWagi!.Value);
        double srednia = wpisyWagi.Average(w => w.WartoscWagi!.Value);

        MinWaga = $"{min:0.0} kg";
        SredniaWaga = $"{srednia:0.0} kg";
        MaxWaga = $"{max:0.0} kg";

        // 2. Punkty wykresu (prawidłowe rzutowanie wartości Wagi)
        var punkty = wpisyWagi
            .Select(w => new DateTimePoint(w.DataUtworzenia, w.WartoscWagi!.Value))
            .ToArray();

        SerieWykresu =
        [
            new LineSeries<DateTimePoint>
            {
                Values = punkty,
                Fill = null,
                Stroke = new SolidColorPaint(new SKColor(240, 122, 94)) { StrokeThickness = 3 },
                GeometryFill = new SolidColorPaint(new SKColor(240, 122, 94)),
                GeometryStroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 },
                GeometrySize = 9
            }
        ];

        // 3. Dynamiczne dopasowanie skali osi Y do danych (z buforem ±1kg)
        OsY =
        [
            new Axis
            {
                MinLimit = Math.Max(0, Math.Floor(min - 1)),
                MaxLimit = Math.Ceiling(max + 1),
                MinStep = 0.5,
                Labeler = value => value.ToString("0.0")
            }
        ];

        // 4. Dopasowanie kroku osi X zależnie od podglądu (7 vs 30 dni)
        OsX =
        [
            new DateTimeAxis(
                WybranyZakresDni == 7 ? TimeSpan.FromDays(1) : TimeSpan.FromDays(5),
                data => data.ToString("dd.MM"))
        ];
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}