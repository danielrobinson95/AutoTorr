using System;
using SHDocVw;
using System.Diagnostics;
using System.IO;

namespace MovieDownloader.FileSorter.Core
{
    public static class Explorer
    {

        public static void Kill()
        {
            var shellWindows = new ShellWindows();

            foreach (InternetExplorer ie in shellWindows)
            {
                // This parses the name of the process
                var processType = Path.GetFileNameWithoutExtension(ie.FullName)?.ToLower();
                if (processType != null && processType.Equals("explorer"))
                {
                    ie.Quit();
                }
            }
        }

        public static void ExecuteCmd(string plexScriptPath)
        {
            if (!File.Exists(plexScriptPath)) 
                throw new FileNotFoundException("Unable to update Plex - Script not found");

            var processInfo = new ProcessStartInfo(plexScriptPath)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            var process = Process.Start(processInfo);
            if (process == null) return;
            process.WaitForExit();
            process.StandardOutput.ReadToEnd();
            process.StandardError.ReadToEnd();
            process.Close();
        }

        public static void Move(string from, string to)
        {
            if (File.Exists(to))
                to += $"_{Guid.NewGuid()}";

            File.Move(from, to);
        }
    }
}
