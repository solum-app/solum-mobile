﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class FazendaListViewModel : BaseViewModel
    {
        private readonly bool _fromAnalise;
        private readonly IUserDialogs _userDialogs;
        private ICommand _deleteCommand;
        private ICommand _editCommand;
        private ICommand _itemTappedCommand;
        private ObservableCollection<Fazenda> _fazendas;
        private bool _hasItems;
        private bool _isLoading;

        public FazendaListViewModel(INavigation navigation, bool fromAnalise) : base(navigation)
        {
            IsLoading = true;
            HasItems = IsLoading;
            LoadFazendaList();
            _fromAnalise = fromAnalise;
            _userDialogs = DependencyService.Get<IUserDialogs>();

        }

        public bool IsLoading
        {
			get { return _isLoading; }
			set { SetPropertyChanged(ref _isLoading, value); }
        }


        public bool HasItems
        {
			get { return _hasItems; }
			set { SetPropertyChanged(ref _hasItems, value); }
        }

        public ObservableCollection<Fazenda> Fazendas
        {
			get { return _fazendas; }
			set { SetPropertyChanged(ref _fazendas, value); }
        }


        public ICommand EditCommand 
        => _editCommand ?? (_editCommand = new Command(async(fazenda) => await Edit(fazenda as Fazenda)));

        public ICommand DeleteCommand
        => _deleteCommand ?? (_deleteCommand = new Command(async(fazenda) => await Delete(fazenda as Fazenda)));

        public ICommand ItemTappedCommand
        => _itemTappedCommand ?? (_itemTappedCommand = new Command(async(fazenda) => await Details(fazenda as Fazenda)));


		private async Task Details(Fazenda fazenda)
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                if (_fromAnalise)
                {
                    MessagingCenter.Send(this, MessagesResource.McFazendaSelecionada, fazenda.Id);
                    await Navigation.PopAsync();
                }
                else
                {
                    await Navigation.PushAsync(new FazendaDetalhesPage(fazenda.Id, _fromAnalise));
                }
                IsBusy = false;
            }
        }

		private async Task Edit(Fazenda fazenda)
        {
            var current = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(new FazendaCadastroPage(fazenda.Id, _fromAnalise));
            if (_fromAnalise)
                Navigation.RemovePage(current);
        }

		private async Task Delete(Fazenda fazenda)
        {
            if (await CanDelete(fazenda.Id))
			{
                var confirm = await _userDialogs.DisplayAlert(MessagesResource.ExcluirConfirmacao, "Sim", "Não");
                if (confirm & !IsBusy) {
                    IsBusy = true;
					await AzureService.Instance.DeleteFazendaAsync(fazenda);
					Fazendas.Remove(fazenda);
					HasItems = Fazendas.Any();
                    _userDialogs.ShowToast(MessagesResource.FazendaRemocaoSucesso);
                    IsBusy = false;
                }
			}
			else
			{
                await _userDialogs.DisplayAlert(MessagesResource.FazendaRemocaoBloqueio);;
			}
        }

        public async Task<bool> CanDelete(string fazendaId)
        {
            var talhoes = await AzureService.Instance.ListTalhaoAsync(fazendaId);
            foreach (var t in talhoes)
                if (await AzureService.Instance.TalhaoHasAnalisesAsync(t.Id))
                    return false;
            return true;
        }

        public void LoadFazendaList()
        {
            IsLoading = true;
            AzureService.Instance.ListFazendaAsync()
                .ContinueWith(t =>
                {
                    var r = t.Result;
                    Fazendas = r.Any() ? new ObservableCollection<Fazenda>(r) : new ObservableCollection<Fazenda>();
                    HasItems = Fazendas.Any();
                    IsLoading = false;
                });
        }
    }
}