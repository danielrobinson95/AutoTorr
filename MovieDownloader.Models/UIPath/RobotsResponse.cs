using Newtonsoft.Json;

namespace MovieDownloader.Models.UIPath
{
    public class RobotsResponse
    {
        [JsonProperty(PropertyName = "odatacontext")]
        public string ODataContext { get; set; }

        [JsonProperty(PropertyName = "value")]
        public Robot[] Robots { get; set; }
    }
} 
