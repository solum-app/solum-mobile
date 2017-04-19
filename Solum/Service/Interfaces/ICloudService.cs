using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Solum.Models;

namespace Solum.Service.Interfaces
{
    public interface ICloudService
    {
        #region Auth

        //Task<AppServiceIdentity> GetIdentityAsync();
        //Task<MobileServiceUser> LoginAsync();
        Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> paramameters = null);
        Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, string provider, JObject obj);
        Task<bool> RefreshUser(IMobileServiceClient client);
        Task LogoutAsync();
        void ClearCookies();

        #endregion

        #region Sync

        Task<ICloudTable<T>> GetTableAsync<T>() where T : EntityData;
        Task SyncOfflineCacheAsync();
       
        #endregion
        
    }

    public interface ICloudTable<T> where T : EntityData
    {
        Task<T> CreateItemAsync(T item);
        Task<T> ReadItemAsync(string id);
        Task<T> UpdateItemAsync(T item);
        Task<T> UpsertItemAsync(T item);
        Task DeleteItemAsync(T item);
        Task<ICollection<T>> ReadAllItemsAsync();
        Task<ICollection<T>> ReadItemsAsync(int start, int count);
        Task PullAsync();
    }

    public interface ISecurityStore
    {
        MobileServiceUser RetrieveTokenFromSecureStore();
        void StoreTokenInSecureStore(MobileServiceUser user);
        void RemoveTokenFromSecureStore();
        Task<MobileServiceUser> LoginAsync(MobileServiceClient client);
        Task LogoutAsync();
        string GetSyncStore();
    }

}