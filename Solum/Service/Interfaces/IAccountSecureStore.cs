using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace Solum.Service.Interfaces
{
    public interface IAccountSecureStore
    {
        MobileServiceUser RetrieveTokenFromSecureStore();
        void StoreTokenInSecureStore(MobileServiceUser user);
        void RemoveTokenFromSecureStore();
        Task<MobileServiceUser> LoginAsync(MobileServiceClient client);
        Task LogoutAsync();
        string GetSyncStore();
    }
}