using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;

namespace SearchApp;

public partial class MainWindow : Window
{
    private readonly FileSearcher _fileSearcher = new();

    public ObservableCollection<SearchResult> Results { get; } = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void OnBrowseClick(object sender, RoutedEventArgs e)
    {
        using var dialog = new Forms.FolderBrowserDialog();
        if (dialog.ShowDialog() == Forms.DialogResult.OK)
        {
            FolderTextBox.Text = dialog.SelectedPath;
        }
    }

    private async void OnSearchClick(object sender, RoutedEventArgs e)
    {
        var folder = FolderTextBox.Text.Trim();
        var query = QueryTextBox.Text.Trim();

        Results.Clear();

        var results = await Task.Run(() => _fileSearcher.Search(folder, query));
        foreach (var result in results)
        {
            Results.Add(result);
        }
    }

    private void OnResultDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is not DataGrid grid || grid.SelectedItem is not SearchResult result)
        {
            return;
        }

        if (!string.Equals(Path.GetExtension(result.FullPath), ".exe", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        Process.Start(new ProcessStartInfo(result.FullPath)
        {
            UseShellExecute = true
        });
    }
}
