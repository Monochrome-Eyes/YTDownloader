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
        }

        private async void downloadBtn_Click(object sender, EventArgs e) {
            string url = urlTxtBox.Text;
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Please enter a URL.");
            }

            try
            {
                string musicFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                string ytDlp = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "tools",
                    "yt-dlp.exe"
                 );
                string ffmpegPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "tools"
                 );

                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.FileName = ytDlp;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.Arguments = "-x --audio-format mp3 --no-playlist " + $"--ffmpeg-location {ffmpegPath} " + $"-o {musicFilePath}\\%(title)s.%(ext)s {url}";

                await Task.Run(() =>
                {
                    Process.Start(startInfo);
                });
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
