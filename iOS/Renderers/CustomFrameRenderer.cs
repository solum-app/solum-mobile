﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.Drawing;

[assembly: ExportRenderer(typeof(Frame), typeof(Solum.iOS.Renderers.CustomFrameRenderer))]
namespace Solum.iOS.Renderers
{
	public class CustomFrameRenderer : FrameRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Frame> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null && e.OldElement == null){
				if (this.Layer.Sublayers != null && this.Layer.Sublayers.Length > 0) {
					var subLayer = this.Layer.Sublayers [0];
					subLayer.CornerRadius = 2.0f;
					subLayer.MasksToBounds = true;
				}
			}

			this.Layer.CornerRadius = 2.0f;
			this.Layer.ShadowColor = UIColor.DarkGray.CGColor;
			this.Layer.ShadowOpacity = 0.4f;
			this.Layer.ShadowRadius = 2.0f;
			this.Layer.ShadowOffset = new SizeF(0, 1);
		}
	}
}

