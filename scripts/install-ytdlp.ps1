$dl = "..\tools\yt-dlp\yt-dlp.exe"
$url = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe"

Invoke-WebRequest -Uri $url -OutFile $dl
Unblock-File $dl
