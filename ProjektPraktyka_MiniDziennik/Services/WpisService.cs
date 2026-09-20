using ProjektPraktyka_MiniDziennik.Models;

namespace ProjektPraktyka_MiniDziennik.Services;

public class WpisService
{
    private readonly List<Wpis> _wpisy = new();

    private int _kolejneId = 1;

    public IReadOnlyList<Wpis> PobierzWszystkie()
    {
        return _wpisy
            .OrderByDescending(x => x.DataUtworzenia)
            .ToList();
    }

    public Wpis? PobierzPoId(int id)
    {
        return _wpisy.FirstOrDefault(x => x.Id == id);
    }

    public void Dodaj(Wpis wpis)
    {
        wpis.Id = _kolejneId++;
        _wpisy.Add(wpis);
    }

    public void Aktualizuj(Wpis wpis)
    {
        var istniejacyWpis = PobierzPoId(wpis.Id);

        if (istniejacyWpis == null)
            return;

        istniejacyWpis.Typ = wpis.Typ;
        istniejacyWpis.WartoscWagi = wpis.WartoscWagi;
        istniejacyWpis.TrescNotatki = wpis.TrescNotatki;
        istniejacyWpis.DataUtworzenia = wpis.DataUtworzenia;
    }

    public void Usun(int id)
    {
        var wpis = PobierzPoId(id);

        if (wpis != null)
        {
            _wpisy.Remove(wpis);
        }
    }
}
