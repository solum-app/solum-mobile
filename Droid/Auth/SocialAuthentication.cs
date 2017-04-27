using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Android.Webkit;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Solum.Auth;
using Solum.Droid.Auth;
using Solum.Helpers;
using Xamarin.Auth;
using Xamarin.Forms;
using Debug = System.Diagnostics.Debug;

[assembly: Dependency(typeof(SocialAuthentication))]

namespace Solum.Droid.Auth
{
    public class SocialAuthentication : IAuthentication
    {
        public async Task<JToken> RegisterUser(IMobileServiceClient client, string endpoint, JObject obj)
        {
            return await client.InvokeApiAsync(endpoint, obj);
        }

        public async Task<MobileServiceUser> LoginAsync(IMobileServiceClient client,
            MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                var accountStore = AccountStore.Create(Forms.Context);
                var user = await client.LoginAsync(Forms.Context, provider, parameters);
                var acc = new Account(user.UserId);
                acc.Properties.Add("token", user.MobileServiceAuthenticationToken);
                accountStore.SaveAsync(acc, provider.ToString());
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, string provider, JObject obj)
        {
            try
            {
                var accountStore = AccountStore.Create(Forms.Context);
                var user = await client.LoginAsync(provider, obj);
                var acc = new Account(user.UserId);
                acc.Properties.Add("token", user.MobileServiceAuthenticationToken);
                accountStore.SaveAsync(acc, provider);
                return user;
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public void ClearCookies()
        {
            try
            {
                if ((int) Build.VERSION.SdkInt >= 21)
                    CookieManager.Instance.RemoveAllCookies(null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public virtual async Task<bool> RefreshUser(IMobileServiceClient client)
        {
            try
            {
                var user = await client.RefreshUserAsync();
                if (user != null)
                {
                    client.CurrentUser = user;
                    Settings.Token = user.MobileServiceAuthenticationToken;
                    Settings.UserId = user.UserId;
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to refresh user: " + e);
            }

            return false;
        }

        public bool IsLogged()
        {
            var accountStore = AccountStore.Create(Forms.Context);
            var identityLogin = accountStore.FindAccountsForService(Settings.AuthProvider).Any();
            var fbLogin = accountStore.FindAccountsForService(MobileServiceAuthenticationProvider.Facebook.ToString()).Any();
            return identityLogin || fbLogin;
        }

        public async Task<string> UserId()
        {
            var accountStore = AccountStore.Create(Forms.Context);
            var userlogins = await accountStore.FindAccountsForServiceAsync(Settings.AuthProvider);
            if (userlogins.Any())
                return userlogins.FirstOrDefault()?.Username;
            userlogins =
                await accountStore.FindAccountsForServiceAsync(MobileServiceAuthenticationProvider.Facebook.ToString());
            if (userlogins.Any())
                return userlogins.FirstOrDefault()?.Username;
            return null;
        }
    }
}