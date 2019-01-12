using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MovieDownloader.Models;
using Timer = System.Timers.Timer;

namespace MovieDownloader.FileSorter.Core
{
    public class FileProber : IFileProber
    {
        private readonly Settings _settings;
        private FileSystemWatcher _completedDownloads;
        private FileSystemWatcher _torrentFiles;
        private readonly List<string> _torrentFilesToBeDeleted;
        private Timer _deleteTorrentTimer;
        private Timer _completedDownloadsTimer;

        // Constructor initialization
        public FileProber(Settings settings)
        {
            _settings = settings;
            _torrentFilesToBeDeleted = new List<string>();
            _deleteTorrentTimer      = new Timer();
            _completedDownloadsTimer = new Timer();
        }

        /// <summary>
        /// Attach Listeners to File System
        /// </summary>
        public void Listen()
        {
            _completedDownloads = new FileSystemWatcher
            {
                Path = _settings.CompletedPath,
                EnableRaisingEvents = true,
                IncludeSubdirectories = true
            }; 

            _torrentFiles = new FileSystemWatcher
            {
                Path = _settings.TorrentsPath,
                EnableRaisingEvents = true
            };

            _completedDownloads.Created += HandleDownloadCompleteFile;
            _torrentFiles.Created += HandleNewTorrentFile;
        }

        /// <summary>
        /// When a new Download is complete this method
        /// will be invoked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleDownloadCompleteFile(object sender, FileSystemEventArgs e)
        {
            var attributes = File.GetAttributes(e.FullPath);
            var file = e.Name.EndsWith(".mp4") ? new FileInfo(e.FullPath) : null;

            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var directory = new DirectoryInfo(e.FullPath);
                file = directory.GetFiles("*.mp4").FirstOrDefault();
            }

            if (file == null)
                return;

            Explorer.Move(from: file.FullName, to: Path.Combine(_settings.PlexPath, file.Name));
            NotificationModule.SendEmail($"New Movie Available - {file.Name}");

            ResetDownloadCompleteDeleteTimer();
        }

        /// <summary>
        /// When a new torrent is discovered this method 
        /// will be invoked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HandleNewTorrentFile(object sender, FileSystemEventArgs e)
        {
            using (var robot = new UIPath(_settings))
            {
                await robot.Authenticate();
                await robot.GetReleaseKey();
                await robot.GetRobot();
                await robot.LaunchRobot();
            }

            AddFileToDeletionQueue(e.FullPath);
            ResetNewTorrentDeleteTimer();

            await Task.Run(() =>
            {
                Thread.Sleep(10000);
                Explorer.Kill();
            });

        }
        
        /// <summary>
        /// Creates the delete torrent timer
        /// </summary>
        /// <param name="milliseconds">Amount in milliseconds for timer</param>
        public void DeleteTorrents(int milliseconds)
        {
            _deleteTorrentTimer = new Timer
            {
                AutoReset = true,
                Enabled = true,
                Interval = milliseconds
            };

            _deleteTorrentTimer.Elapsed += DeleteTorrentFileTimerOnElapsed;
        }

        public void DeleteExtraFiles(int milliseconds)
        {
            _completedDownloadsTimer = new Timer
            {
                AutoReset = true,
                Enabled = true,
                Interval = milliseconds
            };

            _completedDownloadsTimer.Elapsed += DeleteExtraFileTimerOnElapsed;
        }

        /// <summary>
        /// Performs a delete on the entire Completed Path
        /// if an mp4 is currently being written too, the file is locked
        /// and won't be included in this delete submission
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void DeleteExtraFileTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var directory = new DirectoryInfo(_settings.CompletedPath);

            // Delete all files
            var files = Directory.GetFiles(directory.FullName, "*.*", SearchOption.AllDirectories).ToList();
            files.ForEach(file =>
            {
                var fileInfo = new FileInfo(file);
                fileInfo.Attributes &= ~FileAttributes.ReadOnly;
                fileInfo.Delete();
            });

            // Delete all sub directories
            var dirs = directory.GetDirectories().ToList();
            dirs.ForEach(dir => dir.Delete());

            ResetDownloadCompleteDeleteTimer();
        }

        /// <summary>
        /// Will begin deleting the array of files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void DeleteTorrentFileTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _torrentFilesToBeDeleted.ForEach(File.Delete);
            ResetNewTorrentDeleteTimer();
        }
        
        /// <summary>
        /// Teardown of timer resources
        /// </summary>
        public void Dispose()
        {
            _completedDownloads.Dispose();
            _torrentFiles.Dispose();
        }


        /// <summary>
        /// Adds a file to the deletion queue, when timer elapses
        /// deletion will take place for array
        /// </summary>
        /// <param name="path">Path to file required for deletion</param>
        private void AddFileToDeletionQueue(string path) => _torrentFilesToBeDeleted.Add(path);

        /// <summary>
        /// Resets new torrent delete timer back to it's original state
        /// </summary>
        private void ResetNewTorrentDeleteTimer()
        {
            _deleteTorrentTimer.Stop();
            _deleteTorrentTimer.Start();
        }

        /// <summary>
        /// Resets download complete timer back to it's original state
        /// </summary>
        private void ResetDownloadCompleteDeleteTimer()
        {
            _completedDownloadsTimer.Stop();
            _completedDownloadsTimer.Start();
        }
    }
}
