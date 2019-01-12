using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using MovieDownloader.Models;
using MovieDownloader.Models.UIPath;
using Environment = MovieDownloader.Models.UIPath.Environment;

namespace MovieDownloader.FileSorter.Core
{
    public class UIPath : IDisposable
    {
        private readonly Uri _endpoint;
        private readonly HttpClient _http;
        private readonly User _user;
        private string _token;
        private readonly string _releaseName;
        private readonly RobotRequest _robotRequest;

        public UIPath(Settings settings)
        {
            _releaseName = settings.ReleaseName;
            _endpoint    = new Uri(settings.UIPathEndpoint);
            _http        = new HttpClient();
            _user        = new User(settings.Tenancy, settings.UIPathUsername, settings.UIPathPassword);
            _robotRequest = new RobotRequest
            {
                StartInfo = new Startinfo
                {
                    NoOfRobots = 0,
                    Strategy = "Specific"
                }
            };
        }

        public async Task Authenticate()
        {
            var uri      = new Uri(_endpoint, "api/Account/Authenticate");
            var response = await _http.PostAsJsonAsync(uri, _user);

            if (!response.IsSuccessStatusCode)
                throw new HttpException($"{response.StatusCode} - Request to {uri.AbsolutePath} failed");

            var content  = await response.Content.ReadAsAsync<AuthResponse>();
            _token       = content.Token;

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task GetReleaseKey()
        {
            var uri      = new Uri(_endpoint, "odata/Releases");
            var response = await _http.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpException($"{response.StatusCode} - Request to {uri.AbsolutePath} failed");

            var content = await response.Content.ReadAsAsync<ReleaseResponse>();
            var release = content.Releases
                    .FirstOrDefault(x => x.Name.Contains(_releaseName));

            if (release == null) 
                throw new NullReferenceException($"UI Path Error: Unable to locate release with key {_releaseName}");

            _robotRequest.StartInfo.ReleaseKey = release.Key;
        }

        public async Task GetRobot()
        {
            var uri      = new Uri(_endpoint, "odata/Robots");
            var response = await _http.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpException($"{response.StatusCode} - Request to {uri.AbsolutePath} failed");

            var content = await response.Content.ReadAsAsync<RobotsResponse>();
            var robot = content.Robots.FirstOrDefault();

            if(robot == null)
                throw new NullReferenceException($"UI Path Error: Unable to locate robot with release key {_releaseName}");

            _robotRequest.StartInfo.RobotIds = new[] {robot.Id};
        }

        public async Task LaunchRobot()
        {
            var uri = new Uri(_endpoint, "odata/Jobs/UiPath.Server.Configuration.OData.StartJobs");
            await _http.PostAsJsonAsync(uri, _robotRequest);
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
