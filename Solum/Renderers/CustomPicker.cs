using System;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace Solum.Renderers
{
    public class CustomPicker : Picker
    {
        public static readonly BindableProperty HasBorderProperty = BindableProperty.Create(
            nameof(HasBorder),
            typeof(bool),
            typeof(CustomPicker),
            true
        );

        public static readonly BindableProperty TextAlignmentProperty = BindableProperty.Create(
            nameof(TextAlignment),
            typeof(Alignment),
            typeof(CustomPicker),
            Alignment.Left
        );

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
            nameof(FontSize),
            typeof(Double),
            typeof(CustomPicker),
            default(Double)
        );

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
            nameof(PlaceholderColor),
            typeof(Color),
            typeof(CustomPicker),
            Color.Gray
        );

        public static BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(
            nameof(ItemSelectedCommand),
            typeof(ICommand),
            typeof(CustomPicker),
            default(ICommand)
        );

        public static readonly BindableProperty FontProperty =
            BindableProperty.Create("FontFamily", typeof(string), typeof(CustomPicker), null);

        public CustomPicker()
        {
            this.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public string FontFamily
        {
            get { return (string)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

		public double FontSize
        {
            get { return (Double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }

        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public Alignment TextAlignment
        {
            get { return (Alignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)this.GetValue(ItemSelectedCommandProperty); }
            set { this.SetValue(ItemSelectedCommandProperty, value); }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var picker = sender as CustomPicker;
            if (picker.ItemSelectedCommand != null && picker.ItemSelectedCommand.CanExecute(picker.SelectedIndex))
			{
                picker.ItemSelectedCommand.Execute(picker.SelectedIndex);
			}
        }
    }

    public enum Alignment
    {
        Center,
        Right,
        Left
    }
}