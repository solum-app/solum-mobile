using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Solum.Models;

namespace Solum.Abstractions
{
    public interface ICloudService
    {
        ICloudTable<T> GetTable<T>() where T : TableData;

        Task<MobileServiceUser> LoginAsync();

        Task LogoutAsync();

        Task LoginAsync(LoginBinding user);
    }
}
