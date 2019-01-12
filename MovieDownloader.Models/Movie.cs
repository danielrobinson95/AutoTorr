

using System.Collections.Generic;
using MovieDownloader.Models.YifyApi;

namespace MovieDownloader.Models
{
    public class Movie
    {
        public string Title { get; set; }
        public string ImdbCode { get; set; }
        public Torrent[] Torrents { get; set; }
    }
}
