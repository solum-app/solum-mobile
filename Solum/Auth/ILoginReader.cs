using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace Solum.Auth
{
    public interface ILoginReader
    {
        bool IsLogged();
        Task<string> UserId();
    }
}