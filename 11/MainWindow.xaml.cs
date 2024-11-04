using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace sd1._11;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var Items = new ObservableCollection<ListItem>(
            Enumerable.Range(1, 10).Select(static i => new ListItem() { Text = $"Element #{i}" })
        );
        MainListBox.ItemsSource = Items;
    }

    private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (MainListBox.SelectedItem is ListItem selectedItem)
            selectedItem.FontStyle =
                selectedItem.FontStyle == FontStyles.Italic ? FontStyles.Normal : FontStyles.Italic;
    }
}

public class ListItem : INotifyPropertyChanged
{
    private FontStyle _fontStyle = FontStyles.Normal;

    public required string Text { get; set; }

    public FontStyle FontStyle
    {
        get => _fontStyle;
        set
        {
            if (_fontStyle != value)
            {
                _fontStyle = value;
                OnPropertyChanged(nameof(FontStyle));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
