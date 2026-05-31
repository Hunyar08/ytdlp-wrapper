# yt-dlp Wrapper (WPF) — Setup Guide

A clean dark-themed WPF desktop app wrapping yt-dlp.
Runs as a plain .exe — no special runtime installation needed.

---

## What You Need

### 1. Visual Studio 2022 (Community — free)
https://visualstudio.microsoft.com/vs/community/
Workload to check: ✅ .NET Desktop Development

### 2. yt-dlp.exe
https://github.com/yt-dlp/yt-dlp/releases/latest
Drop yt-dlp.exe in the same folder as your built/published .exe

### 3. FFmpeg (for MP3 + video merging)
winget install ffmpeg
Or download from https://ffmpeg.org/download.html

---

## Opening the Project

1. Open Visual Studio 2022
2. File → Open → Project/Solution
3. Select YtDlpWPF.csproj

---

## Publishing a Standalone .exe

1. Right-click project → Publish
2. Target: Folder
3. Settings:
   - Configuration: Release
   - Target Runtime: win-x64
   - Deployment mode: Self-contained
   - ✅ Produce single file
4. Click Publish

The .exe will be in bin\Release\net8.0-windows\publish\
Copy yt-dlp.exe into that same folder and you're done!

---

## Why WPF instead of WinUI 3?

WPF produces a plain .exe that runs on any Windows 10/11 PC
without needing extra runtimes installed. WinUI 3 requires the
Windows App Runtime to be installed separately, which caused issues.
