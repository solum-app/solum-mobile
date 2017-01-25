using Xamarin.Forms;
using Solum.ViewModel;
using Solum.Models;
using System;

namespace Solum.Pages
{
	public partial class AnalisePage : ContentPage
	{
		public AnalisePage ()
		{
			BindingContext = new AnaliseViewModel (Navigation);
			Init ();
		}

		public AnalisePage (Analise analise)
		{
			BindingContext = new AnaliseViewModel (Navigation, analise);
			Init ();
		}

		void Init(){
			NavigationPage.SetBackButtonTitle (this, "Voltar");

			InitializeComponent ();

			//fazendaEntry.Completed += (s, e) => talhaoEntry.Focus ();
			//talhaoEntry.Completed += (s, e) => datePicker.Focus ();
			//phEntry.Completed += (s, e) => pEntry.Focus ();
			//pEntry.Completed += (s, e) => kEntry.Focus ();
			//kEntry.Completed += (s, e) => caEntry.Focus ();
			//caEntry.Completed += (s, e) => mgEntry.Focus ();
			//mgEntry.Completed += (s, e) => alEntry.Focus ();
			//alEntry.Completed += (s, e) => hEntry.Focus ();
			//hEntry.Completed += (s, e) => moEntry.Focus ();
			//moEntry.Completed += (s, e) => areiaEntry.Focus ();
			//areiaEntry.Completed += (s, e) => sliteEntry.Focus ();
			//sliteEntry.Completed += (s, e) => argilaEntry.Focus ();
		}

		void OnEntryFocused (object sender, EventArgs e){
			if (Device.OS == TargetPlatform.Android)
				boxSpacing.IsVisible = !boxSpacing.IsVisible;
		}

		void OnEntryUnfocused(object sender, EventArgs e)
		{
			if (Device.OS == TargetPlatform.Android)
				boxSpacing.IsVisible = false;
		}
	}
}

