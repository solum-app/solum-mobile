﻿using System;
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
        public AnaliseViewModel(INavigation navigation) : base(navigation)
        {
            PageTitle = "Nova Análise";
            FazendaNome = "Selecione uma fazenda";
            TalhaoNome = "Selecione um talhão";
            FazendaLabelColor = (Color) Application.Current.Resources["colorTextHint"];
            TalhaoLabelColor = (Color) Application.Current.Resources["colorTextHint"];
            Subscribe();
        }

        public AnaliseViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
            PageTitle = "Editar Análise";
			AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(async (task) =>
            {
                Analise = task.Result;
                IdentificacaoAnalise = Analise.Identificacao;
				Talhao = await AzureService.Instance.FindTalhaoAsync(Analise.TalhaoId);
				Fazenda = await AzureService.Instance.FindFazendaAsync(Talhao.FazendaId);

				FazendaNome = Fazenda.Nome;
                TalhaoNome = Talhao.Nome;
                
                DateSelected = Analise.DataRegistro;
                PotencialHidrogenico = Analise.PotencialHidrogenico.ToString();
                Fosforo = Analise.Fosforo.ToString();
                Potassio = Analise.Potassio.ToString();
                Calcio = Analise.Calcio.ToString();
                Magnesio = Analise.Magnesio.ToString();
                Aluminio = Analise.Aluminio.ToString();
                Hidrogenio = Analise.Hidrogenio.ToString();
                MateriaOrganica = Analise.MateriaOrganica.ToString();
                Areia = Analise.Areia.ToString();
                Silte = Analise.Silte.ToString();
                Argila = Analise.Argila.ToString();
            });
            
            Subscribe();
        }

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

        public Analise Analise
        {
			get { return _analise; }
			set { SetPropertyChanged(ref _analise, value); }
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
                MessagesResource.AnaliseFazendaNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (IsNotBusy)
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
            if (!IsNotBusy) return;
            if (string.IsNullOrEmpty(IdentificacaoAnalise))
            {
                MessagesResource.AnaliseIdentificacaoNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (Talhao == null)
            {
                MessagesResource.AnaliseTalhaoNull.ToDisplayAlert(MessageType.Info);
                return;
            }


            if (DateSelected == default(DateTime))
            {
                MessagesResource.AnaliseDataInvalida.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (string.IsNullOrEmpty(PotencialHidrogenico))
            {
                MessagesResource.AnalisePhNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (string.IsNullOrEmpty(Fosforo))
            {
                MessagesResource.AnalisePNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (string.IsNullOrEmpty(Potassio))
            {
                MessagesResource.AnaliseKNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (string.IsNullOrEmpty(Calcio))
            {
                MessagesResource.AnaliseCaNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (string.IsNullOrEmpty(Magnesio))
            {
                MessagesResource.AnaliseMgNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (string.IsNullOrEmpty(Aluminio))
            {
                MessagesResource.AnaliseAlNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (string.IsNullOrEmpty(Hidrogenio))
            {
                MessagesResource.AnaliseHNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (string.IsNullOrEmpty(MateriaOrganica))
            {
                MessagesResource.AnaliseMoNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (string.IsNullOrEmpty(Areia))
            {
                MessagesResource.AnaliseAreiaNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (string.IsNullOrEmpty(Silte))
            {
                MessagesResource.AnaliseSilteNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            if (string.IsNullOrEmpty(Argila))
            {
                MessagesResource.AnaliseArgilaNull.ToDisplayAlert(MessageType.Info);
                return;
            }

            var userId = await DependencyService.Get<IAuthentication>().UserId();
            if (Analise == null)
            {
                Analise = new Analise
                {
                    Id = Guid.NewGuid().ToString(),
                    UsuarioId = userId,
                    TalhaoId = Talhao.Id,
                    Identificacao = IdentificacaoAnalise,
                    DataRegistro = DateSelected,
                    PotencialHidrogenico =
                        float.Parse("0" + PotencialHidrogenico.Replace(',', '.'), CultureInfo.InvariantCulture),
                    Fosforo = float.Parse("0" + Fosforo.Replace(',', '.'), CultureInfo.InvariantCulture),
                    Potassio = float.Parse("0" + Potassio.Replace(',', '.'), CultureInfo.InvariantCulture),
                    Calcio = float.Parse("0" + Calcio.Replace(',', '.'), CultureInfo.InvariantCulture),
                    Magnesio = float.Parse("0" + Magnesio.Replace(',', '.'), CultureInfo.InvariantCulture),
                    Aluminio = float.Parse("0" + Aluminio.Replace(',', '.'), CultureInfo.InvariantCulture),
                    Hidrogenio = float.Parse("0" + Hidrogenio.Replace(',', '.'), CultureInfo.InvariantCulture),
                    MateriaOrganica =
                        float.Parse("0" + MateriaOrganica.Replace(',', '.'), CultureInfo.InvariantCulture),
                    Areia = float.Parse("0" + Areia.Replace(',', '.'), CultureInfo.InvariantCulture),
                    Silte = float.Parse("0" + Silte.Replace(',', '.'), CultureInfo.InvariantCulture),
                    Argila = float.Parse("0" + Argila.Replace(',', '.'), CultureInfo.InvariantCulture)
                };
                await AzureService.Instance.InsertAnaliseAsync(Analise);
            }
            else
            {
                Analise.Identificacao = IdentificacaoAnalise;
                Analise.UsuarioId = userId;
                Analise.TalhaoId = Talhao.Id;
                Analise.DataRegistro = DateSelected;
                Analise.PotencialHidrogenico = float.Parse("0" + PotencialHidrogenico.Replace(',', '.'),
                    CultureInfo.InvariantCulture);
                Analise.Fosforo = float.Parse("0" + Fosforo.Replace(',', '.'), CultureInfo.InvariantCulture);
                Analise.Potassio = float.Parse("0" + Potassio.Replace(',', '.'), CultureInfo.InvariantCulture);
                Analise.Calcio = float.Parse("0" + Calcio.Replace(',', '.'), CultureInfo.InvariantCulture);
                Analise.Magnesio = float.Parse("0" + Magnesio.Replace(',', '.'), CultureInfo.InvariantCulture);
                Analise.Aluminio = float.Parse("0" + Aluminio.Replace(',', '.'), CultureInfo.InvariantCulture);
                Analise.Hidrogenio = float.Parse("0" + Hidrogenio.Replace(',', '.'), CultureInfo.InvariantCulture);
                Analise.MateriaOrganica = float.Parse("0" + MateriaOrganica.Replace(',', '.'),
                    CultureInfo.InvariantCulture);
                Analise.Areia = float.Parse("0" + Areia.Replace(',', '.'), CultureInfo.InvariantCulture);
                Analise.Silte = float.Parse("0" + Silte.Replace(',', '.'), CultureInfo.InvariantCulture);
                Analise.Argila = float.Parse("0" + Argila.Replace(',', '.'), CultureInfo.InvariantCulture);
                Analise.WasInterpreted = false;
                Analise.HasCalagem = false;
                Analise.HasCorretiva = false;
                Analise.HasSemeadura = false;
                Analise.HasCobertura = false;
                await AzureService.Instance.UpdateAnaliseAsync(Analise);
            }

            MessagesResource.AnaliseSucesso.ToToast(ToastNotificationType.Sucesso);
            var currentPage = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(new GerenciamentoAnalisePage(Analise.Id));
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