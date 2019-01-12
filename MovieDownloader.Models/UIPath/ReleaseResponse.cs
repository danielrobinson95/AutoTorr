

using Newtonsoft.Json;

namespace MovieDownloader.Models.UIPath
{
    public class ReleaseResponse
    {
        [JsonProperty(PropertyName = "odatacontext")]
        public string ODataContext { get; set; }

        [JsonProperty(PropertyName = "value")]
        public Release[] Releases { get; set; }
    }
}
