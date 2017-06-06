using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Auth;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class AnaliseViewModel : BaseViewModel
    {
		private Color _fazendaLabelColor;
		private Color _talhaoLabelColor;
		private DateTimeOffset _data = DateTimeOffset.Now;
		private string _fazendaNome;
		private string _talhaoNome;
		private string _identificacaoAnalise;
		private string _potencialHidrogenico;
		private string _fosforo;
		private string _potassio;
		private string _calcio;
		private string _magnesio;
		private string _aluminio;
		private string _hidrogenio;
		private string _materiaOrganica;
		private string _areia;
		private string _silte;
		private string _argila;
		private Fazenda _fazenda;
		private Talhao _talhao;
		private Analise _analise;
		private ICommand _selectDateCommand;
		private ICommand _showFazendasCommand;
		private ICommand _showTalhoesCommand;
		private ICommand _saveCommand;
        private readonly IUserDialogs _userDialogs;

        public AnaliseViewModel(INavigation navigation) : base(navigation)
        {
            PageTitle = "Nova Análise";
            FazendaNome = "Selecione uma fazenda";
            TalhaoNome = "Selecione um talhão";
            FazendaLabelColor = (Color) Application.Current.Resources["colorTextHint"];
            TalhaoLabelColor = (Color) Application.Current.Resources["colorTextHint"];
            _userDialogs = DependencyService.Get<IUserDialogs>();
            Subscribe();
        }

        public AnaliseViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
            PageTitle = "Editar Análise";
			AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(async (task) =>
            {
                _analise = task.Result;
                IdentificacaoAnalise = _analise.Identificacao;
				Talhao = await AzureService.Instance.FindTalhaoAsync(_analise.TalhaoId);
				Fazenda = await AzureService.Instance.FindFazendaAsync(Talhao.FazendaId);

				FazendaNome = Fazenda.Nome;
                TalhaoNome = Talhao.Nome;
                
                DateSelected = _analise.DataRegistro;
                PotencialHidrogenico = _analise.PotencialHidrogenico.ToString();
                Fosforo = _analise.Fosforo.ToString();
                Potassio = _analise.Potassio.ToString();
                Calcio = _analise.Calcio.ToString();
                Magnesio = _analise.Magnesio.ToString();
                Aluminio = _analise.Aluminio.ToString();
                Hidrogenio = _analise.Hidrogenio.ToString();
                MateriaOrganica = _analise.MateriaOrganica.ToString();
                Areia = _analise.Areia.ToString();
                Silte = _analise.Silte.ToString();
                Argila = _analise.Argila.ToString();
            });

            _userDialogs = DependencyService.Get<IUserDialogs>();
            Subscribe();
        }

        public string FazendaNome
        {
			get { return _fazendaNome; }
            set
            {
                SetPropertyChanged(ref _fazendaNome, value);
                FazendaLabelColor = Color.Black;
            }
        }

        public Color FazendaLabelColor
        {
			get { return _fazendaLabelColor; }
			set { SetPropertyChanged(ref _fazendaLabelColor, value); }
        }

        public string TalhaoNome
        {
			get { return _talhaoNome; }
            set
            {
                SetPropertyChanged(ref _talhaoNome, value);
                TalhaoLabelColor = Color.Black;
            }
        }

        public Color TalhaoLabelColor
        {
			get { return _talhaoLabelColor; }
			set { SetPropertyChanged(ref _talhaoLabelColor, value); }
        }

        public string IdentificacaoAnalise
        {
			get { return _identificacaoAnalise; }
			set { SetPropertyChanged(ref _identificacaoAnalise, value); }
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

        public DateTimeOffset DateSelected
        {
			get { return _data; }
			set { SetPropertyChanged(ref _data, value); }
        }

        public string PotencialHidrogenico
        {
			get { return _potencialHidrogenico; }
			set { SetPropertyChanged(ref _potencialHidrogenico, $"{value:0.00}"); }
        }

        public string Fosforo
        {
			get { return _fosforo; }
			set { SetPropertyChanged(ref _fosforo, value); }
        }

        public string Potassio
        {
			get { return _potassio; }
			set { SetPropertyChanged(ref _potassio, value); }
        }

        public string Calcio
        {
			get { return _calcio; }
			set { SetPropertyChanged(ref _calcio, value); }
        }

        public string Magnesio
        {
			get { return _magnesio; }
			set { SetPropertyChanged(ref _magnesio, value); }
        }

        public string Aluminio
        {
			get { return _aluminio; }
			set { SetPropertyChanged(ref _aluminio, value); }
        }

        public string Hidrogenio
        {
			get { return _hidrogenio; }
			set { SetPropertyChanged(ref _hidrogenio, value); }
        }

        public string MateriaOrganica
        {
			get { return _materiaOrganica; }
			set { SetPropertyChanged(ref _materiaOrganica, value); }
        }

        public string Areia
        {
			get { return _areia; }
			set { SetPropertyChanged(ref _areia, value); }
        }

        public string Silte
		{
			get { return _silte; }
			set { SetPropertyChanged(ref _silte, value); }
        }

        public string Argila
        {
			get { return _argila; }
			set { SetPropertyChanged(ref _argila, value); }
        }

        public ICommand SelectDateCommand
            => _selectDateCommand ?? (_selectDateCommand = new Command(SelectDate));

        public ICommand ShowFazendasCommand
			=> _showFazendasCommand ?? (_showFazendasCommand = new Command(async ()=> await ShowFazendas()));

        public ICommand ShowTalhoesCommand
			=> _showTalhoesCommand ?? (_showTalhoesCommand = new Command(async ()=> await ShowTalhoes()));

		public ICommand SaveCommand 
			=> _saveCommand ?? (_saveCommand = new Command(async ()=> await Save()));


        private void SelectDate(object date)
        {
			DateSelected = (DateTime) date;
        }

		private async Task ShowFazendas()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new FazendaListPage(true));
                IsBusy = false;
            }
        }

		private async Task SelectFazenda(string id)
        {
            Fazenda = await AzureService.Instance.FindFazendaAsync(id);
            FazendaNome = Fazenda.Nome;
        }

        private async Task ShowTalhoes()
        {
            if (Fazenda == null)
            {
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseFazendaNull);
                return;
            }

            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new FazendaDetalhesPage(Fazenda.Id, true));
                IsBusy = false;
            }
        }

		private async Task SelectTalhao(string id)
        {
            Talhao = await AzureService.Instance.FindTalhaoAsync(id);
            TalhaoNome = Talhao.Nome;
        }

		private async Task Save()
		{
			if (string.IsNullOrEmpty(IdentificacaoAnalise))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseIdentificacaoNull);
				return;
			}

			if (Talhao == null)
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseTalhaoNull);
				return;
			}

			if (DateSelected == default(DateTime))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseDataInvalida);
				return;
			}

			if (string.IsNullOrEmpty(PotencialHidrogenico))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnalisePhNull);
				return;
			}

			if (string.IsNullOrEmpty(Fosforo))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnalisePNull);
				return;
			}

			if (string.IsNullOrEmpty(Potassio))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseKNull);
				return;
			}

			if (string.IsNullOrEmpty(Calcio))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseCaNull);
				return;
			}

			if (string.IsNullOrEmpty(Magnesio))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseMgNull);
				return;
			}

			if (string.IsNullOrEmpty(Aluminio))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseAlNull);
				return;
			}

			if (string.IsNullOrEmpty(Hidrogenio))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseHNull);
				return;
			}

			if (string.IsNullOrEmpty(MateriaOrganica))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseMoNull);
				return;
			}

			if (string.IsNullOrEmpty(Areia))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseAreiaNull);
				return;
			}

			if (string.IsNullOrEmpty(Silte))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseSilteNull);
				return;
			}

			if (string.IsNullOrEmpty(Argila))
			{
                await _userDialogs.DisplayAlert(MessagesResource.AnaliseArgilaNull);
				return;
			}

            IsBusy = true;

            //var userId = await DependencyService.Get<IAuthentication>().UserId();
            _analise = _analise ?? new Analise();
           // _analise.UsuarioId = userId;
			_analise.Identificacao = IdentificacaoAnalise;
			_analise.TalhaoId = Talhao.Id;
			_analise.DataRegistro = DateSelected;
			_analise.PotencialHidrogenico = float.Parse("0" + PotencialHidrogenico.Replace(',', '.'),
				CultureInfo.InvariantCulture);
			_analise.Fosforo = float.Parse("0" + Fosforo.Replace(',', '.'), CultureInfo.InvariantCulture);
			_analise.Potassio = float.Parse("0" + Potassio.Replace(',', '.'), CultureInfo.InvariantCulture);
			_analise.Calcio = float.Parse("0" + Calcio.Replace(',', '.'), CultureInfo.InvariantCulture);
			_analise.Magnesio = float.Parse("0" + Magnesio.Replace(',', '.'), CultureInfo.InvariantCulture);
			_analise.Aluminio = float.Parse("0" + Aluminio.Replace(',', '.'), CultureInfo.InvariantCulture);
			_analise.Hidrogenio = float.Parse("0" + Hidrogenio.Replace(',', '.'), CultureInfo.InvariantCulture);
			_analise.MateriaOrganica = float.Parse("0" + MateriaOrganica.Replace(',', '.'),
				CultureInfo.InvariantCulture);
			_analise.Areia = float.Parse("0" + Areia.Replace(',', '.'), CultureInfo.InvariantCulture);
			_analise.Silte = float.Parse("0" + Silte.Replace(',', '.'), CultureInfo.InvariantCulture);
			_analise.Argila = float.Parse("0" + Argila.Replace(',', '.'), CultureInfo.InvariantCulture);
			_analise.WasInterpreted = false;
			_analise.HasCalagem = false;
			_analise.HasCorretiva = false;
			_analise.HasSemeadura = false;
			_analise.HasCobertura = false;

			await AzureService.Instance.AddOrUpdateAnaliseAsync(_analise);
            _userDialogs.ShowToast(MessagesResource.AnaliseSucesso);
			var currentPage = Navigation.NavigationStack.LastOrDefault();
			await Navigation.PushAsync(new GerenciamentoAnalisePage(_analise.Id));
			Navigation.RemovePage(currentPage);

			IsBusy = false;
			Dispose();
		}

        public override void Dispose()
        {
            base.Dispose();
            Unsubscribe();
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<FazendaListViewModel, string>(this, MessagesResource.McFazendaSelecionada,
                async (model, id) => await SelectFazenda(id));
            MessagingCenter.Subscribe<FazendaCadastroViewModel, string>(this, MessagesResource.McFazendaSelecionada,
                async (model, id) => await SelectFazenda(id));
            MessagingCenter.Subscribe<FazendaDetalhesViewModel, string>(this, MessagesResource.McTalhaoSelecionado,
                async (model, id) => await SelectTalhao(id));
            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, MessagesResource.McTalhaoSelecionado,
                async (model, id) => await SelectTalhao(id));
        }

        private void Unsubscribe()
        {
            MessagingCenter.Unsubscribe<FazendaListViewModel, string>(this, MessagesResource.McFazendaSelecionada);
            MessagingCenter.Unsubscribe<FazendaCadastroViewModel, string>(this, MessagesResource.McFazendaSelecionada);
            MessagingCenter.Unsubscribe<FazendaDetalhesViewModel, string>(this, MessagesResource.McTalhaoSelecionado);
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel, string>(this, MessagesResource.McTalhaoSelecionado);
        }

    }
}