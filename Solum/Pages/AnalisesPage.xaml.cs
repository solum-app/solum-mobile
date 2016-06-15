using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Solum.Models;
using Solum.Pages;
using System.Collections.ObjectModel;
using Solum.Rederers;
using System.Threading.Tasks;
using System.Linq;

namespace Solum
{
	public partial class AnalisesPage : ContentPage
	{

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
				var fab = new FloatingActionButtonView () {
					ImageName = "ic_add",
					ColorNormal = Color.FromHex ("FFD54F"),
					ColorPressed = Color.FromHex ("E6C047"),
					ColorRipple = Color.FromHex ("FFD54F"),
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
	}
}

