using System.Threading.Tasks;

namespace Solum.Auth
{
    public interface ILoginReader
    {
        Task<bool> IsLogged();
    }
}