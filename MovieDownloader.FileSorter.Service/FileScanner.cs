using System;
using System.ServiceProcess;
using MovieDownloader.FileSorter.Core;
using MovieDownloader.Models;


namespace MovieDownloader.FileSorter.Service
{
    public partial class FileScanner : ServiceBase
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IFileProber _fileProber;
        public FileScanner(IFileProber fileProber)
        {
            InitializeComponent();
            _fileProber = fileProber;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var timer = Settings.AppSettings.DeleteTimer;

                _fileProber.Listen();
                _fileProber.DeleteTorrents(milliseconds: timer); 
                _fileProber.DeleteExtraFiles(millseconds: timer); 
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
                NotificationModule.SendEmail("Application exception " + ex.Message);
            }
        }

        protected override void OnStop()
        {
            _fileProber.Dispose();
            NotificationModule.SendEmail("Application stopping");
        }
    }
}
