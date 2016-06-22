using System;
using System.Windows.Input;
using Xamarin.Forms;
namespace Solum.Renderers
{
	public class BindableDatePicker : DatePicker
	{
		public static BindableProperty DateSelectedCommandProperty =
			BindableProperty.Create<BindableDatePicker, ICommand> (
				x => x.DateSelectedCommand, default (ICommand)
			);

		public ICommand DateSelectedCommand {
			get { return (ICommand)this.GetValue (DateSelectedCommandProperty); }
			set { this.SetValue (DateSelectedCommandProperty, value); }
		}

		public BindableDatePicker ()
		{
			this.DateSelected += this.OnDateSelected;
		}

		private void OnDateSelected (object sender, DateChangedEventArgs e)
		{
			if (this.DateSelectedCommand != null && this.DateSelectedCommand.CanExecute (e)) {
				this.DateSelectedCommand.Execute (e.NewDate);
			}
		}
	}
}

