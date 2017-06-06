using System;
using Android.App;
using Android.Content.PM;
using Android.Gms.Auth;
using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Solum.Droid.Renderers;
using Solum.Helpers;
using Solum.Models;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Solum.Droid
{
    [Activity(Icon = "@drawable/icon", Theme = "@style/Theme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            SetTheme(Resource.Style.MyTheme);
            base.OnCreate(bundle);

			//Inicializations
            Forms.Init(this, bundle);
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			Settings.DBPath = FileAccessHelper.GetLocalFilePath(Settings.DBPath);
            LoadApplication(new App());
            XFGloss.Droid.Library.Init(this, bundle);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == LoginPageRenderer.RC_SIGN_IN)
			{
                var result = Android.Gms.Auth.Api.Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
				HandleSignInResult(result);
   			}
        }

        private void HandleSignInResult(GoogleSignInResult result)
        {
            if (result.IsSuccess)
			{
                var acct = result.SignInAccount;
                var credentials = new GoogleCredentials
                {
                    IdToken = acct.IdToken,
                    AuthorizationCode = acct.ServerAuthCode,
                    Nome = acct.DisplayName,
                    Email = acct.Email
                };
                LoginPageRenderer.SigInCompletionSource?.SetResult(credentials);
            } else {
                LoginPageRenderer.SigInCompletionSource?.SetResult(null);
            }
        }
    }
}