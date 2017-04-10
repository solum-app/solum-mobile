using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.OS;
using Android.Webkit;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Solum.Auth;
using Solum.Droid.Auth;
using Solum.Helpers;
using Xamarin.Forms;
using Debug = System.Diagnostics.Debug;

[assembly: Dependency(typeof(SocialAuthentication))]

namespace Solum.Droid.Auth
{
    public class SocialAuthentication : IAuthentication
    {
        public Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                return client.LoginAsync(Forms.Context, provider, parameters);
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
                var user = await client.LoginAsync(provider, obj);
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
    }
}