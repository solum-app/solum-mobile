using Foundation;
using Microsoft.WindowsAzure.MobileServices;
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
            //HockeyApp setup
#if (!DEBUG)
			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure (HOCKEYAPP_KEY);
			manager.CrashManager.CrashManagerStatus = BITCrashManagerStatus.AutoSend;
			manager.StartManager ();
			#endif

            Forms.Init();
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            CurrentPlatform.Init();
			Settings.DBPath = FileAccessHelper.GetLocalFilePath ("Solum.db");
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}