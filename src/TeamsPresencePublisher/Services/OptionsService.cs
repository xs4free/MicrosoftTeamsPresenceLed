using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TeamsPresencePublisher.Options;

namespace TeamsPresencePublisher.Services
{
    public class OptionsService : IOptionsService
    {
        private readonly TeamsPresencePublisherOptions _options;

        public OptionsService(TeamsPresencePublisherOptions options)
        {
            _options = options;
        }

        public async void SaveSettingsAsync<T>(T value)
        {
            string json = JsonSerializer.Serialize(value);

            string fileName = $"{value.GetType().Name}.json";
            string filePath = Path.Combine(_options.SettingsFolder, fileName);

            Directory.CreateDirectory(_options.SettingsFolder);

            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task<T> ReadSettingsAsync<T>() where T : new()
        {
            string fileName = $"{typeof(T).Name}.json";
            string filePath = Path.Combine(_options.SettingsFolder, fileName);

            T settings;

            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);

                settings = JsonSerializer.Deserialize<T>(json);
            }
            else
            {
                settings = new T();
            }

            return settings;
        }
    }
}
