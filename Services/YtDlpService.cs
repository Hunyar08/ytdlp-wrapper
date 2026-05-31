using System.Diagnostics;
using System.Text.RegularExpressions;

namespace YtDlpWPF.Services;

public class YtDlpService
{
    private const string YtDlpExe = "yt-dlp.exe";

    public event Action<string>? OnProgressLine;
    public event Action<double>? OnProgressPercent;
    public event Action<string>? OnTitleFound;

    public async Task<(bool Success, string Error)> DownloadAsync(
        string url,
        string outputFolder,
        bool audioOnly,
        CancellationToken ct = default)
    {
        var args = audioOnly
            ? $"--newline -x --audio-format mp3 -o \"{outputFolder}\\%(title)s.%(ext)s\" \"{url}\""
            : $"--newline -f \"bestvideo[ext=mp4]+bestaudio[ext=m4a]/best[ext=mp4]/best\" -o \"{outputFolder}\\%(title)s.%(ext)s\" \"{url}\"";

        var psi = new ProcessStartInfo
        {
            FileName = YtDlpExe,
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError  = true,
            UseShellExecute  = false,
            CreateNoWindow   = true,
        };

        try
        {
            using var process = new Process { StartInfo = psi };
            var errorLines = new List<string>();

            process.OutputDataReceived += (_, e) =>
            {
                if (e.Data is null) return;
                OnProgressLine?.Invoke(e.Data);
                ParseProgressLine(e.Data);
            };
            process.ErrorDataReceived += (_, e) =>
            {
                if (e.Data is null) return;
                errorLines.Add(e.Data);
                OnProgressLine?.Invoke("[ERR] " + e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync(ct);

            return process.ExitCode == 0
                ? (true, string.Empty)
                : (false, string.Join('\n', errorLines));
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    private static readonly Regex ProgressRegex =
        new(@"\[download\]\s+([\d\.]+)%", RegexOptions.Compiled);

    private void ParseProgressLine(string line)
    {
        var m = ProgressRegex.Match(line);
        if (m.Success && double.TryParse(m.Groups[1].Value, out double pct))
            OnProgressPercent?.Invoke(pct);

        if (line.Contains("[info]") && line.Contains("Downloading"))
        {
            var title = line.Replace("[info]", "").Split(':')[0].Trim();
            if (!string.IsNullOrEmpty(title))
                OnTitleFound?.Invoke(title);
        }
    }
}
