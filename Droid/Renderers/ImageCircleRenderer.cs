using Android.Graphics;
using Android.Views;
using Java.Lang;
using Solum.Droid.Renderers;
using Solum.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using View = Android.Views.View;
[assembly: ExportRenderer(typeof(ImageCircle), typeof(ImageCircleRenderer))]
namespace Solum.Droid.Renderers
{
    public class ImageCircleRenderer : ImageRenderer
    {
        protected override bool DrawChild(Canvas canvas, View child, long drawingTime)
        {
            try
            {
                var radius = Math.Min(Width, Height) / 2;
                const int strokeWidth = 10;
                radius -= strokeWidth / 2;

                //Create path to clip
                var path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);
                canvas.Save();
                canvas.ClipPath(path);

                var result = base.DrawChild(canvas, child, drawingTime);

                canvas.Restore();

                // Create path for circle border
                path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);

                var paint = new Paint();
                paint.AntiAlias = true;
                paint.StrokeWidth = 5;
                paint.SetStyle(Paint.Style.Stroke);
                paint.Color = Color.White;

                canvas.DrawPath(path, paint);

                //Properly dispose
                paint.Dispose();
                path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create circle image: " + ex);
            }

            return base.DrawChild(canvas, child, drawingTime);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null) return;
            if ((int)Android.OS.Build.VERSION.SdkInt < 18)
                SetLayerType(LayerType.Software, null);
        }
    }
}