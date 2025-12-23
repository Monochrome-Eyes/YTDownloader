using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
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
            string urlText = urlTxtBox.Text;
            if (string.IsNullOrEmpty(urlText)) {
                MessageBox.Show("Please enter a URL.");
            }

            try {
                string musicFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                Console.WriteLine(musicFilePath);
                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.FileName = "C:\\Users\\Heatec\\AppData\\Local\\Programs\\yt-dlp\\yt-dlp.exe";
                //startInfo.UseShellExecute = false;
                startInfo.Arguments = $"-x --audio-format mp3 --no-playlist -o {musicFilePath}\\%(title)s.%(ext)s {urlText}";

                await Task.Run(() => {
                    Process.Start(startInfo);
                });
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
