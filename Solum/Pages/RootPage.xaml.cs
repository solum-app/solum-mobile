using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Solum
{
	public partial class RootPage : MasterDetailPage
	{
		public RootPage ()
		{
			InitializeComponent ();

			Detail = new NavigationPage(new AnalisesPage ()){
				BarBackgroundColor = Color.FromHex("#86E2D5"),
				BarTextColor = Color.Black
			};
		}
	}
}

