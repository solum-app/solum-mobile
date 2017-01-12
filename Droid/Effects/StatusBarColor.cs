using Android.App;
using Android.OS;
using Android.Views;
using Solum.Droid.Effects;
using Solum.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:Dependency(typeof(StatusBarColor))]
namespace Solum.Droid.Effects
{
    public class StatusBarColor : IStatusBarColor
    {
        public void SetColor(Color color)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop) return;
            var activity = Forms.Context as Activity;
            if (activity == null) return;
            activity.Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            activity.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            activity.Window.SetStatusBarColor(color.ToAndroid());
        }
    }
}