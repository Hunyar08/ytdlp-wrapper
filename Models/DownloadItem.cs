namespace YtDlpWPF.Models;

public class DownloadItem
{
    public string Url         { get; set; } = string.Empty;
    public string Title       { get; set; } = string.Empty;
    public string Format      { get; set; } = string.Empty;
    public string OutputFolder{ get; set; } = string.Empty;
    public string Status      { get; set; } = "Queued";
    public DateTime StartedAt { get; set; } = DateTime.Now;

    public string Summary     => $"[{Format}] {(string.IsNullOrEmpty(Title) ? Url : Title)}";
    public string StatusLine  => $"{Status}  •  {StartedAt:HH:mm:ss}";
}
