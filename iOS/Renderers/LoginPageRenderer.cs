using System;
using System.Threading.Tasks;
using Foundation;
using Google.SignIn;
using Solum.iOS.Renderers;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
namespace Solum.iOS.Renderers
{
    public class LoginPageRenderer : PageRenderer, ISignInUIDelegate, ISignInDelegate
    {
        const string SERVER_CLIENT_ID = "479539108104-obr8p5s2nrude4bbj9cgssceohlduks1.apps.googleusercontent.com";
        private TaskCompletionSource<GoogleCredentials> _tcs;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            var page = e.NewElement as LoginPage;
            page.GoogleSigInAsync = GoogleSigInAsync;
        }

        public Task<GoogleCredentials> GoogleSigInAsync()
		{
            _tcs = new TaskCompletionSource<GoogleCredentials>();
			SignIn.SharedInstance.SignInUser();
			return _tcs.Task;
		}

        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            var credentials = new GoogleCredentials
            {
                IdToken = user.Authentication.IdToken,
                AuthorizationCode = user.ServerAuthCode,
                Nome = user.Profile.Name,
                Email = user.Profile.Email
            };

            _tcs?.SetResult(credentials);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			SignIn.SharedInstance.UIDelegate = this;
			SignIn.SharedInstance.Delegate = this;
            SignIn.SharedInstance.ServerClientID = SERVER_CLIENT_ID;
            SignIn.SharedInstance.SignOutUser();
        }

        public void ExecuteGoogleLogin() {
            SignIn.SharedInstance.SignInUser();
        }
    }
}
