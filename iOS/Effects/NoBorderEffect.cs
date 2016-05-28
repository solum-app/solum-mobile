using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Solum.iOS.Effects;
using UIKit;
using System;

[assembly:ResolutionGroupName ("Sydy")]
[assembly:ExportEffect (typeof(NoBorderEffect), "NoBorderEffect")]
namespace Solum.iOS.Effects

{
	public class NoBorderEffect : PlatformEffect
	{
		protected override void OnAttached ()
		{
			try {
				(Control as UITextField).BorderStyle = UITextBorderStyle.None;
			} catch (Exception ex) {
				Console.WriteLine ("Cannot set property on attached control. Error: ", ex.Message);
			}
		}

		protected override void OnDetached ()
		{
		}
	}
}