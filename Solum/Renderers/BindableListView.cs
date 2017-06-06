using System.Windows.Input;
using Xamarin.Forms;

namespace Solum.Renderers
{
    public class BindableListView : ListView
    {
        public static readonly BindableProperty ItemClickedCommandProperty = BindableProperty.Create(
            nameof(ItemClickedCommand),
            typeof(ICommand),
            typeof(BindableListView),
            default(ICommand)
        );
        
        public BindableListView()
        {
            ItemTapped += OnItemTapped;
        }

        public BindableListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            ItemTapped += OnItemTapped;
        }

        public ICommand ItemClickedCommand
        {
            get { return (ICommand) GetValue(ItemClickedCommandProperty); }
            set { SetValue(ItemClickedCommandProperty, value); }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && ItemClickedCommand != null && ItemClickedCommand.CanExecute(e))
            {
                ItemClickedCommand.Execute(e.Item);
                SelectedItem = null;
            }
        }
    }
}