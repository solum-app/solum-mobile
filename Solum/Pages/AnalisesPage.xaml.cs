using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Solum.Models;
using Solum.Pages;
using System.Linq;
using Solum.Helpers;
using System.Collections.ObjectModel;

namespace Solum
{
	public partial class AnalisesPage : ContentPage
	{
		public AnalisesPage ()
		{
			NavigationPage.SetBackButtonTitle(this, "Voltar");

			InitializeComponent ();

			if (Device.OS == TargetPlatform.iOS) {
				var item = new ToolbarItem ("Add", "ic_add", async () => await Navigation.PushAsync (new AnalisePage ()));
				this.ToolbarItems.Add (item);
			}

			var list = new List<Analise>();

			for (int i = 0; i < 5; i++)
			{
				var analise = new Analise();
				analise.Fazenda = "Fazendinha";
				analise.Talhao = "Talhão 1";
				analise.Data = DateTime.Now;

				list.Add(analise);

				var analise2 = new Analise();
				analise2.Fazenda = "Fazendão";
				analise2.Talhao = "Talhão 4";
				analise2.Data = DateTime.Now;

				list.Add(analise2);
			}

			var groupList =
				list.OrderBy (a => a.Fazenda)
					.GroupBy (a => a.Fazenda)
					.Select (a => new Grouping<string, Analise> (a.Key, a));
			
			analisesList.ItemsSource = new ObservableCollection<Grouping<string, Analise>>(groupList);
		}
	}
}

