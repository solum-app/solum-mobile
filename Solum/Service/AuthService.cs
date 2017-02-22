using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Realms;
using Solum.Handlers;
using Solum.Models;
using Solum.Remotes;
using Solum.Remotes.Results;

namespace Solum.Service
{
    public class AuthService
    {
        private readonly AccountRemote _accountRemote;
        private static AuthService _instance;
        private readonly Realm _realm;

        private AuthService()
        {
            _accountRemote = new AccountRemote();
            _realm = Realm.GetInstance();
        }

        public static AuthService Instance => _instance ?? (_instance = new AuthService());

        public async Task<RegisterResult> Register(RegisterBinding register)
        {
            var result = await _accountRemote.Register(register);
            if (result != null)
                return result.IsSuccessStatusCode ? RegisterResult.RegisterSuccefully : RegisterResult.RegisterUnsuccessfully;
            return RegisterResult.ServerUnrecheable;
        }

        public async Task<AuthResult> Login(LoginBinding login)
        {
            var result = await _accountRemote.Login(login);
            if (result == null) return AuthResult.ServerUnrecheable;
            if (!result.IsSuccessStatusCode) return AuthResult.LoginUnsuccessfully;
            var content = await result.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

            var usuario = new Usuario()
            {
                Username = json["Username"],
                TokenValidate = DateTimeOffset.Parse(json["TokenValidate"]),
                TokenCreated = DateTimeOffset.Parse(json["TokenCreated"]),
                RefreshToken = json["RefreshToken"],
                AccessToken = json["AccessToken"],
                Id = json["Id"],
                Nome = json["Name"]
            };

            using (var transaction = _realm.BeginWrite())
            {
                _realm.Add(usuario, true);
                transaction.Commit();
            }
            _accountRemote.SetToken(usuario.AccessToken);
            return AuthResult.LoginSuccessFully;
        }

        public async Task<RefreshTokenResult> RefreshToken(RefreshTokenBinding refreshToken)
        {
            var result = await _accountRemote.RefreshToken(refreshToken);
            if (result == null) return RefreshTokenResult.ServerUnrecheable;
            if (!result.IsSuccessStatusCode) return RefreshTokenResult.Fail;
            var content = await result.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            var username = json["username"];
            var user = _realm.All<Usuario>().FirstOrDefault(x => x.Username.Equals(username));

            using (var transaction = _realm.BeginWrite())
            {
                user.AccessToken = json["access_token"];
                user.RefreshToken = json["refresh_token"];
                user.TokenCreated = DateTimeOffset.Parse(json[".issued"]);
                user.TokenValidate = DateTimeOffset.Parse(json[".expires"]);
                transaction.Commit();
            }
            return RefreshTokenResult.Success;
        }

        public async Task<bool> Logoff()
        {
            var result = await _accountRemote.Logout();
            if (result == null)
            {
                "Nâo foi possível realizar logoff, servidor indisponível".ToDisplayAlert(MessageType.Falha);
                return false;
            }
            using (var transaction = _realm.BeginWrite())
            {
                _realm.RemoveAll<Usuario>();
                transaction.Commit();
            }
            return true;
        }
    }
}