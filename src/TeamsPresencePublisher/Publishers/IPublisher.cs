using Microsoft.Graph;
using System.Threading.Tasks;

namespace TeamsPresencePublisher.Publishers
{
    public interface IPublisher
    {
        bool Enabled { get; }
        Task PublishAsync(Presence presence);
    }
}