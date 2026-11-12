using System.Collections.Generic;

namespace AvaloniaApp.Models;

/// <summary>
/// Odpowiednik KS2 AktualneZnakiChromatyczne + logiki z Rysowanie.RysujNute:
/// pamięta znak obowiązujący dla każdej nuty i wyznacza znak do narysowania (w tym kasownik).
/// Nazwy znaków jak w KS2: brak, bemol, krzyzyk, podwojny_bemol, podwojny_krzyzyk, kasownik.
/// </summary>
public sealed class Ks2AccidentalState
{
    private readonly Dictionary<int, string> _current = new();

    public string GetCurrent(int noteNumber) =>
        _current.TryGetValue(noteNumber, out var sign) ? sign : "brak";

    public string Resolve(int noteNumber, string ks2Sign)
    {
        var effective = Effective(ks2Sign, GetCurrent(noteNumber));
        _current[noteNumber] = ks2Sign;
        return effective;
    }

    public void Reset() => _current.Clear();

    public static string Effective(string ks2Sign, string currentSign)
    {
        switch (ks2Sign)
        {
            case "brak":
                return currentSign == "brak" ? "brak" : "kasownik";
            case "bemol":
            case "krzyzyk":
            case "podwojny_bemol":
            case "podwojny_krzyzyk":
                return ks2Sign == currentSign ? "brak" : ks2Sign;
            default:
                throw new System.ArgumentException("Nieznany znak chromatyczny", nameof(ks2Sign));
        }
    }
}
