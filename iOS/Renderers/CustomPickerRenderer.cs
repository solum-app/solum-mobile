using System;
using System.ComponentModel;
using Solum.iOS;
using Solum.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace Solum.iOS
{
	public class CustomPickerRenderer : PickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null && e.OldElement == null)
			{
				var view = (CustomPicker)Element;

				if (view != null)
				{
					SetBorder(view);
					SetAlignment(view);
					SetFontFamily(view);
					SetPlaceholderColor(view);
					SetTextSize(view);
				}
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var view = (CustomPicker)Element;

			if (e.PropertyName == CustomPicker.HasBorderProperty.PropertyName)
				SetBorder(view);
			if (e.PropertyName == CustomPicker.TextAlignmentProperty.PropertyName)
				SetAlignment(view);
			if (e.PropertyName == CustomPicker.TextSizeProperty.PropertyName)
				SetTextSize(view);
			if (e.PropertyName == CustomPicker.PlaceholderColorProperty.PropertyName)
				SetPlaceholderColor(view);
		}

		void SetBorder(CustomPicker view)
		{
			Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;

		}

		void SetTextSize(CustomPicker view)
		{
			if (view.TextSize != default(Double)){
				var newUiFont = UIFont.FromName(Control.Font.FamilyName, (nfloat)view.TextSize);
				Control.Font = newUiFont;
			}
		}

		void SetPlaceholderColor(CustomPicker view)
		{
			Control.AttributedPlaceholder = new Foundation.NSAttributedString(view.Title, new UIStringAttributes
			{
				ForegroundColor = view.PlaceholderColor.ToUIColor()
			});
		}

		void SetFontFamily(CustomPicker view)
		{
			if (!string.IsNullOrWhiteSpace(view.FontFamily))
			{
				var fontSize = Control.Font.PointSize;
				if (view.TextSize != default(Double))
				{
					fontSize = (nfloat)view.TextSize;
				}
				var newUiFont = UIFont.FromName(view.FontFamily, fontSize);
				Control.Font = newUiFont;
			}
		}

		void SetAlignment(CustomPicker view)
		{
			if (view.TextAlignment == Alignment.Left)
				Control.TextAlignment = UITextAlignment.Left;
			if (view.TextAlignment == Alignment.Center)
				Control.TextAlignment = UITextAlignment.Center;
			if (view.TextAlignment == Alignment.Right)
				Control.TextAlignment = UITextAlignment.Right;
		}
	}
}