using System;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Solum.Droid.Renderers;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
namespace Solum.Droid.Renderers
{
    public class LoginPageRenderer : PageRenderer
    {
		private const string SERVER_CLIENT_ID = "479539108104-obr8p5s2nrude4bbj9cgssceohlduks1.apps.googleusercontent.com";
        private GoogleApiClient googleApiClient;
        public static int RC_SIGN_IN = 9001;

        public static TaskCompletionSource<GoogleCredentials> SigInCompletionSource
        {
            get;
            private set;
        }
     
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
                               
            var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                             .RequestIdToken(SERVER_CLIENT_ID)
                                             .Build();

            googleApiClient = new GoogleApiClient.Builder(Forms.Context)
                                                 .AddApi(Android.Gms.Auth.Api.Auth.GOOGLE_SIGN_IN_API, gso)
                                                 .Build();

            Android.Gms.Auth.Api.Auth.GoogleSignInApi.SignOut(googleApiClient);

            var loginPage = e.NewElement as LoginPage;
            loginPage.GoogleSigInAsync = GoogleSigInAsync;
        }

        public Task<GoogleCredentials> GoogleSigInAsync()
		{
            SigInCompletionSource = new TaskCompletionSource<GoogleCredentials>();
            var signInIntent = Android.Gms.Auth.Api.Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
            if (Forms.Context is Activity formsActivity)
                formsActivity.StartActivityForResult(signInIntent, RC_SIGN_IN);
			return SigInCompletionSource.Task;
		}

    }
}
