using System.ComponentModel;
using System.Runtime.CompilerServices;
using ProjektPraktyka_MiniDziennik.Models;
using ProjektPraktyka_MiniDziennik.Services;

namespace ProjektPraktyka_MiniDziennik.ViewModels;

public class StronaGlownaViewModel : INotifyPropertyChanged
{
    private readonly WpisService _wpisService;

    private string _ostatniaWaga = "Brak wpisu";
    private string _dataOstatniejWagi = string.Empty;

    private string _ostatniaNotatka = "Brak wpisu";
    private string _dataOstatniejNotatki = string.Empty;

    public StronaGlownaViewModel(WpisService wpisService)
    {
        _wpisService = wpisService;

        Odswiez();
    }

    public string OstatniaWaga
    {
        get => _ostatniaWaga;
        private set
        {
            if (_ostatniaWaga == value)
                return;

            _ostatniaWaga = value;
            OnPropertyChanged();
        }
    }

    public string DataOstatniejWagi
    {
        get => _dataOstatniejWagi;
        private set
        {
            if (_dataOstatniejWagi == value)
                return;

            _dataOstatniejWagi = value;
            OnPropertyChanged();
        }
    }

    public string OstatniaNotatka
    {
        get => _ostatniaNotatka;
        private set
        {
            if (_ostatniaNotatka == value)
                return;

            _ostatniaNotatka = value;
            OnPropertyChanged();
        }
    }

    public string DataOstatniejNotatki
    {
        get => _dataOstatniejNotatki;
        private set
        {
            if (_dataOstatniejNotatki == value)
                return;

            _dataOstatniejNotatki = value;
            OnPropertyChanged();
        }
    }

    public async void Odswiez()
    {
        var wpisy = await _wpisService.PobierzWszystkieAsync();

        var ostatniaWaga = wpisy
            .FirstOrDefault(x => x.Typ == TypWpisu.Waga);

        var ostatniaNotatka = wpisy
            .FirstOrDefault(x => x.Typ == TypWpisu.Notatka);

        // OSTATNIA WAGA
        if (ostatniaWaga == null)
        {
            OstatniaWaga = "Brak wpisu";
            DataOstatniejWagi = string.Empty;
        }
        else
        {
            OstatniaWaga = $"{ostatniaWaga.WartoscWagi:0.0} kg";
            DataOstatniejWagi = FormatujDate(ostatniaWaga.DataUtworzenia);
        }

        // OSTATNIA NOTATKA
        if (ostatniaNotatka == null)
        {
            OstatniaNotatka = "Brak wpisu";
            DataOstatniejNotatki = string.Empty;
        }
        else
        {
            OstatniaNotatka = ostatniaNotatka.TrescNotatki ?? string.Empty;
            DataOstatniejNotatki = FormatujDate(ostatniaNotatka.DataUtworzenia);
        }
    }

    private string FormatujDate(DateTime data)
    {
        return data.ToString(
            "dd.MM.yyyy, HH:mm",
            new System.Globalization.CultureInfo("pl-PL"));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? nazwa = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(nazwa));
    }
}