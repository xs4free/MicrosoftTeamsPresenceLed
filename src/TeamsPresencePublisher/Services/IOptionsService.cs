using System.Threading.Tasks;

namespace TeamsPresencePublisher.Services
{
    public interface IOptionsService
    {
        Task<T> ReadSettingsAsync<T>() where T : new();
        void SaveSettingsAsync<T>(T value);
    }
}