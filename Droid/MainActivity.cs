﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using HockeyApp;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Solum.Droid
{
    [Activity(Icon = "@drawable/icon", Theme = "@style/Theme.Splash", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        private const string HOCKEYAPP_KEY = "c4aa5557f72547d3b39966736013ff92";

        protected override void OnCreate(Bundle bundle)
        {
            //HockeyApp setup
#if (!DEBUG)
			CrashManager.Register (this, HOCKEYAPP_KEY, new CrashManagerSettings ());
			MetricsManager.Register(this, Application, HOCKEYAPP_KEY);
			#endif

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            SetTheme(Resource.Style.MyTheme);
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            CurrentPlatform.Init();
            LoadApplication(new App());
        }
    }

    public class CrashManagerSettings : CrashManagerListener
    {
        public override bool ShouldAutoUploadCrashes()
        {
            return true;
        }
    }
}