using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace Solum.Auth
{
    public interface IAuthentication : ILoginReader
    {
        Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> paramameters = null);
        Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, string provider, JObject obj);
        Task<bool> RefreshUser(IMobileServiceClient client);
        void ClearCookies();
    }
}