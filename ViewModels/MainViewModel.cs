using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using YtDlpWPF.Models;
using YtDlpWPF.Services;

namespace YtDlpWPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly YtDlpService _service = new();
    private CancellationTokenSource? _cts;

    [ObservableProperty] private string _url          = string.Empty;
    [ObservableProperty] private string _outputFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
    [ObservableProperty] private bool   _audioOnly    = false;
    [ObservableProperty] private double _progress     = 0;
    [ObservableProperty] private string _statusMessage= "Ready";
    [ObservableProperty] private bool   _isDownloading= false;
    [ObservableProperty] private string _logText      = string.Empty;

    public ObservableCollection<DownloadItem> History { get; } = new();

    public MainViewModel()
    {
        _service.OnProgressLine    += line  => System.Windows.Application.Current.Dispatcher.Invoke(() => LogText += line + "\n");
        _service.OnProgressPercent += pct   => System.Windows.Application.Current.Dispatcher.Invoke(() => Progress = pct);
        _service.OnTitleFound      += title => System.Windows.Application.Current.Dispatcher.Invoke(() => StatusMessage = $"Downloading: {title}");
    }

    [RelayCommand]
    private async Task DownloadAsync()
    {
        if (string.IsNullOrWhiteSpace(Url))       { StatusMessage = "Please paste a URL first.";          return; }
        if (!System.IO.Directory.Exists(OutputFolder)) { StatusMessage = "Output folder does not exist."; return; }

        IsDownloading = true;
        Progress      = 0;
        LogText       = string.Empty;
        StatusMessage = "Starting download…";

        var item = new DownloadItem
        {
            Url = Url, Format = AudioOnly ? "MP3" : "MP4",
            OutputFolder = OutputFolder, Status = "Downloading"
        };
        History.Insert(0, item);

        _cts = new CancellationTokenSource();
        var (success, error) = await _service.DownloadAsync(Url, OutputFolder, AudioOnly, _cts.Token);

        if (success)
        {
            Progress = 100; StatusMessage = "Done! ✓"; item.Status = "Done";
        }
        else if (_cts.IsCancellationRequested)
        {
            StatusMessage = "Cancelled."; item.Status = "Cancelled";
        }
        else
        {
            StatusMessage = "Error — see log."; item.Status = "Error";
            LogText += "ERROR: " + error + "\n";
        }

        IsDownloading = false;
    }

    [RelayCommand]
    private void Cancel() => _cts?.Cancel();
}
