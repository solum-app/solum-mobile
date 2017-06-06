using Foundation;
using Google.SignIn;
using Solum.Helpers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Solum.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        private const string HOCKEYAPP_KEY = "48f0548fc8a044709b96170f1979a0c6";

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			//Inicializations
            Forms.Init();
			XFGloss.iOS.Library.Init();
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			var googleServiceDictionary = NSDictionary.FromFile("GoogleService-Info.plist");
			SignIn.SharedInstance.ClientID = googleServiceDictionary["CLIENT_ID"].ToString();

		 	UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
			//Settings.DBPath = FileAccessHelper.GetLocalFilePath (Settings.DBPath);
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			var openUrlOptions = new UIApplicationOpenUrlOptions(options);
			return SignIn.SharedInstance.HandleUrl(url, openUrlOptions.SourceApplication, openUrlOptions.Annotation);
		}

		public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			return SignIn.SharedInstance.HandleUrl(url, sourceApplication, annotation);
		}
    }
}