using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Auth;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class TalhaoCadastroViewModel : BaseViewModel
    {
        private readonly bool _fromAnalise;
        private readonly IUserDialogs _userDialogs;
        private Fazenda _fazenda;
        private ICommand _saveCommand;
        private Talhao _talhao;
        private string _talhaoArea;
        private string _talhaoName;

        public TalhaoCadastroViewModel(INavigation navigation, string fazendaId, bool fromAnalise) : base(navigation)
        {
            PageTitle = "Novo Talhão";
            _fromAnalise = fromAnalise;
            AzureService.Instance.FindFazendaAsync(fazendaId).ContinueWith(t => { _fazenda = t.Result; });
            _userDialogs = DependencyService.Get<IUserDialogs>();
        }

        public TalhaoCadastroViewModel(INavigation navigation, string talhaoId) : base(navigation)
        {
            PageTitle = "Editar Talhão";
            AzureService.Instance.FindTalhaoAsync(talhaoId)
                .ContinueWith(async(task) =>
                {
                    _talhao = task.Result;
                    TalhaoName = _talhao.Nome;
                    TalhaoArea = _talhao.Area;
                    _fazenda = await AzureService.Instance.FindFazendaAsync(_talhao.FazendaId);
                });
            _userDialogs = DependencyService.Get<IUserDialogs>();
        }


        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new Command(async ()=> await Save()));


        public string TalhaoName
        {
            get { return _talhaoName; }
            set { SetPropertyChanged(ref _talhaoName, value); }
        }

        public string TalhaoArea
        {
            get { return _talhaoArea; }
            set { SetPropertyChanged(ref _talhaoArea, value); }
        }

		private async Task Save()
		{
			if (string.IsNullOrEmpty(TalhaoName))
			{
                await _userDialogs.DisplayAlert(MessagesResource.TalhaoCadastroNomeVazio);
				return;
			}

            IsBusy = true;

			if (!string.IsNullOrEmpty(TalhaoArea))
				TalhaoArea = TalhaoArea.Replace("ha", "").Trim();

            bool isUpdate = _talhao != default(Talhao);
            _talhao = _talhao ?? new Talhao();

            //_talhao.UsuarioId = await DependencyService.Get<IAuthentication>().UserId();
			_talhao.FazendaId = _fazenda.Id;
            _talhao.Nome = TalhaoName;
            _talhao.Area = TalhaoArea;

            await AzureService.Instance.AddOrUpdateTalhaoAsync(_talhao);

            if (_fromAnalise)
                MessagingCenter.Send(this, MessagesResource.McTalhaoSelecionado, _talhao.Id);
            else if (isUpdate)
                _userDialogs.ShowToast(MessagesResource.TalhaoEdicaoSucesso);
			else
				_userDialogs.ShowToast(MessagesResource.TalhaoCadastroSucesso);
			
			await Navigation.PopAsync();

			IsBusy = false;
		}
    }
}