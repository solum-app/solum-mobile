using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Handlers;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class FazendaDetalhesViewModel : BaseViewModel
    {
        private readonly bool _fromAnalise;
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
                .ContinueWith(t =>
                {
                    Fazenda = t.Result;
                    UpdateTalhoesList();
                });
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetPropertyChanged(ref _isLoading, value);
        }


        public bool HasItems
        {
            get => _hasItems;
            set => SetPropertyChanged(ref _hasItems, value);
        }

        public Fazenda Fazenda
        {
            get => _fazenda;
            set => SetPropertyChanged(ref _fazenda, value);
        }

        public ObservableCollection<Talhao> Talhoes
        {
            get => _talhoes;
            set => SetPropertyChanged(ref _talhoes, value);
        }


        public ICommand DeleteTalhaoCommand
            => _deleteTalhaoCommand ?? (_deleteTalhaoCommand = new Command(obj => DeleteTalhao(obj as Talhao)));

        public ICommand ShowEditTalhaoPageCommand
            =>
                _showEditTalhaoPageCommand ??
                (_showEditTalhaoPageCommand = new Command(obj => ShowEditTalhaoPage(obj as Talhao)));

        public ICommand ItemTappedCommand
            => _itemTappedCommand ?? (_itemTappedCommand = new Command(obj => SelectTalhao(obj as Talhao)));

        public async void UpdateTalhoesList()
        {
            IsLoading = true;
            HasItems = IsLoading;
            var talhaos = await AzureService.Instance.ListTalhaoAsync(Fazenda.Id);
            Talhoes = talhaos.Any() ? new ObservableCollection<Talhao>(talhaos) : new ObservableCollection<Talhao>();
            HasItems = talhaos.Any();
            PageTitle = Fazenda.Nome;
            IsLoading = false;
        }

        private async void SelectTalhao(Talhao talhao)
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

        private async void ShowEditTalhaoPage(Talhao talhao)
        {
            await Navigation.PushAsync(new TalhaoCadastroPage(talhao.Id));
        }

        public async Task<bool> CanDelete(string talhaoid)
        {
            return !await AzureService.Instance.TalhaoHasAnalisesAsync(talhaoid);
        }

        private async void DeleteTalhao(Talhao talhao)
        {
            await AzureService.Instance.DeleteTalhaoAsync(talhao);
            Talhoes.Remove(talhao);
            HasItems = Talhoes.Any();
            MessagesResource.TalhaoRemocaoSucesso.ToToast();
        }
    }
}