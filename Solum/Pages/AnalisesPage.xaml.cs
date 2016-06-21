using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Solum.Models;
using Solum.Pages;
using System.Collections.ObjectModel;
using Solum.Rederers;
using System.Threading.Tasks;
using System.Linq;
using Solum.ViewModel;

namespace Solum
{
	public partial class AnalisesPage : ContentPage
	{

		public AnalisesPage ()
		{
			NavigationPage.SetBackButtonTitle(this, "Voltar");

			InitializeComponent ();

			BindingContext = new AnalisesViewModel (Navigation);

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

		void OnEdit(object sender, EventArgs e){
			var analise = (sender as MenuItem).CommandParameter;
			(BindingContext as AnalisesViewModel).EditarCommand.Execute (analise);
		}

		void OnDelete (object sender, EventArgs e)
		{
			var analise = (sender as MenuItem).CommandParameter;
			(BindingContext as AnalisesViewModel).ExcluirCommand.Execute (analise);
		}
	}
}

