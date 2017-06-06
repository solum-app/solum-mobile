using System;
using CoreGraphics;
using Solum.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(RoundedBorderEffect), "RoundedBorderEffect")]
namespace Solum.iOS.Effects
{
	public class RoundedBorderEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			try
			{
				var textField = Control as UITextField;
				textField.BorderStyle = UITextBorderStyle.None;
				textField.Layer.CornerRadius = 2;
				textField.Layer.BorderWidth = 1;
				textField.Layer.BorderColor = Color.FromHex("#E6E6E6").ToCGColor();
				textField.BackgroundColor = Color.FromHex("#FAFAFA").ToUIColor();
				var paddingView = new UIView(new CGRect(0, 0, 16, 0));
				textField.LeftView = paddingView;
				textField.LeftViewMode = UITextFieldViewMode.Always;
				textField.RightView = paddingView;
				textField.LeftViewMode = UITextFieldViewMode.Always;
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
