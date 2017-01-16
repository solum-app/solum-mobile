﻿using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class CadastroPage : ContentPage
    {
        public CadastroPage()
        {
            InitializeComponent();
            BindingContext = new CadastroViewModel(Navigation);
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            MessagingCenter.Subscribe<CadastroViewModel, string>(this, "NullEntrys",
                (view, arg) => { ShowErrorMessage(arg); });
            MessagingCenter.Subscribe<CadastroViewModel, string>(this, "RegisterSuccessful",
                (view, arg) => { ShowSuccessMessage(arg); });
            MessagingCenter.Subscribe<CadastroViewModel, string>(this, "RegisterUnsuccessful",
                (view, arg) => { ShowErrorMessage(arg); });
            MessagingCenter.Subscribe<CadastroViewModel, string>(this, "CidadeNull",
             (view, arg) => { ShowErrorMessage(arg); });
        }

        private async void ShowErrorMessage(string message)
        {
            await DisplayAlert("Erro!", message, "Ok");
        }

        private async void ShowSuccessMessage(string message)
        {
            await DisplayAlert("Sucesso!", message, "OK");
        }
    }
}