using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsPresencePublisher.Options;

namespace TeamsPresencePublisher.Services
{
    public class MicrosoftAuthentication : IMicrosoftAuthentication
    {
        private const string ClientId = "7cb6e9cb-6042-49d7-b60c-fbcf1c669599"; // Teams presence publisher
        private const string Authority = "https://login.microsoftonline.com/common/";
        private const string RedirectUri = "http://localhost";
        private static readonly string[] s_scopes = new string[] { "User.Read", "Presence.Read" };
        private readonly MsalCacheHelper _msalCacheHelper;
        private IPublicClientApplication _publicClientApplication;

        public IAuthenticationProvider AuthProvider { get; private set; }

        public MicrosoftAuthentication(TeamsPresencePublisherOptions options)
        {
            StorageCreationProperties storageProperties =
                new StorageCreationPropertiesBuilder("TeamsPresencePublisher.msalcache.bin", options.CacheFolder, ClientId)
                .Build();

            _msalCacheHelper = MsalCacheHelper.CreateAsync(storageProperties).GetAwaiter().GetResult();

            _publicClientApplication = BuildPublicClientApplication();
            AuthProvider = new InteractiveAuthenticationProvider(_publicClientApplication, s_scopes);
        }

        public async Task<bool> SigninAsync()
        {
            bool result = false;

            try
            {
                AcquireTokenInteractiveParameterBuilder builder = _publicClientApplication.AcquireTokenInteractive(s_scopes);
                await builder.ExecuteAsync();

                result = true;
            }
            catch (MsalServiceException) // access_denied when user cancels login
            {
            }

            return result;
        }

        public async Task<bool> IsSignedInAsync()
        {
            IEnumerable<IAccount> accounts = await _publicClientApplication.GetAccountsAsync();
            return accounts.Any();
        }

        public async Task<string> GetUserNameAsync()
        {
            IEnumerable<IAccount> accounts = await _publicClientApplication.GetAccountsAsync();
            return accounts.FirstOrDefault()?.Username;
        }

        public async Task Signout()
        {
            foreach (IAccount account in await _publicClientApplication.GetAccountsAsync())
            {
                await _publicClientApplication.RemoveAsync(account);
            }
        }

        private IPublicClientApplication BuildPublicClientApplication()
        {
            IPublicClientApplication application = PublicClientApplicationBuilder
                    .Create(ClientId)
                    .WithAuthority(Authority)
                    .WithRedirectUri(RedirectUri)
                    .Build();

            _msalCacheHelper.RegisterCache(application.UserTokenCache);

            return application;
        }
    }
}
