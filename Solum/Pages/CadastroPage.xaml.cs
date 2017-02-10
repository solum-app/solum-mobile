﻿using System;
using System.ComponentModel;
using Solum.ViewModel;
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
        }

        private void NomeEntry_OnCompleted(object sender, EventArgs e)
        {
            EmailEntry.Focus();
        }

        private void EmailEntry_OnCompleted(object sender, EventArgs e)
        {
            SenhaEntry.Focus();
        }

        private void SenhaEntry_OnCompleted(object sender, EventArgs e)
        {
            ConfirmarSenhaEntry.Focus();
        }

        private void ConfirmarSenhaEntry_OnCompleted(object sender, EventArgs e)
        {
            EstadosPicker.Focus();
        }
    }
}