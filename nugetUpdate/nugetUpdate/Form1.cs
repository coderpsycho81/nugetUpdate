using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace nugetUpdate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateNuGetPackage(textBox1.Text);
            MessageBox.Show("NuGet paketi başarıyla güncellendi.");
        }

        private void UpdateNuGetPackage(string packageName)
        {
            string nugetExePath = Path.Combine(Application.StartupPath, "nuget.exe");
            string projectFilePath = Path.Combine(Application.StartupPath, "packages.config"); 

            if (!System.IO.File.Exists(nugetExePath))
            {
                throw new FileNotFoundException("nuget.exe bulunamadı.", nugetExePath);
            }

            var processStartInfo = new ProcessStartInfo
            {
                FileName = nugetExePath,
                Arguments = $"update {projectFilePath} -Id {packageName} -Source https://api.nuget.org/v3/index.json",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processStartInfo))
            {
                if (process != null)
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    MessageBox.Show(output);

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"NuGet paketi güncellenirken bir hata oluştu. Çıkış kodu: {process.ExitCode}\nHata: {error}");
                    }
                }
                else
                {
                    throw new Exception("NuGet.CommandLine aracı başlatılamadı.");
                }
            }
        }
    }
}
