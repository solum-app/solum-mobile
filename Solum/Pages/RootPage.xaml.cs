using System;
using Xamarin.Forms;

namespace Solum.Pages
{
	public partial class RootPage : MasterDetailPage
	{
		NavigationPage navigationPage;
		Page currentPage;

		public RootPage ()
		{
			InitializeComponent ();

			currentPage = new AnalisesPage();

			navigationPage = new NavigationPage(currentPage){
				BarBackgroundColor = Color.FromHex("#24BE55"),
				BarTextColor = Color.White
			};

			Detail = navigationPage;
		}

		async void OnAnalisesTapped(object sender, EventArgs e){
			if (Device.OS == TargetPlatform.iOS) {
				if (currentPage.GetType() == typeof(AnalisesPage))
				{
					IsPresented = false;
				}
				else {
					currentPage = new AnalisesPage();
					Detail = new NavigationPage(currentPage)
					{
						BarBackgroundColor = Color.FromHex("#24BE55"),
						BarTextColor = Color.White
					};
					IsPresented = false;
				}
			} else {
				var page = new AnalisesPage();
				await navigationPage.Navigation.PushAsync(page);
				navigationPage.Navigation.RemovePage(currentPage);
				currentPage = page;

				IsPresented = false;
			}
		}

		//void OnNovaAnaliseTapped(object sender, EventArgs e)
		//{
		//	if (currentPage.GetType() == typeof(AnalisePage))
		//	{
		//		IsPresented = false;
		//	}
		//	else {
		//		currentPage = new AnalisePage();
		//		Detail = new NavigationPage(currentPage)
		//		{
		//			BarBackgroundColor = Color.FromHex("#24BE55"),
		//			BarTextColor = Color.White
		//		};
		//		IsPresented = false;
		//	}
		//}

		async void OnSobreTapped(object sender, EventArgs e)
		{
			if (Device.OS == TargetPlatform.iOS)
			{
				if (currentPage.GetType() == typeof(SobrePage))
				{
					IsPresented = false;
				}
				else {
					currentPage = new SobrePage();
					Detail = new NavigationPage(currentPage)
					{
						BarBackgroundColor = Color.FromHex("#24BE55"),
						BarTextColor = Color.White
					};
					IsPresented = false;
				}
			}
			else {
				var page = new SobrePage();
				await navigationPage.Navigation.PushAsync(page);
				navigationPage.Navigation.RemovePage(currentPage);
				currentPage = page;

				IsPresented = false;
			}
		}
	}
}

