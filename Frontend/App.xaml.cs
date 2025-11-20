using System.Configuration;
using System.Data;
using System.Windows;
using System.IO;
using System.Diagnostics;

namespace ROGraph
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            LoadEnvVariables();

            base.OnStartup(e);
        }

        public static void LoadEnvVariables()
        {
            string filePath = "./.env";

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(".env file not present");
            }

            foreach (string line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                {
                    continue;
                }

                string[] parts = line.Split('=', 2);
                if(parts.Length != 2)
                {
                    continue;
                }

                string key = parts[0].Trim();
                string value = parts[1].Trim();

                Debug.WriteLine($"Setting env var '{key}' to '{value}'");
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }

}
