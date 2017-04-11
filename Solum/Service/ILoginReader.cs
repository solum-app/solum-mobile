using System.Threading.Tasks;

namespace Solum.Service
{
    public interface ILoginReader
    {
        Task<bool> IsLogged();
    }
}