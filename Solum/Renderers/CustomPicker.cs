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

		public static readonly BindableProperty TextSizeProperty = BindableProperty.Create(
			nameof(TextSize),
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

		public static BindableProperty ItemsSourceProperty = BindableProperty.Create(
			nameof(ItemsSource),
			typeof(IEnumerable),
			typeof(CustomPicker),
			default(IEnumerable),
			propertyChanged: OnItemsSourceChanged
		);

		public static BindableProperty SelectedItemProperty = BindableProperty.Create(
			nameof(SelectedItem),
			typeof(object),
			typeof(CustomPicker),
			default(object),
			BindingMode.TwoWay,
			propertyChanged: OnSelectedItemChanged
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

		public Double TextSize
		{
			get { return (Double)GetValue(TextSizeProperty); }
			set { SetValue(TextSizeProperty, value); }
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

		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public object SelectedItem
		{
			get { return (object)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		private static void OnItemsSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			var picker = bindable as CustomPicker;
			picker.Items.Clear();
			if (newvalue != null)
			{
				foreach (var item in (newvalue as IEnumerable))
				{
					picker.Items.Add(item.ToString());
				}
			}
		}

		private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
			{
				SelectedItem = null;
			}
			else {
				var items = this.ItemsSource as IList;
				SelectedItem = items[SelectedIndex];
			}
		}

		private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			var picker = bindable as CustomPicker;
			if (newvalue != null)
			{
				picker.SelectedIndex = picker.Items.IndexOf(newvalue.ToString());

				var items = picker.ItemsSource as IList;
				if (picker.ItemSelectedCommand != null && picker.ItemSelectedCommand.CanExecute(items[picker.SelectedIndex]))
				{
					picker.ItemSelectedCommand.Execute(items[picker.SelectedIndex]);
				}
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