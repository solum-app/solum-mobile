using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Solum.Auth;
using Solum.Helpers;
using Solum.iOS.Auth;
using Solum.Service;
using UIKit;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(SocialAuthentication))]

namespace Solum.iOS.Auth
{
    public class SocialAuthentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(IMobileServiceClient client,
            MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                var accountStore = AccountStore.Create();
                var user = await client.LoginAsync(GetController(), provider, parameters);
                var acc = new Account(user.UserId);
                acc.Properties.Add("token", user.MobileServiceAuthenticationToken);
                accountStore.SaveAsync(acc, provider.ToString());
                return user;
            }
            catch (Exception e)
            {
                e.Data["method"] = "LoginAsync";
            }

            return null;
        }

        public void ClearCookies()
        {
            var store = NSHttpCookieStorage.SharedStorage;
            var cookies = store.Cookies;

            foreach (var c in cookies)
                store.DeleteCookie(c);
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

        public async Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, string provider,
            JObject obj)
        {
            var accountStore = AccountStore.Create();
            var user = await client.LoginAsync(provider, obj);
            var acc = new Account(user.UserId);
            acc.Properties.Add("token", user.MobileServiceAuthenticationToken);
            accountStore.SaveAsync(acc, provider);
            return user;
        }

        private UIViewController GetController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var root = window.RootViewController;
            if (root == null)
                return null;

            var current = root;
            while (current.PresentedViewController != null)
                current = current.PresentedViewController;

            return current;
        }

        public async Task<bool> IsLogged()
        {
            var accountStore = AccountStore.Create();
            var identityLogin = (await accountStore.FindAccountsForServiceAsync(Settings.AuthProvider)).Any();
            var fbLogin =
                (await accountStore.FindAccountsForServiceAsync(MobileServiceAuthenticationProvider.Facebook.ToString()))
                .Any();
            var gLogin =
                (await accountStore.FindAccountsForServiceAsync(MobileServiceAuthenticationProvider.Google.ToString()))
                .Any();
            return identityLogin || fbLogin || gLogin;
        }
    }
}