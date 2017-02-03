﻿using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class RecomendaSemeaduraPage : ContentPage
    {
        public RecomendaSemeaduraPage(string analiseId, int expectativaSelected, string culturaSelected)
        {
            InitializeComponent();
            BindingContext = new RecomendacaoSemeaduraViewModel(Navigation, analiseId, expectativaSelected, culturaSelected);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }
    }
}