using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Counter;
public class CounterItem : INotifyPropertyChanged
{
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    private int _value = 0;
    public int Value
    {
        get => _value;
        set { _value = value; OnPropertyChanged(); }
    }

    public int InitialValue { get; set; } = 0;

    // Automatyczne odświeżanie widoku po zmianie właściwości
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}