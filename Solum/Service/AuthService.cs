using System.Threading.Tasks;
using Solum.Models;
using Solum.Remotes;

namespace Solum.Service
{
    public class AuthService
    {
        private readonly AccountRemote _accountRemote;
        private readonly UserDataService _userDataService;

        public AuthService()
        {
            _accountRemote = new AccountRemote();
        }

        public async Task<RegisterResult> Register(RegisterBinding register)
        {
            var result = await _accountRemote.Register(register);
            return result.IsSuccessStatusCode ? RegisterResult.RegisterSuccefully : RegisterResult.RegisterUnsuccessfully;
        }

        public async Task Login(LoginBinding login)
        {
            await _accountRemote.Login(login);
        }

        public async Task RefreshToken(RefreshTokenBinding refreshToken) { }

        public async Task Logoff() { }
    }
}