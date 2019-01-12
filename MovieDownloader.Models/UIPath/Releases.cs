using System;

namespace MovieDownloader.Models.UIPath
{
    public class Release
    {
        public string Key { get; set; }
        public string ProcessKey { get; set; }
        public string ProcessVersion { get; set; }
        public bool IsLatestVersion { get; set; }
        public bool IsProcessDeleted { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int EnvironmentId { get; set; }
        public string EnvironmentName { get; set; }
        public Environment Environment { get; set; }
        public string InputArguments { get; set; }
        public CurrentVersion CurrentVersion { get; set; }
        public ReleaseVersion[] ReleaseVersions { get; set; }
        public Arguments Arguments { get; set; }
        public int Id { get; set; }
    }
}
