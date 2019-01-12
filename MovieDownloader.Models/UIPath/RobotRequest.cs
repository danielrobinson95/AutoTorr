using Newtonsoft.Json;

namespace MovieDownloader.Models.UIPath
{
    public class RobotRequest
    {
        [JsonProperty(PropertyName = "startInfo")]
        public Startinfo StartInfo { get; set; }
    }
}
