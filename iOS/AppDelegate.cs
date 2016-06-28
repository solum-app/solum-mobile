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
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

			//Get the shared instance
			var manager = BITHockeyManager.SharedHockeyManager;
			//Configure it to use our APP_ID
			manager.Configure("48f0548fc8a044709b96170f1979a0c6");
			//Start the manager
			manager.StartManager();

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

