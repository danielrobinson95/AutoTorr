using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MovieDownloader.Models;
using Yify.API;

namespace MovieDownloader.Controllers
{
    public class MovieController : ApiController
    {
        private readonly IYifyService _service;

        public MovieController(IYifyService service)
        {
            _service = service;
        }

        [Route("api/movies/{movieName}")]
        [HttpGet]
        public async Task<IHttpActionResult> Search(string movieName)
        {
            try
            {
                var result = await _service.SearchMovie(movieName, 5);
                var resultViewModel = result.Select(x => new SearchResultViewModel(x)).ToList();
                return Ok(resultViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/download/{imdbCode}")]
        [HttpGet]
        public async Task<IHttpActionResult> Download(string imdbCode)
        {
            try
            {
                var result = await _service.DownloadMovie(imdbCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
