﻿using System;
using Solum.Models;
using Solum.Renderers;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class FazendaDetalhesPage : ContentPage
    {
        public FazendaDetalhesPage(Fazenda fazenda)
        {
            InitializeComponent();
            BindingContext = new FazendaDetalhesViewModel(Navigation, fazenda);
            if (Device.OS == TargetPlatform.Android)
            {
                var fab = new FloatingActionButtonView
                {
                    ImageName = "ic_add",
                    ColorNormal = Color.FromHex("FFD54F"),
                    ColorPressed = Color.FromHex("E6C047"),
                    ColorRipple = Color.FromHex("FFD54F"),
                    Clicked = async (sender, args) =>
                        await Navigation.PushAsync(new TalhaoCadastroPage())
                };

                // Overlay the FAB in the bottom-right corner
                AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(fab,
                    new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                absoluteLayout.Children.Add(fab);
            }
            else
            {
                var item = new ToolbarItem("Add", "ic_add", async () => await Navigation.PushAsync(new AnalisePage()));
                ToolbarItems.Add(item);
            }
        }

        private void OnEdit(object sender, EventArgs e)
        {
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            //var confirm = await DisplayAlert("Confirmação", "Tem certeza que deseja excluir este item?", "Sim", "Não");

            //if (confirm)
            //{
            //    var analise = (sender as MenuItem).CommandParameter;
            //    (BindingContext as AnalisesViewModel).ExcluirCommand.Execute(analise);
            //}
        }
    }
}