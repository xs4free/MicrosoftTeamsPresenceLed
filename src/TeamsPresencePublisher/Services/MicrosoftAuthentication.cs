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
        private const string ClientId = "c6135228-1fc2-4915-bf89-9afc5578b6fd"; // Teams Presence Publisher Azure 2 - azure2@developeratwork.nl
        //private const string ClientId = "677aa18f-676f-417d-a570-c29b11a3306e"; // Teams Presence Publisher @ rogier@developeratwork.onmicrosoft.com
        //private const string ClientId = "201c8652-822d-4778-bfa2-160bb5e97a5d"; // Test PresenceLed help
        //private const string ClientId = "8d0c043c-ce1b-4993-906e-6428354a6b73"; // TestLed
        //private const string ClientId = "7cb6e9cb-6042-49d7-b60c-fbcf1c669599"; // Teams presence publisher
        //private const string ClientId = "f8119f58-4523-44c1-ab72-b2e0c815bd6a"; // PresenceLight
        /*private const string ClientId = "3baaf745-161e-447d-98a2-9f7274bd757a";*/ // DeviceCodeFlowConsole-sample2 -> verify Publisher domain, misschien werkt hij dan wel?!
        //private const string ClientId = "12a0fda7-afa5-41b8-924a-755501d2761d"; // DeviceCodeFlowConsole-sample

        private const string Authority = "https://login.microsoftonline.com/common/";
        private static readonly string[] s_scopes = new string[] { "user.read", "presence.read" };
        private static readonly string[] s_scopes_default = new string[] { "https://graph.microsoft.com/.default" };
        //private static readonly string[] s_scopes = new string[] { "https://graph.microsoft.com/user.read", "https://graph.microsoft.com/presence.read" };
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

            //AuthProvider = new WPFAuthorizationProvider(_publicClientApplication, s_scopes_default);
        }

        public async Task<bool> SigninAsync()
        {
            bool result = false;

            try
            {
                var builder = _publicClientApplication.AcquireTokenInteractive(s_scopes_default);
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
            var application = PublicClientApplicationBuilder
                    .Create(ClientId)
                    .WithAuthority(Authority)
                    .WithRedirectUri("http://localhost")
                    .Build();

            _msalCacheHelper.RegisterCache(application.UserTokenCache);

            return application;
        }
    }
}
