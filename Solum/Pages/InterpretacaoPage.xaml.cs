using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Solum.Models;
using Solum.ViewModel;

namespace Solum.Pages
{
	public partial class InterpretacaoPage : ContentPage
	{
		public InterpretacaoPage (Analise analise)
		{
			BindingContext = new InterpretacaoViewModel (Navigation, analise);
			InitializeComponent ();
		}
	}
}

