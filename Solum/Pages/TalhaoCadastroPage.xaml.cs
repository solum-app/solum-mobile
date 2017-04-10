﻿using System;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class TalhaoCadastroPage : ContentPage
    {
        public TalhaoCadastroPage(string fazendaId, bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new TalhaoCadastroViewModel(Navigation, fazendaId, fromAnalise);
            NavigationPage.SetBackButtonTitle(this, "Voltar");
        }

        public TalhaoCadastroPage(string talhaoId)
        {
            InitializeComponent();
            BindingContext = new TalhaoCadastroViewModel(Navigation, talhaoId);
            NavigationPage.SetBackButtonTitle(this, "Voltar");
        }

        private void NomeEntryOnCompleted(object sender, EventArgs e)
        {
            AreaEntry.Focus();
        }

        private void AreaEntry_OnFocused(object sender, FocusEventArgs e)
        {
            var text = ((Entry) sender).Text;
            if (!string.IsNullOrEmpty(text))
                ((Entry) sender).Text = text.Replace("ha", "").Trim();
        }

        private void AreaEntry_OnUnfocused(object sender, FocusEventArgs e)
        {
            var text = ((Entry)sender).Text;
            if (!string.IsNullOrEmpty(text))
                ((Entry) sender).Text = $"{text} ha";
        }
    }
}