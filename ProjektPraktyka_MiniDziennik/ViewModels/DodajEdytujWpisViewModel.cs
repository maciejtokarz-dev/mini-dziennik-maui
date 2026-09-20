using System.ComponentModel;
using System.Runtime.CompilerServices;
using ProjektPraktyka_MiniDziennik.Models;
using ProjektPraktyka_MiniDziennik.Services;

namespace ProjektPraktyka_MiniDziennik.ViewModels;

public class DodajEdytujWpisViewModel : INotifyPropertyChanged
{
    private readonly WpisService _wpisService;

    private bool _czyWaga = true;
    private double? _wartoscWagi;
    private string _trescNotatki = string.Empty;
    private DateTime _dataWpisu = DateTime.Now;

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

    public double? WartoscWagi
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

    public int LiczbaZnakow => TrescNotatki.Length;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? nazwa = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(nazwa));
    }
}