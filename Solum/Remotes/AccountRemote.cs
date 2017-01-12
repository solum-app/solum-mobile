using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Solum.Models;

namespace Solum.Remotes
{
    public class AccountRemote : BaseRemote
    {
        public async Task<HttpResponseMessage> Register(RegisterBinding binding)
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception(
                    "Sem Conexão com Internet. Não foi possível enviar dos dados de cadastro para o servidor.");
            var dict = new Dictionary<string, string>
            {
                {"Nome", binding.Nome},
                {"Email", binding.Email},
                {"Password", binding.Password},
                {"ConfirmPassword", binding.ConfirmPassword},
                {"CidadeId", binding.CidadeId}
            };

            var content = new FormUrlEncodedContent(dict);
            var url = $"{Settings.BaseUri}{Settings.AccountRegisterUri}";
            var result = await Client.PostAsync(url, content);
            return result;
        }

        public async Task<HttpResponseMessage> Login(LoginBinding binding)
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception(
                    "Sem Conexão com Internet. Não foi possível enviar os dados de login para o servidor.");
            var dict = new Dictionary<string, string>
            {
                {"username", binding.Username},
                {"password", binding.Password}
            };
            var content = new FormUrlEncodedContent(dict);
            var url = $"{Settings.BaseUri}{Settings.AccountLoginUri}";
            return await Client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> RefreshToken(RefreshTokenBinding refreshToken)
        {
            if (!CrossConnectivity.Current.IsConnected) throw new Exception("Sem conexão com a Internet");
            var dict = new Dictionary<string, string>()
            {
                {"refresh_token", refreshToken.RefreshToken },
                {"grant_type", RefreshTokenBinding.GrantType }
            };
            var content = new FormUrlEncodedContent(dict);
            var url = $"{Settings.BaseUri}{Settings.TokenUri}";
            return await Client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> Logout()
        {
            var url = $"{Settings.BaseUri}{Settings.AccountLogoutUri}";
            return await Client.PostAsync(url, null);
        }
    }
}