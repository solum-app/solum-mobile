using System.Collections.ObjectModel;
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
    public class FazendaDetalhesViewModel : BaseViewModel
    {
        private readonly bool _fromAnalise;
        private readonly IUserDialogs _userDialogs;
        private ICommand _deleteTalhaoCommand;
        private Fazenda _fazenda;
        private bool _hasItems;
        private bool _isLoading;
        private ICommand _itemTappedCommand;
        private ICommand _showEditTalhaoPageCommand;
        private ObservableCollection<Talhao> _talhoes;

        public FazendaDetalhesViewModel(INavigation navigation, string fazendaId, bool fromAnalise) : base(navigation)
        {
            _fromAnalise = fromAnalise;
            AzureService.Instance.FindFazendaAsync(fazendaId)
	            .ContinueWith(async (task) =>
                {
                    Fazenda = task.Result;
					await UpdateTalhoesList();
                });

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

        public Fazenda Fazenda
        {
			get { return _fazenda; }
			set { SetPropertyChanged(ref _fazenda, value); }
        }

        public ObservableCollection<Talhao> Talhoes
        {
			get { return _talhoes; }
			set { SetPropertyChanged(ref _talhoes, value); }
        }


        public ICommand DeleteTalhaoCommand
			=> _deleteTalhaoCommand ?? (_deleteTalhaoCommand = new Command(async(obj) => await DeleteTalhao(obj as Talhao)));

        public ICommand ShowEditTalhaoPageCommand
            => _showEditTalhaoPageCommand ?? (_showEditTalhaoPageCommand = new Command(async(obj) => await ShowEditTalhaoPage(obj as Talhao)));

        public ICommand ItemTappedCommand
            => _itemTappedCommand ?? (_itemTappedCommand = new Command(async(obj) => await SelectTalhao(obj as Talhao)));

		public async Task UpdateTalhoesList()
        {
			if (Fazenda != null)
			{
				IsLoading = true;
				HasItems = IsLoading;
				var talhaos = await AzureService.Instance.ListTalhaoAsync(Fazenda.Id);
				Talhoes = talhaos.Any() ? new ObservableCollection<Talhao>(talhaos) : new ObservableCollection<Talhao>();
				HasItems = talhaos.Any();
				PageTitle = Fazenda.Nome;
				IsLoading = false;
			}
        }

		private async Task SelectTalhao(Talhao talhao)
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                if (_fromAnalise)
                {
                    MessagingCenter.Send(this, MessagesResource.McTalhaoSelecionado, talhao.Id);
                    await Navigation.PopAsync();
                }
                IsBusy = false;
            }
        }

		private async Task ShowEditTalhaoPage(Talhao talhao)
        {
            await Navigation.PushAsync(new TalhaoCadastroPage(talhao.Id));
        }

        public async Task<bool> CanDelete(Talhao talhao)
        {
			return !await AzureService.Instance.TalhaoHasAnalisesAsync(talhao.Id);
        }

		private async Task DeleteTalhao(Talhao talhao)
        {
			var canDelete = await CanDelete(talhao);
            if (canDelete)
            {
                var confirm = await _userDialogs.DisplayAlert(MessagesResource.ExcluirConfirmacao, "Sim", "Não");
                if (confirm && !IsBusy)
				{
                    IsBusy = true;
					await AzureService.Instance.DeleteTalhaoAsync(talhao);
					Talhoes.Remove(talhao);
					HasItems = Talhoes.Any();
                    _userDialogs.ShowToast(MessagesResource.TalhaoRemocaoSucesso);
                    IsBusy = false;
				}
            }
            else
            {
                await _userDialogs.DisplayAlert(MessagesResource.TalhaoRemocaoBloqueio);
            }
        }
    }
}