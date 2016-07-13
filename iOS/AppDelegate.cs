using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using HockeyApp;
using UIKit;

namespace Solum.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		const string HOCKEYAPP_KEY = "48f0548fc8a044709b96170f1979a0c6";

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			//HockeyApp setup
			#if (!DEBUG)
			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure (HOCKEYAPP_KEY);
			manager.CrashManager.CrashManagerStatus = BITCrashManagerStatus.AutoSend;
			manager.StartManager ();
			#endif

			global::Xamarin.Forms.Forms.Init ();
			UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

