using Xamarin.Forms;
using Solum.Droid.Effects;
using Xamarin.Forms.Platform.Android;
using System;


[assembly:ResolutionGroupName ("Sydy")]
[assembly:ExportEffect (typeof(NoBorderEffect), "NoBorderEffect")]
namespace Solum.Droid.Effects

{
	public class NoBorderEffect : PlatformEffect
	{
		protected override void OnAttached ()
		{
			try {
				Control.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
			} catch (Exception ex) {
				Console.WriteLine ("Cannot set property on attached control. Error: ", ex.Message);
			}
		}

		protected override void OnDetached ()
		{
		}
	}
}