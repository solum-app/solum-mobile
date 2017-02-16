using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace Solum.Renderers
{
    public class BindableListView : ListView
    {
        public BindableListView()
        {
            ItemTapped += OnItemTapped;
            ItemAppearing += OnItemAppearing;
        }

        public BindableListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            ItemTapped += OnItemTapped;
            ItemAppearing += OnItemAppearing;
        }

        public ICommand ItemClickedCommand
        {
            get { return (ICommand) GetValue(ItemClickedCommandProperty); }
            set { SetValue(ItemClickedCommandProperty, value); }
        }

        public ICommand LoadCommand
        {
            get { return (ICommand) GetValue(LoadCommandProperty); }
            set { SetValue(LoadCommandProperty, value); }
        }


        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && ItemClickedCommand != null && ItemClickedCommand.CanExecute(e))
            {
                ItemClickedCommand.Execute(e.Item);
                SelectedItem = null;
            }
        }

        private void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = ItemsSource as IList;

            if (items != null && e.Item == items[items.Count - 1])
                if (LoadCommand != null && LoadCommand.CanExecute(this)) LoadCommand.Execute(this);
        }
#pragma warning disable CS0618 // Type or member is obsolete
        public static BindableProperty ItemClickedCommandProperty =
            BindableProperty.Create<BindableListView, ICommand>(x => x.ItemClickedCommand, default(ICommand));

        public static BindableProperty LoadCommandProperty =
            BindableProperty.Create<BindableListView, ICommand>(x => x.LoadCommand, default(ICommand));
#pragma warning restore CS0618 // Type or member is obsolete
    }
}