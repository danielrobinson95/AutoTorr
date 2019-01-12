using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MovieDownloader.Models.YifyApi;
using Movie = MovieDownloader.Models.Movie;

namespace Yify.API
{
    public class YifyService : IYifyService
    {
        private readonly Uri _yifyBaseUri;
        private readonly string _torrentOutputDir;
        private const string ServiceUrl = "list_movies.json?query_term={0}&quality=1080p&limit=";

        public YifyService(string url, string torrentPath)
        {
            _yifyBaseUri = new Uri(url);
            _torrentOutputDir = torrentPath;
        }

        /// <summary>
        /// Search Yify Torrent Site for a movie of your choosing
        /// </summary>
        /// <param name="searchTerm">Query term such as movie name, imdb code, etc.</param>
        /// <param name="resultsCount">The amount of results you'd like returned.</param>
        /// <returns></returns>
        public async Task<List<Movie>> SearchMovie(string searchTerm, int resultsCount)
        {
            using (var http = new HttpClient())
            {
                var uri      = new Uri(_yifyBaseUri, string.Format(ServiceUrl, searchTerm) + resultsCount);
                var response = await http.GetAsync(uri);

                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsAsync<Rootobject>();
                return content.ValidateResponse();
            }
        }

        /// <summary>
        /// Download a movie with a specific imdb code
        /// </summary>
        /// <param name="imdbCode"></param>
        /// <returns></returns>
        public async Task<string> DownloadMovie(string imdbCode)
        {
            var result = await SearchMovie(imdbCode, 1);
            var movie  = result?.FirstOrDefault();

            if (movie == null) throw new Exception("Movie not found!");
            var torrent = movie.Torrents.FirstOrDefault(x => x.quality == "1080p");
            if (torrent == null) throw new Exception("1080p Format not available");

            var fileDestination = Path.Combine(_torrentOutputDir, $"{imdbCode}.torrent");

            using (var http = new WebClient())
            {
                await http.DownloadFileTaskAsync(torrent.url, fileDestination);
            }

            return "File has begun downloading";
        }
    }
}
