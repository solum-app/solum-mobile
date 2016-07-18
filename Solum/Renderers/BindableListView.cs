using System;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace Solum.Renderers
{
	public class BindableListView : ListView
	{
		public static BindableProperty ItemClickedCommandProperty = BindableProperty.Create<BindableListView, ICommand> (x => x.ItemClickedCommand, default (ICommand));

		public static BindableProperty LoadCommandProperty = BindableProperty.Create<BindableListView, ICommand> (x => x.LoadCommand, default (ICommand));

		public BindableListView ()
		{
			this.ItemTapped += this.OnItemTapped;
			this.ItemAppearing += this.OnItemAppearing;
		}

		public BindableListView (ListViewCachingStrategy strategy) : base (strategy)
		{
			this.ItemTapped += this.OnItemTapped;
			this.ItemAppearing += this.OnItemAppearing;
		}

		public ICommand ItemClickedCommand {
			get { return (ICommand)this.GetValue (ItemClickedCommandProperty); }
			set { this.SetValue (ItemClickedCommandProperty, value); }
		}


		private void OnItemTapped (object sender, ItemTappedEventArgs e)
		{
			if (e.Item != null && this.ItemClickedCommand != null && this.ItemClickedCommand.CanExecute (e)) {
				this.ItemClickedCommand.Execute (e.Item);
				this.SelectedItem = null;
			}
		}

		public ICommand LoadCommand {
			get { return (ICommand)this.GetValue (LoadCommandProperty); }
			set { this.SetValue (LoadCommandProperty, value); }
		}

		private void OnItemAppearing (object sender, ItemVisibilityEventArgs e)
		{
			var items = this.ItemsSource as IList;

			if (items != null && e.Item == items [items.Count - 1]) {
				if (this.LoadCommand != null && this.LoadCommand.CanExecute (this)) {
					this.LoadCommand.Execute (this);
				}
			}
		}
	}
}

