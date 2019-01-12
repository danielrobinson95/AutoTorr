using Newtonsoft.Json;

namespace MovieDownloader.Models.UIPath
{
    public class AuthResponse
    {
        [JsonProperty(PropertyName = "result")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "targetUrl")]
        public object TargetUrl { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public object Error { get; set; }

        [JsonProperty(PropertyName = "unAuthorizedRequest")]
        public bool UnAuthorizedRequest { get; set; }

        [JsonProperty(PropertyName = "__abp")]
        public bool Abp { get; set; }
    }
}
