using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Realms;
using Solum.Models;
using Solum.Remotes;
using Solum.Remotes.Results;

namespace Solum.Service
{
    public class AuthService
    {
        private readonly AccountRemote _accountRemote;

        public AuthService()
        {
            _accountRemote = new AccountRemote();
        }

        public async Task<RegisterResult> Register(RegisterBinding register)
        {
            var result = await _accountRemote.Register(register);
            return result.IsSuccessStatusCode ? RegisterResult.RegisterSuccefully : RegisterResult.RegisterUnsuccessfully;
        }

        public async Task<AuthResult> Login(LoginBinding login)
        {
            var result = await _accountRemote.Login(login);
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
            var realm = Realm.GetInstance();
            using (var tsc = realm.BeginWrite())
            {
                realm.Add(usuario);
                tsc.Commit();
            }
            
            return AuthResult.LoginSuccessFully;
        }

        public async Task<RefreshTokenResult> RefreshToken(RefreshTokenBinding refreshToken)
        {
            var result = await _accountRemote.RefreshToken(refreshToken);
            if (!result.IsSuccessStatusCode) return RefreshTokenResult.Fail;
            var content = await result.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            var username = json["username"];
            var realm = Realm.GetInstance();
            var user = realm.All<Usuario>().FirstOrDefault(x => x.Username.Equals(username));
            using (var tsc = realm.BeginWrite())
            {
                user.AccessToken = json["access_token"];
                user.RefreshToken = json["refresh_token"];
                user.TokenCreated = DateTimeOffset.Parse(json[".issued"]);
                user.TokenValidate = DateTimeOffset.Parse(json[".expires"]);
            }
            return RefreshTokenResult.Success;
        }

        public async Task Logoff()
        {
            //verificar dados não sincronizados primeiro;
            if(!CrossConnectivity.Current.IsConnected)
                throw new Exception("Nâo foi possível fazer logoff pois não existe conexão com o servidor!");
            await _accountRemote.Logout();
            var realm = Realm.GetInstance();
            using (var tsc = realm.BeginWrite())
            {
                realm.RemoveAll<Usuario>();
                tsc.Commit();
            }
        }
    }
}