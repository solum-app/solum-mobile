using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Solum.ViewModel;

namespace Solum.Pages
{
	public partial class AnalisePage : ContentPage
	{
		public AnalisePage ()
		{
			NavigationPage.SetBackButtonTitle(this, "Voltar");

			InitializeComponent ();
			BindingContext = new AnaliseViewModel (Navigation);
		}
	}
}

