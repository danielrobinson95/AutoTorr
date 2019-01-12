using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieDownloader.Models
{
    public class SearchResultViewModel
    {
        private readonly Movie _movie;

        public SearchResultViewModel(Movie movie)
        {
            _movie = movie;
        }

        public string Title => _movie.Title;
        public string ImdbCode => _movie.ImdbCode;
    }
}