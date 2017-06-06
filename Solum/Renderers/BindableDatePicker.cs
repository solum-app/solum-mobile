using System.Windows.Input;
using Xamarin.Forms;

namespace Solum.Renderers
{
    public class BindableDatePicker : DatePicker
    {
		public static readonly BindableProperty DateSelectedCommandProperty = BindableProperty.Create(
			nameof(BindableDatePicker),
			typeof(ICommand),
			typeof(BindableDatePicker),
			default(ICommand)
		);

        public BindableDatePicker()
        {
            DateSelected += OnDateSelected;
        }

        public ICommand DateSelectedCommand
        {
            get { return (ICommand) GetValue(DateSelectedCommandProperty); }
            set { SetValue(DateSelectedCommandProperty, value); }
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            if (DateSelectedCommand != null && DateSelectedCommand.CanExecute(e))
                DateSelectedCommand.Execute(e.NewDate);
        }

    }
}