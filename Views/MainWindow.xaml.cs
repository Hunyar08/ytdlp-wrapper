using System.Windows;
using YtDlpWPF.ViewModels;

namespace YtDlpWPF.Views;

public partial class MainWindow : Window
{
    private MainViewModel VM => (MainViewModel)DataContext;

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        // Load last saved path if present
        try
        {
            var p = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YtDlpWPF", "lastpath.txt");
            if (System.IO.File.Exists(p))
            {
                var last = System.IO.File.ReadAllText(p).Trim();
                if (!string.IsNullOrWhiteSpace(last) && System.IO.Directory.Exists(last))
                    VM.OutputFolder = last;
            }
        }
        catch { /* ignore */ }
    }

    private void PasteButton_Click(object sender, RoutedEventArgs e)
    {
        if (System.Windows.Clipboard.ContainsText())
            VM.Url = System.Windows.Clipboard.GetText().Trim();
    }

    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        using var dialog = new System.Windows.Forms.FolderBrowserDialog
        {
            Description = "Choose where to save downloads",
            SelectedPath = VM.OutputFolder,
            UseDescriptionForTitle = true
        };

        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            VM.OutputFolder = dialog.SelectedPath;
            SaveLastPath(dialog.SelectedPath);
        }
    }

    private void OutputFolderTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var path = OutputFolderTextBox.Text?.Trim();
        if (!string.IsNullOrWhiteSpace(path) && System.IO.Directory.Exists(path))
            SaveLastPath(path);
    }

    private void SaveLastPath(string path)
    {
        try
        {
            var dir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YtDlpWPF");
            System.IO.Directory.CreateDirectory(dir);
            var file = System.IO.Path.Combine(dir, "lastpath.txt");
            System.IO.File.WriteAllText(file, path);
        }
        catch { /* ignore */ }
    }
}
