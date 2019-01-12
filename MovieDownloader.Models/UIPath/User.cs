using Newtonsoft.Json;

namespace MovieDownloader.Models.UIPath
{
    public class User
    {
        public User(string tenancy, string user, string password)
        {
            Username = user;
            TenancyName = tenancy;
            Password = password;
        }

        [JsonProperty(PropertyName = "tenancyName")]
        public string TenancyName { get; private set; }

        [JsonProperty(PropertyName = "usernameOrEmailAddress")]
        public string Username { get; private set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; private set; }
    }
}
