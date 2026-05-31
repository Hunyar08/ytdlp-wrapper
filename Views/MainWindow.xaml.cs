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
            VM.OutputFolder = dialog.SelectedPath;
    }
}
