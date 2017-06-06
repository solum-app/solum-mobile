using System;
using Xamarin.Forms;

namespace Solum.Renderers
{
    public class FloatingActionButtonView : View
    {
        public delegate void AttachToListViewDelegate(ListView listView);

        public delegate void ShowHideDelegate(bool animate = true);

        public ShowHideDelegate Show { get; set; }
        public ShowHideDelegate Hide { get; set; }
        public Action<object, EventArgs> Clicked { get; set; }

		public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(
			nameof(HasShadow),
			typeof(bool),
			typeof(FloatingActionButtonView),
			true
		);

		public bool HasShadow
		{
			get { return (bool)GetValue(HasShadowProperty); }
			set { SetValue(HasShadowProperty, value); }
		}

		public static readonly BindableProperty ImageNameProperty = BindableProperty.Create(
            nameof(ImageName),
            typeof(string),
			typeof(FloatingActionButtonView),
            string.Empty
		);

        public string ImageName
        {
            get { return (string) GetValue(ImageNameProperty); }
            set { SetValue(ImageNameProperty, value); }
        }

		public static readonly BindableProperty ColorNormalProperty = BindableProperty.Create(
			nameof(ColorNormal),
			typeof(Color),
			typeof(FloatingActionButtonView),
			Color.White
		);

        public Color ColorNormal
        {
            get { return (Color) GetValue(ColorNormalProperty); }
            set { SetValue(ColorNormalProperty, value); }
        }

		public static readonly BindableProperty ColorPressedProperty = BindableProperty.Create(
            nameof(ColorPressed),
			typeof(Color),
			typeof(FloatingActionButtonView),
			Color.White
		);

        public Color ColorPressed
        {
            get { return (Color) GetValue(ColorPressedProperty); }
            set { SetValue(ColorPressedProperty, value); }
        }

		public static readonly BindableProperty ColorRippleProperty = BindableProperty.Create(
			nameof(ColorRipple),
			typeof(Color),
			typeof(FloatingActionButtonView),
			Color.White
		);

        public Color ColorRipple
        {
            get { return (Color) GetValue(ColorRippleProperty); }
            set { SetValue(ColorRippleProperty, value); }
        }

		public static readonly BindableProperty SizeProperty = BindableProperty.Create(
			nameof(Size),
			typeof(FloatingActionButtonSize),
			typeof(FloatingActionButtonView),
			FloatingActionButtonSize.Normal
		);

        public FloatingActionButtonSize Size
        {
            get { return (FloatingActionButtonSize) GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }
    }
}