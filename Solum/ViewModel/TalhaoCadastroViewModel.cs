using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Auth;
using Solum.Handlers;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class TalhaoCadastroViewModel : BaseViewModel
    {
        private readonly bool _fromAnalise;
        private readonly bool _isUpdate;
        private Fazenda _fazenda;
        private ICommand _saveCommand;
        private Talhao _talhao;
        private string _talhaoArea;
        private string _talhaoName;

        public TalhaoCadastroViewModel(INavigation navigation, string fazendaId, bool fromAnalise) : base(navigation)
        {
            PageTitle = "Novo Talhão";
            _isUpdate = false;
            _fromAnalise = fromAnalise;
            AzureService.Instance.FindFazendaAsync(fazendaId).ContinueWith(t => { Fazenda = t.Result; });
        }

        public TalhaoCadastroViewModel(INavigation navigation, string talhaoId) : base(navigation)
        {
            PageTitle = "Editar Talhão";
            _isUpdate = true;
            AzureService.Instance.FindTalhaoAsync(talhaoId)
                .ContinueWith(t =>
                {
                    Talhao = t.Result;
                    TalhaoName = Talhao.Nome;
                    TalhaoArea = Talhao.Area;
                    AzureService.Instance.FindFazendaAsync(Talhao.FazendaId).ContinueWith(s => { Fazenda = s.Result; });
                });
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

		public Fazenda Fazenda
		{
			get { return _fazenda; }
			set { SetPropertyChanged(ref _fazenda, value); }
		}

		public Talhao Talhao
		{
			get { return _talhao; }
			set { SetPropertyChanged(ref _talhao, value); }
		}

        private async Task Save()
        {
            if (!IsNotBusy) return;

            IsBusy = true;

            if (!_isUpdate)
            {
                if (string.IsNullOrEmpty(TalhaoName))
                {
                    MessagesResource.TalhaoCadastroNomeVazio.ToDisplayAlert();
                    IsBusy = false;
                    return;
                }

                if (!string.IsNullOrEmpty(TalhaoArea))
                    TalhaoArea = TalhaoArea.Replace("ha", "").Trim();
                var userId = await DependencyService.Get<IAuthentication>().UserId();
                var novo = new Talhao
                {
                    Id = Guid.NewGuid().ToString(),
                    UsuarioId = userId,
                    FazendaId = Fazenda.Id,
                    Area = TalhaoArea,
                    Nome = TalhaoName,
                    HasArea = !string.IsNullOrEmpty(TalhaoArea)
                };

                await AzureService.Instance.InsertTalhaoAsync(novo);
                if (!_fromAnalise)
                    MessagesResource.TalhaoCadastroSucesso.ToToast();
                else
                    MessagingCenter.Send(this, MessagesResource.McTalhaoSelecionado, novo.Id);
                await Navigation.PopAsync();
                IsBusy = false;
            }

            else
            {
                if (string.IsNullOrEmpty(TalhaoName))
                {
                    MessagesResource.TalhaoCadastroNomeVazio.ToDisplayAlert(MessageType.Info);
                    IsBusy = false;
                    return;
                }
                if (!string.IsNullOrEmpty(TalhaoArea))
                    TalhaoArea = TalhaoArea.Replace("ha", "").Trim();

                Talhao.Nome = TalhaoName;
                Talhao.Area = TalhaoArea;
                Talhao.HasArea = !string.IsNullOrEmpty(Talhao.Area);
                await AzureService.Instance.UpdateTalhaoAsync(Talhao);
                if (!_fromAnalise)
                    MessagesResource.TalhaoCadastroSucesso.ToToast();
                else
                    MessagingCenter.Send(this, MessagesResource.McTalhaoSelecionado, Talhao.Id);
                await Navigation.PopAsync();
                IsBusy = false;
            }
        }
    }
}