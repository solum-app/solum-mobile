using System.Linq;
using Solum.Models;

namespace Solum.Service
{
    public class UserDataService : DataService<Usuario>
    {
        public Usuario GetLoggedUser()
        {
            return Instance.All<Usuario>().FirstOrDefault();
        }

        public Usuario FindByUsername(string username)
        {
            return Instance.All<Usuario>().FirstOrDefault(x => x.Username.Equals(username));
        }
    }
}