using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Solum.Models;
using Solum.Pages;
using System.Linq;
using System.Collections.ObjectModel;
using Solum.Rederers;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;

namespace Solum
{
	public partial class AnalisesPage : ContentPage
	{
		readonly FloatingActionButtonView fab;
		int appearingListItemIndex = 0;

		public AnalisesPage ()
		{
			NavigationPage.SetBackButtonTitle(this, "Voltar");

			InitializeComponent ();

			var list = new List<Analise>();

			for (int i = 0; i < 5; i++)
			{
				var analise = new Analise();
				analise.Fazenda = "Fazenda Santo Augustinho";
				analise.Talhao = "Talhão " + (i + 1);
				analise.Data = DateTime.Now;

				list.Add(analise);

				var analise2 = new Analise();
				analise2.Fazenda = "Fazenda Esperança";
				analise2.Talhao = "Talhão " + (i + 1);
				analise2.Data = DateTime.Now;

				list.Add(analise2);
			}

			var groupList =
				list.OrderBy (a => a.Fazenda)
					.GroupBy (a => a.Fazenda).ToList ();
			
			analisesList.ItemsSource = new ObservableCollection<IGrouping<string, Analise>>(groupList);

			if (Device.OS == TargetPlatform.Android) {
				fab = new FloatingActionButtonView () {
					ImageName = "ic_menu",
					ColorNormal = Color.FromHex ("FFC107"),
					ColorPressed = Color.FromHex ("E6AE07"),
					ColorRipple = Color.FromHex ("FFC107"),
					Clicked = async (sender, args) => 
						await Navigation.PushAsync (new AnalisePage ())
				};

				// Overlay the FAB in the bottom-right corner
				AbsoluteLayout.SetLayoutFlags (fab, AbsoluteLayoutFlags.PositionProportional);
				AbsoluteLayout.SetLayoutBounds (fab, new Rectangle (1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
				absolute.Children.Add (fab);
			} else {
				var item = new ToolbarItem ("Add", "ic_add", async () => await Navigation.PushAsync (new AnalisePage ()));
				this.ToolbarItems.Add (item);
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (Device.OS == TargetPlatform.Android) {
				analisesList.ItemAppearing += List_ItemAppearing;
				analisesList.ItemDisappearing += List_ItemDisappearing;
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			if (Device.OS == TargetPlatform.Android) {
				analisesList.ItemAppearing -= List_ItemAppearing;
				analisesList.ItemDisappearing -= List_ItemDisappearing;
			}
		}

		async void List_ItemDisappearing (object sender, ItemVisibilityEventArgs e)
		{
			await Task.Run(() =>
				{
					var source = analisesList.ItemsSource as List<IGrouping<string, Analise>>;

					if (source != null){
						var items = source.SelectMany (o => o).ToList ();

						if(items != null)
						{
							var index = items.IndexOf((Analise)e.Item);
							if (index < appearingListItemIndex)
							{
								Device.BeginInvokeOnMainThread(() => fab.Hide());
							}
							appearingListItemIndex = index;
						}
					}
				}
			);
		}

		async void List_ItemAppearing (object sender, ItemVisibilityEventArgs e)
		{
			await Task.Run(() =>
				{
					var source = analisesList.ItemsSource as List<IGrouping<string, Analise>>;

					if (source != null){
						var items = source.SelectMany (o => o).ToList ();

						if(items != null)
						{
							var index = items.IndexOf((Analise)e.Item);
							if (index < appearingListItemIndex)
							{
								Device.BeginInvokeOnMainThread(() => fab.Show());
							}
							appearingListItemIndex = index;
						}
					}
				}
			);
		}
	}
}

