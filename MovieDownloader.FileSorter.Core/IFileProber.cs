using System;


namespace MovieDownloader.FileSorter.Core
{
    public interface IFileProber : IDisposable
    {
        void Listen();
        void DeleteTorrents(int milliseconds);
        void DeleteExtraFiles(int millseconds);
    }
}
