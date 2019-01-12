using System.Collections.Generic;
using System.Threading.Tasks;
using MovieDownloader.Models;

namespace Yify.API
{
    public interface IYifyService
    {
        Task<List<Movie>> SearchMovie(string searchTerm, int resultsCount);
        Task<string> DownloadMovie(string imdbCode);
    }
}
