using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.Remotes
{
    public class AccountRemote : BaseRemote
    {
        public async Task<HttpResponseMessage> Register(RegisterBinding binding)
        {
            if (!CrossConnectivity.Current.IsConnected)
                    "Não foi possível realizar o cadastro pois não existe conexão com a internet".ToToast(ToastNotificationType.Erro);
            var isReacheable = await CrossConnectivity.Current.IsReachable(Settings.BaseUri.Host);
            if (!isReacheable)
            {
                return null;
            }
            else
            {

                var dict = new Dictionary<string, string>
                {
                    {"Nome", binding.Nome.Trim()},
                    {"Email", binding.Email.Trim()},
                    {"Password", binding.Password.Trim()},
                    {"ConfirmPassword", binding.ConfirmPassword.Trim()},
                    {"CidadeId", binding.CidadeId.Trim()}
                };

                var content = new FormUrlEncodedContent(dict);
                var url = $"{Settings.BaseUri}{Settings.AccountRegisterUri}";
                var result = await Client.PostAsync(url, content);
                return result;
            }
        }

        public async Task<HttpResponseMessage> Login(LoginBinding binding)
        {
            if (!CrossConnectivity.Current.IsConnected)
                "Não foi possível realizar login pois não existe conexão com a internet".ToToast(ToastNotificationType.Erro);
            var isReacheable = await CrossConnectivity.Current.IsReachable(Settings.BaseUri.Host);
            if (!isReacheable)
            {
                return null;
            }
            var dict = new Dictionary<string, string>
            {
                {"username", binding.Username.Trim()},
                {"password", binding.Password.Trim()}
            };
            var content = new FormUrlEncodedContent(dict);
            var url = $"{Settings.BaseUri}{Settings.AccountLoginUri}";
            return await Client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> RefreshToken(RefreshTokenBinding refreshToken)
        {
            if (!CrossConnectivity.Current.IsConnected)
                "Não foi possível atualizar dados".ToToast(ToastNotificationType.Erro);
            var isReacheable = await CrossConnectivity.Current.IsReachable(Settings.BaseUri.Host);
            if (!isReacheable)
            {
                return null;
            }

            var dict = new Dictionary<string, string>()
            {
                {"refresh_token", refreshToken.RefreshToken.Trim() },
                {"grant_type", RefreshTokenBinding.GrantType.Trim() }
            };
            var content = new FormUrlEncodedContent(dict);
            var url = $"{Settings.BaseUri}{Settings.TokenUri}";
            return await Client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> Logout()
        {
            if (!CrossConnectivity.Current.IsConnected)
                "Não foi possível sair pois não existe conexão com a internet".ToToast(ToastNotificationType.Erro);
            var isReacheable = await CrossConnectivity.Current.IsReachable(Settings.BaseUri.Host);
            if (!isReacheable)
            {
                return null;
            }
            var url = $"{Settings.BaseUri}{Settings.AccountLogoutUri}";
            return await Client.PostAsync(url, null);
        }
    }
}