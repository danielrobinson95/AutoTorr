using System;
using System.Collections.Generic;
using System.Linq;
using MovieDownloader.Models.YifyApi;
using Movie = MovieDownloader.Models.Movie;

namespace Yify.API
{
    public static class Validation
    {
        public static List<Movie> ValidateResponse(this Rootobject yifyData)
        {
            if (yifyData.status_message != "Query was successful")
                throw new Exception("Web Service Request Failed");
            if (yifyData.data.movie_count == 0)
                throw new Exception("Movie Not Found");


            return yifyData.data.movies.Select(movie => new Movie
            {
                Title = $"{movie.title} ({movie.year})",
                ImdbCode = movie.imdb_code,
                Torrents = movie.torrents
            }).ToList();
        }
    }
}
