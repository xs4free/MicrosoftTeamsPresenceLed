using Microsoft.Graph;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TeamsPresencePublisher.Services
{
    public interface IPresenceService
    {
        Task<Presence> GetPresenceAsync();
        BitmapImage GetDefaultPhoto();
        Task<BitmapImage> GetPhotoAsync();
    }
}