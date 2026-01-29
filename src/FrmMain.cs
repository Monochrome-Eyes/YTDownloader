using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YTDownloader {
    public partial class FrmMain : Form {
        public FrmMain() {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e) {
            string ffmpegBin = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "tools",
                "ffmpeg",
                "bin",
                "ffmpeg.exe"
             );

            string ytdlpBin = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "tools",
                "yt-dlp",
                "yt-dlp.exe"
            );
        }

        private async void downloadBtn_Click(object sender, EventArgs e) {
            string ytDlpPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "tools",
                "yt-dlp.exe"
            );
            string ffmpegPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "tools"
            );

            statusTextBox.Clear();

            if (string.IsNullOrEmpty(urlTxtBox.Text))
            {
                MessageBox.Show("Please enter valid url");
                return;
            }

            try
            {
                await RunYtDlpAsync(ytDlpPath, ffmpegPath, urlTxtBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateStatus(string message)
        {
            if (string.IsNullOrEmpty (message))
            {
                return;
            }

            statusTextBox.BeginInvoke(new Action(() =>
            {
                statusTextBox.AppendText(text: message + Environment.NewLine);
                statusTextBox.SelectionStart = statusTextBox.Text.Length;
                statusTextBox.ScrollToCaret();
            }));
        }

        private Task RunYtDlpAsync(string ytDlpPath, string ffmpegPath, string url)
        {
            return Task.Run(() =>
            {
                string musicFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

                var startInfo = new ProcessStartInfo()
                {
                    FileName = ytDlpPath,
                    Arguments =
                        "-x --audio-format mp3 " +
                        "--no-playlist " +
                        $"--ffmpeg-location {ffmpegPath} " +
                        $"-o {musicFilePath}\\%(title)s.%(ext)s {url}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process();
                process.StartInfo = startInfo;

                process.OutputDataReceived += (sender, events) =>
                {
                    if (!string.IsNullOrEmpty(events.Data))
                    {
                        UpdateStatus(events.Data);
                    }
                };

                process.ErrorDataReceived += (sender, events) =>
                {
                    if (!string.IsNullOrEmpty(events.Data))
                    {
                        UpdateStatus(events.Data);
                    }
                };

                BeginInvoke(new Action(() => {
                    statusLabel.Text = "Status: Downloading. . . ";
                }));

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                BeginInvoke(new Action(() =>
                {
                    statusLabel.Text = "Status: Done";
                }));

                process.Dispose();
            });
        }
    }
}
