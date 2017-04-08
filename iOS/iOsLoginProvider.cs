﻿using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Solum.Abstractions;
using Solum.iOS;
using UIKit;
using Xamarin.Auth;

[assembly: Xamarin.Forms.Dependency(typeof(iOSLoginProvider))]
namespace Solum.iOS
{
    public class iOSLoginProvider : ILoginProvider
    {
        public UIViewController RootView => UIApplication.SharedApplication.KeyWindow.RootViewController;

        public AccountStore AccountStore { get; private set; }

        public iOSLoginProvider()
        {
            AccountStore = AccountStore.Create();
        }

        public MobileServiceUser RetrieveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService("solum");
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    string token;

                    if (acct.Properties.TryGetValue("token", out token))
                    {
                        return new MobileServiceUser(acct.Username)
                        {
                            MobileServiceAuthenticationToken = token
                        };
                    }
                }
            }
            return null;
        }

        public void StoreTokenInSecureStore(MobileServiceUser user)
        {
            var account = new Account(user.UserId);
            account.Properties.Add("token", user.MobileServiceAuthenticationToken);
            AccountStore.Save(account, "solum");
        }

        public void RemoveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService("solum");
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    AccountStore.Delete(acct, "solum");
                }
            }
        }

        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client)
        {
            #region Azure AD Client Flow
            // var accessToken = await LoginADALAsync();
            // var zumoPayload = new JObject();
            // zumoPayload["access_token"] = accessToken;
            // return await client.LoginAsync("aad", zumoPayload);
            #endregion

            #region Auth0 Client Flow
            // var accessToken = await LoginAuth0Async();
            // var zumoPayload = new JObject();
            // zumoPayload["access_token"] = accessToken;
            // return await client.LoginAsync("auth0", zumoPayload);
            #endregion

            #region Facebook Client Flow
            // var accessToken = await LoginFacebookAsync();
            // var zumoPayload = new JObject();
            // zumoPayload["access_token"] = accessToken;
            // return await client.LoginAsync("facebook", zumoPayload);
            #endregion

            // Server Flow
            return await client.LoginAsync(RootView, "identity");
        }

        //#region Azure AD Client Flow
        ///// <summary>
        ///// Login via ADAL
        ///// </summary>
        ///// <returns>(async) token from the ADAL process</returns>
        //public async Task<string> LoginADALAsync()
        //{
        //    Uri returnUri = new Uri(Locations.AadRedirectUri);

        //    var authContext = new AuthenticationContext(Locations.AadAuthority);
        //    if (authContext.TokenCache.ReadItems().Count() > 0)
        //    {
        //        authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);
        //    }
        //    var authResult = await authContext.AcquireTokenAsync(
        //        Locations.AppServiceUrl, /* The resource we want to access  */
        //        Locations.AadClientId,   /* The Client ID of the Native App */
        //        returnUri,               /* The Return URI we configured    */
        //        new PlatformParameters(RootView));
        //    return authResult.AccessToken;
        //}
        //#endregion

        //#region Facebook Client Flow
        //private TaskCompletionSource<string> fbtcs;

        //public async Task<string> LoginFacebookAsync()
        //{
        //    fbtcs = new TaskCompletionSource<string>();
        //    var loginManager = new LoginManager();

        //    loginManager.LogInWithReadPermissions(new[] { "public_profile" }, RootView, LoginTokenHandler);
        //    return await fbtcs.Task;
        //}

        //private void LoginTokenHandler(LoginManagerLoginResult loginResult, NSError error)
        //{
        //    if (loginResult.Token != null)
        //    {
        //        fbtcs.TrySetResult(loginResult.Token.TokenString);
        //    }
        //    else
        //    {
        //        fbtcs.TrySetException(new Exception("Facebook Client Flow Login Failed"));
        //    }
        //}
        //#endregion
    }
}