using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Counter;
public partial class MainPage : ContentPage
{
    public ObservableCollection<CounterItem> Counters { get; set; } = new();

    private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "counters.json");

    public MainPage()
    {
        InitializeComponent();

        // Przypisanie źródła danych raz na starcie
        CountersCollection.ItemsSource = Counters;
        LoadCounters();
    }

    private void OnAddCounterClicked(object sender, EventArgs e)
    {
        string name = CounterNameEntry.Text?.Trim() ?? string.Empty;

        // walidacja
        if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, @"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s]+$"))
        {
            DisplayAlert("Błąd", "Nazwa licznika może zawierać wyłącznie litery i spacje!", "OK");
            return; // Przerywamy, jeśli nazwa jest niepoprawna
        }

        string initialText = InitialValueEntry.Text?.Trim() ?? string.Empty;
        int initialVal = 0;

        if (!string.IsNullOrEmpty(initialText))
        {
            
            if (!Regex.IsMatch(initialText, @"^-?\d+$"))
            {
                DisplayAlert("Błąd", "Wartość początkowa może zawierać wyłącznie cyfry!", "OK");
                return; 
            }

            int.TryParse(initialText, out initialVal);
        }

        
        var newCounter = new CounterItem
        {
            Name = name,
            InitialValue = initialVal,
            Value = initialVal
        };

        Counters.Add(newCounter);

        // Czyszczenie pól formularza
        CounterNameEntry.Text = string.Empty;
        InitialValueEntry.Text = string.Empty;

        SaveCounters();
    }

    private void OnIncrementClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is CounterItem item)
        {
            item.Value++; 
            SaveCounters();
        }
    }

    private void OnDecrementClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is CounterItem item)
        {
            item.Value--; 
            SaveCounters();
        }
    }

    private void SaveCounters()
    {
        try
        {
            string json = JsonSerializer.Serialize(Counters);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Błąd zapisu: {ex.Message}");
        }
    }

    private void LoadCounters()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                var loadedCounters = JsonSerializer.Deserialize<List<CounterItem>>(json);

                if (loadedCounters != null)
                {
                    Counters.Clear();
                    foreach (var counter in loadedCounters)
                    {
                        Counters.Add(counter);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Błąd wczytywania: {ex.Message}");
        }
    }
}