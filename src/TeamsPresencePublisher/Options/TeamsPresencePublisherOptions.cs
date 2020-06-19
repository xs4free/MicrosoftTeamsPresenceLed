using System;
using System.IO;

namespace TeamsPresencePublisher.Options
{
    public class TeamsPresencePublisherOptions
    {
        public string SettingsFolder => Path.Combine(ApplicationFolder, "settings");
        public string CacheFolder => Path.Combine(ApplicationFolder, "cache");
        private string ApplicationFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TeamsPresencePublisher");
    }
}
