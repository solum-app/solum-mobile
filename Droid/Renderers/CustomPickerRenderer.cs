using System;
using Android.Graphics;
using Android.Views;
using Solum.Droid;
using Solum.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace Solum.Droid
{
    public class CustomPickerRenderer : PickerRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && e.OldElement == null)
            {
                var view = (CustomPicker)Element;

                SetHasBorder(view);
                SetFontFamily(view);
                SetAlignment(view);
                SetPlaceholderColor(view);
                SetTextSize(view);
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (CustomPicker)Element;

            if (e.PropertyName == CustomPicker.HasBorderProperty.PropertyName)
                SetHasBorder(view);
            if (e.PropertyName == CustomPicker.FontSizeProperty.PropertyName)
                SetTextSize(view);
            if (e.PropertyName == CustomPicker.TextAlignmentProperty.PropertyName)
                SetAlignment(view);
            if (e.PropertyName == CustomPicker.PlaceholderColorProperty.PropertyName)
                SetPlaceholderColor(view);
        }

        void SetHasBorder(CustomPicker view)
        {
            if (!view.HasBorder)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }

        void SetTextSize(CustomPicker view)
        {
            if (view.FontSize != default(Double))
                Control.TextSize = (float)view.FontSize;
        }

        void SetPlaceholderColor(CustomPicker view)
        {
            Control.SetHintTextColor(view.PlaceholderColor.ToAndroid());
        }

        void SetFontFamily(CustomPicker view)
        {
            if (!string.IsNullOrWhiteSpace(view.FontFamily))
            {
                Typeface font;
                try
                {
                    font = Typeface.CreateFromAsset(Forms.Context.Assets, view.FontFamily + ".otf");
                }
                catch (Exception)
                {
                    font = Typeface.Create(view.FontFamily, TypefaceStyle.Normal);
                }

                Control.Typeface = font;
            }
        }

        void SetAlignment(CustomPicker view)
        {
            if (view.TextAlignment == Alignment.Left)
                Control.Gravity = GravityFlags.Left;
            if (view.TextAlignment == Alignment.Center)
                Control.Gravity = GravityFlags.CenterHorizontal;
            if (view.TextAlignment == Alignment.Right)
                Control.Gravity = GravityFlags.Right;
        }
    }
}