using System;
using System.Linq;
using Android.Widget;
using Solum.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(TextSizeEffect), "TextSizeEffect")]
namespace Solum.Droid.Effects
{
	public class TextSizeEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			try
			{
				var effect = (Solum.Effects.TextSizeEffect)Element.Effects.FirstOrDefault(e => e is Solum.Effects.TextSizeEffect);
				(Control as EditText).SetTextSize(Android.Util.ComplexUnitType.Sp, (float)effect.TextSize);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}
		}

		protected override void OnDetached()
		{
			throw new NotImplementedException();
		}
	}
}

