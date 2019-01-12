using System;
using System.Configuration;

namespace MovieDownloader.Models
{
    public class Settings
    {
        private static Settings _instance = null;

        private Settings()
        {
        }

        public static Settings AppSettings => _instance ?? (_instance = new Settings());

        private const string NullMessage = "App setting key required: ";

        public string TorrentsPath { get; }          = ConfigurationManager.AppSettings["TorrentsPath"] ?? throw new NullReferenceException(NullMessage + "TorrentsPath");
        public string CompletedPath { get; }         = ConfigurationManager.AppSettings["CompletedPath"] ?? throw new NullReferenceException(NullMessage + "CompletedPath");
        public string PlexPath { get; }              = ConfigurationManager.AppSettings["PlexPath"] ?? throw new NullReferenceException(NullMessage + "PlexPath");
        public string PlexScriptPath { get; }        = ConfigurationManager.AppSettings["PlexScriptPath"] ?? throw new NullReferenceException(NullMessage + "PlexScriptPath");
        public string UIPathUsername { get; set; }   = ConfigurationManager.AppSettings["UIPathUsername"] ?? throw new NullReferenceException(NullMessage + "UIPathUsername");
        public string UIPathPassword { get; set; }   = ConfigurationManager.AppSettings["UIPathPassword"] ?? throw new NullReferenceException(NullMessage + "UIPathPassword");
        public string Tenancy { get; set; }          = ConfigurationManager.AppSettings["Tenancy"] ?? throw new NullReferenceException(NullMessage + "Tenancy");
        public string ToEmailAddress { get; set; }   = ConfigurationManager.AppSettings["ToEmailAddress"] ?? throw new NullReferenceException(NullMessage + "ToEmailAddress");
        public string FromEmailAddress { get; set; } = ConfigurationManager.AppSettings["FromEmailAddress"] ?? throw new NullReferenceException(NullMessage + "FromEmailAddress");
        public string EmailPassword { get; set; }    = ConfigurationManager.AppSettings["EmailPassword"] ?? throw new NullReferenceException(NullMessage + "EmailPassword");
        public string ReleaseName { get; set; }      = ConfigurationManager.AppSettings["ReleaseName"] ?? throw new NullReferenceException(NullMessage + "ReleaseName");
        public string UIPathEndpoint{ get; set; }    = ConfigurationManager.AppSettings["UIPathEndpoint"] ?? throw new NullReferenceException(NullMessage + "UIPathEndpoint");

        public int DeleteTimer { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["DeletePauseInMilliseconds"]);
    }
}
