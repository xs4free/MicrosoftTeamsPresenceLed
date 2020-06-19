using Microsoft.Graph;
using System.Threading.Tasks;

namespace TeamsPresencePublisher.Services
{
    public interface IMicrosoftAuthentication
    {
        IAuthenticationProvider AuthProvider { get; }

        Task<string> GetUserNameAsync();

        Task<bool> SigninAsync();

        Task<bool> IsSignedInAsync();

        Task Signout();
    }
}
