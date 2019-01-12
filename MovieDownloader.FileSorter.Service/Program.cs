using System.ServiceProcess;
using MovieDownloader.FileSorter.Core;
using MovieDownloader.Models;

namespace MovieDownloader.FileSorter.Service
{
    internal static class Program
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            try
            {
                IFileProber prober = new FileProber(Settings.AppSettings);

                var servicesToRun = new ServiceBase[]
                {
                    new FileScanner(prober)
                };
                ServiceBase.Run(servicesToRun);
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }
    }
}
