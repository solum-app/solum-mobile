﻿using System;
using Solum.Handlers;
using Solum.Models;
using Solum.Renderers;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class FazendaListPage : ContentPage
    {
        public FazendaListPage(bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new FazendaListViewModel(Navigation, fromAnalise);
            NavigationPage.SetBackButtonTitle(this, "Voltar");

            if (Device.OS == TargetPlatform.Android)
            {
                var fab = new FloatingActionButtonView
                {
                    ImageName = "ic_add",
                    ColorNormal = Color.FromHex("FFD54F"),
                    ColorPressed = Color.FromHex("E6C047"),
                    ColorRipple = Color.FromHex("FFD54F"),
                    Clicked = async (sender, args) =>
                    {
                        if (!IsBusy)
                        {
                            IsBusy = true;
                            await Navigation.PushAsync(new FazendaCadastroPage(fromAnalise));
                            if (fromAnalise)
                                Navigation.RemovePage(this);
                            IsBusy = false;
                        }
                    }
                };
                AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(fab,
                    new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                absoluteLayout.Children.Add(fab);
            }
            else
            {
                var item = new ToolbarItem("Add", "ic_add", async () =>
                {
                    if (!IsBusy)
                    {
                        IsBusy = true;
                        await Navigation.PushAsync(new FazendaCadastroPage(fromAnalise));
                        if (fromAnalise)
                            Navigation.RemovePage(this);
                        IsBusy = false;
                    }
                });
                ToolbarItems.Add(item);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as FazendaListViewModel)?.UpdateFazendaList();
        }

        private void OnEdit(object sender, EventArgs e)
        {
            var fazenda = (sender as MenuItem)?.CommandParameter;
            var context = BindingContext as FazendaListViewModel;
            context?.EditCommand.Execute(fazenda);
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            var fazenda = (sender as MenuItem)?.CommandParameter;
            var context = BindingContext as FazendaListViewModel;
            var canDelete = await context?.CanDelete((fazenda as Fazenda)?.Id);
            if (canDelete)
            {
                var confirm = await DisplayAlert("Confirmação", "Tem certeza que deseja excluir este item?", "Sim",
                    "Não");
                if (confirm)
                    context?.DeleteCommand.Execute(fazenda);
            }
            else
            {
                "Essa fazenda não pode ser removida, existem análises atreladas à ela.\nRemova as análises primeiro"
                    .ToDisplayAlert(MessageType.Aviso);
            }
        }
    }
}