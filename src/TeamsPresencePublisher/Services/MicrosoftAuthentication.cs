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
        private const string ClientId = "f8119f58-4523-44c1-ab72-b2e0c815bd6a"; // PresenceLight
        /*private const string ClientId = "3baaf745-161e-447d-98a2-9f7274bd757a";*/ // DeviceCodeFlowConsole-sample2 -> verify Publisher domain, misschien werkt hij dan wel?!
        //private const string ClientId = "12a0fda7-afa5-41b8-924a-755501d2761d"; // DeviceCodeFlowConsole-sample

        private const string Authority = "https://login.microsoftonline.com/common/";
        private static readonly string[] s_scopes = new string[] { "User.Read", "Presence.Read" };
        private readonly MsalCacheHelper _msalCacheHelper;
        private IPublicClientApplication _publicClientApplication;

        public IAuthenticationProvider AuthProvider { get; private set; }

        public MicrosoftAuthentication(TeamsPresencePublisherOptions options)
        {
            var storageProperties =
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
                var builder = _publicClientApplication.AcquireTokenInteractive(s_scopes);
                await builder.ExecuteAsync();

                result = true;
            }
            catch(MsalServiceException) // access_denied when user cancels login
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
            var application = PublicClientApplicationBuilder
                    .Create(ClientId)
                    .WithAuthority(Authority)
                    //.WithDefaultRedirectUri()
                    .WithRedirectUri("http://localhost")
                    .Build();

            _msalCacheHelper.RegisterCache(application.UserTokenCache);

            return application;
        }
    }
}
