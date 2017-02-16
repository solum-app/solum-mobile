using System.Windows.Input;
using Xamarin.Forms;

namespace Solum.Renderers
{
    public class BindableDatePicker : DatePicker
    {
        public static BindableProperty DateSelectedCommandProperty =
#pragma warning disable CS0618 // Type or member is obsolete
            BindableProperty.Create<BindableDatePicker, ICommand>(
                x => x.DateSelectedCommand, default(ICommand)
            );
#pragma warning restore CS0618 // Type or member is obsolete

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