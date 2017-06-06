using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Solum.Helpers;
using Solum.Models;
using Xamarin.Auth;

namespace Solum.Service
{
    public class AuthService
    {
		private static AuthService _instance;
		public static AuthService Instance = _instance ?? (_instance = new AuthService());

        public async Task<RequestResult<MobileServiceUser>> LoginAsync(string username, string password)
		{
			try
			{
				var client = new MobileServiceClient(Settings.BaseUri);
				var credentials = new JObject
				{
                    ["email"] = username,
                    ["password"] = password,
				};

                var user = await client.LoginAsync("custom", credentials);
				SaveCredentials(user);
                return new RequestResult<MobileServiceUser> { Data = user, StatusCode = System.Net.HttpStatusCode.OK };
			}
            catch (MobileServiceInvalidOperationException ex)
			{
                var content = await ex.Response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RequestResult<MobileServiceUser>>(content);
                result.StatusCode = ex.Response.StatusCode;
                return result;
			}
		}

        public async Task<RequestResult<MobileServiceUser>> GoogleLoginAsync(GoogleCredentials credentials)
		{
            try
            {
                var client = new MobileServiceClient(Settings.BaseUri);
                var accessToken = new JObject
                {
                    ["id_token"] = credentials.IdToken,
                    ["authorization_code"] = credentials.AuthorizationCode,
                };

                var user = await client.LoginAsync(MobileServiceAuthenticationProvider.Google, accessToken);

                var externalUser = new Dictionary<string, string>
				{
					{"email", credentials.Email},
					{"nome", credentials.Nome},
					{"externalId", user.UserId},
                    {"provider", ((int)Provider.Google).ToString() }
				};

                var response = await new HttpClient().PostAsync($"{Settings.BaseUri}/.auth/register/external", new FormUrlEncodedContent(externalUser));

                var result = new RequestResult<MobileServiceUser>();

                if (response.IsSuccessStatusCode) {
					SaveCredentials(user);
                    result.Data = user;
                } else {
					var content = await response.Content.ReadAsStringAsync();
					result = JsonConvert.DeserializeObject<RequestResult<MobileServiceUser>>(content);
                }

                result.StatusCode = response.StatusCode;
                return result;
            }
			catch (MobileServiceInvalidOperationException ex)
            {
				var content = await ex.Response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<RequestResult<MobileServiceUser>>(content);
				result.StatusCode = ex.Response.StatusCode;
				return result;
			}
		}

        public async Task<RequestResult> RegisterAsync(string nome, string email, string password, string cidadeId)
        {
            var data = new Dictionary<string, string>
            {
                {"nome", nome},
                {"email", email},
                {"password", password},
                {"cidadeId", cidadeId}
            };

			var response = await new HttpClient().PostAsync($"{Settings.BaseUri}/.auth/register", new FormUrlEncodedContent(data));

			var result = new RequestResult<MobileServiceUser>();
			var content = await response.Content.ReadAsStringAsync();
			result = JsonConvert.DeserializeObject<RequestResult<MobileServiceUser>>(content);
			result.StatusCode = response.StatusCode;
			return result;

		}		

        public void SaveCredentials(MobileServiceUser user) {
            if (user != null)
            {
                Account account = new Account
                {
                    Username = user.UserId
                };
                account.Properties.Add("token", user.MobileServiceAuthenticationToken);
                AccountStore.Create().Save(account, App.AppName);
            }
        }

        public MobileServiceUser GetCredentials
		{
			get
			{
				var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                if (account != null) {
                    return new MobileServiceUser (account.Username)
                    {
                        MobileServiceAuthenticationToken = account.Properties["token"]
                    };
                }

                return null;
			}
		}

		public void DeleteCredentials()
		{
			var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
			if (account != null)
			{
				AccountStore.Create().Delete(account, App.AppName);
			}
		}
    }
}
