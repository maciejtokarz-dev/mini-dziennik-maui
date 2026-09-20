namespace ProjektPraktyka_MiniDziennik.Models;

public enum TypWpisu
{
    Waga,
    Notatka
}

public class Wpis
{
    public int Id { get; set; }

    public TypWpisu Typ { get; set; }

    public double? WartoscWagi { get; set; }

    public string? TrescNotatki { get; set; }

    public DateTime DataUtworzenia { get; set; }
}
