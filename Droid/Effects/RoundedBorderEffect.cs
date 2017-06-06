using System;
using Solum.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(RoundedBorderEffect), "RoundedBorderEffect")]
namespace Solum.Droid.Effects
{
	public class RoundedBorderEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			try
			{
				Control.SetBackgroundResource(Resource.Drawable.backgroud_entry);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}
		}

		protected override void OnDetached()
		{
		}
	}
}