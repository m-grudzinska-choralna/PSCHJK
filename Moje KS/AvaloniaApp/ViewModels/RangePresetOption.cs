using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaApp.ViewModels;

public partial class RangePresetOption : ObservableObject
{
    public RangePresetOption(string name, string trebleFrom, string trebleTo, string bassFrom, string bassTo)
    {
        Name = name;
        TrebleFrom = trebleFrom;
        TrebleTo = trebleTo;
        BassFrom = bassFrom;
        BassTo = bassTo;
        _label = name;
    }

    public RangePresetOption(string name)
    {
        Name = name;
        IsCustom = true;
        TrebleFrom = TrebleTo = BassFrom = BassTo = string.Empty;
        _label = name;
    }

    public string Name { get; }
    public bool IsCustom { get; }
    public string TrebleFrom { get; }
    public string TrebleTo { get; }
    public string BassFrom { get; }
    public string BassTo { get; }

    [ObservableProperty]
    private string _label;

    public void UpdateLabel(bool isTreble)
    {
        Label = Name;
    }
}
