using System;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Messages;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;
using static Solum.Messages.AnaliseMessages;

namespace Solum.ViewModel
{
    public class AnaliseViewModel : BaseViewModel
    {
        public AnaliseViewModel(INavigation navigation) : base(navigation)
        {
            _realm = Realm.GetInstance();
            PageTitle = "Nova Análise";
            FazendaName = "Selecione uma fazenda";
            TalhaoName = "Selecione um talhão";
            Subscribe();
        }

        public AnaliseViewModel(INavigation navigation, Analise analise) : base(navigation)
        {
            _realm = Realm.GetInstance();
            PageTitle = $"Atualizar Análise: {analise.Nome}";
            Fazenda = _realm.Find<Fazenda>(analise.Talhao.FazendaId);
            Talhao = _realm.Find<Talhao>(analise.Id);
            FazendaName = Fazenda.Nome;
            TalhaoName = Talhao.Nome;
            DateSelected = analise.Data;
            PotencialHidrogenico = analise.PotencialHidrogenico.ToString();
            Fosforo = analise.Fosforo.ToString();
            Potassio = analise.Potassio.ToString();
            Calcio = analise.Calcio.ToString();
            Magnesio = analise.Magnesio.ToString();
            Aluminio = analise.Aluminio.ToString();
            Hidrogenio = analise.Hidrogenio.ToString();
            MateriaOrganica = analise.MateriaOrganica.ToString();
            Areia = analise.Areia.ToString();
            Silte = analise.Silte.ToString();
            Argila = analise.Argila.ToString();
            Subscribe();
        }

        #region Private Properties

        private string _pageTitle;
        private string _fazendaName;
        private string _talhaoName;
        private string _analiseName;
        private DateTimeOffset _data = DateTimeOffset.Now;

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

        private ICommand _selectDateCommand;
        private ICommand _showFazendasCommand;
        private ICommand _showTalhoesCommand;
        private ICommand _saveCommand;

        private Fazenda _fazenda;
        private Talhao _talhao;
        private Analise _analise;
        private readonly Realm _realm;

        #endregion

        #region Binding Properties

        public string PageTitle
        {
            get { return _pageTitle; }
            set { SetPropertyChanged(ref _pageTitle, value); }
        }

        public string FazendaName
        {
            get { return _fazendaName; }
            set { SetPropertyChanged(ref _fazendaName, value); }
        }

        public string TalhaoName
        {
            get { return _talhaoName; }
            set { SetPropertyChanged(ref _talhaoName, value); }
        }

        public string AnaliseName
        {
            get { return _analiseName; }
            set { SetPropertyChanged(ref _analiseName, value); }
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
            set { SetPropertyChanged(ref _potencialHidrogenico, string.Format("{0:0.00}", value)); }
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

        #endregion

        #region Commands

        public ICommand SelectDateCommand
            => _selectDateCommand ?? (_selectDateCommand = new Command(SelectDate));

        public ICommand ShowFazendasCommand
            => _showFazendasCommand ?? (_showFazendasCommand = new Command(ShowFazendas));

        public ICommand ShowTalhoesCommand
            => _showTalhoesCommand ?? (_showTalhoesCommand = new Command(ShowTalhoes));

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new Command(Save));

        #endregion

        #region Funções

      
        private void SelectDate(object date)
        {
            DateSelected = (DateTimeOffset)date;
        }

        private async void ShowFazendas()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new FazendaListPage(true));
                IsBusy = false;
            }
        }

        private void SelectFazenda(string id)
        {
            Fazenda = _realm.Find<Fazenda>(id);
            FazendaName = Fazenda.Nome;
        }

        private async void ShowTalhoes()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new FazendaDetalhesPage(Fazenda, true));
                IsBusy = false;
            }
        }

        private void SelectTalhao(string id)
        {
            Talhao = _realm.Find<Talhao>(id);
            TalhaoName = Talhao.Nome;
        }

        private async void Save()
        {
            if (Talhao == null)
            {
                TalhaoIsntSelected.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(AnaliseName))
            {
                "Identifique a análise com um nome".ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (DateSelected == default(DateTime))
            {
                InvalidDate.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(PotencialHidrogenico))
            {
                PhNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Fosforo))
            {
                PNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Potassio))
            {
                KNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Calcio))
            {
                CaNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Magnesio))
            {
                MgNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Aluminio))
            {
                AlNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Hidrogenio))
            {
                HNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(MateriaOrganica))
            {
                MoNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Areia))
            {
                AreaiNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Silte))
            {
                SilteNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Argila))
            {
                ArgilaNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }


            var analise = new Analise
            {
                Id = Guid.NewGuid().ToString(),
                TalhaoId = Talhao.Id,
                Nome = AnaliseName,
                Talhao = Talhao,
                Data = DateSelected,
                PotencialHidrogenico =
                    float.Parse("0" + PotencialHidrogenico.Replace(',', '.'), CultureInfo.InvariantCulture),
                Fosforo = float.Parse("0" + Fosforo.Replace(',', '.'), CultureInfo.InvariantCulture),
                Potassio = float.Parse("0" + Potassio.Replace(',', '.'), CultureInfo.InvariantCulture),
                Calcio = float.Parse("0" + Calcio.Replace(',', '.'), CultureInfo.InvariantCulture),
                Magnesio = float.Parse("0" + Magnesio.Replace(',', '.'), CultureInfo.InvariantCulture),
                Aluminio = float.Parse("0" + Aluminio.Replace(',', '.'), CultureInfo.InvariantCulture),
                Hidrogenio = float.Parse("0" + Hidrogenio.Replace(',', '.'), CultureInfo.InvariantCulture),
                MateriaOrganica = float.Parse("0" + MateriaOrganica.Replace(',', '.'), CultureInfo.InvariantCulture),
                Areia = float.Parse("0" + Areia.Replace(',', '.'), CultureInfo.InvariantCulture),
                Silte = float.Parse("0" + Silte.Replace(',', '.'), CultureInfo.InvariantCulture),
                Argila = float.Parse("0" + Argila.Replace(',', '.'), CultureInfo.InvariantCulture)
            };

            using (var transaction = _realm.BeginWrite())
            {
                _realm.Add(analise);
                transaction.Commit();
            }

            Success.ToToast(ToastNotificationType.Sucesso);
            var last = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(new GerenciamentoAnalisePage(analise));
            Navigation.RemovePage(last);
            Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();
            Unsubscribe();
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<FazendaListViewModel, string>(this, MessagingCenterMessages.FazendaSelected,
                (model, id) => { SelectFazenda(id); });

            MessagingCenter.Subscribe<FazendaCadastroViewModel, string>(this, MessagingCenterMessages.FazendaSelected,
                (model, id) => { SelectFazenda(id); });

            MessagingCenter.Subscribe<FazendaDetalhesViewModel, string>(this, MessagingCenterMessages.TalhaoSelected,
                (model, id) => { SelectTalhao(id); });

            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, MessagingCenterMessages.TalhaoSelected,
                (model, id) => { SelectTalhao(id); });
        }

        private void Unsubscribe()
        {
            MessagingCenter.Unsubscribe<FazendaListViewModel, string>(this, MessagingCenterMessages.FazendaSelected);
            MessagingCenter.Unsubscribe<FazendaCadastroViewModel, string>(this, MessagingCenterMessages.FazendaSelected);
            MessagingCenter.Unsubscribe<FazendaDetalhesViewModel, string>(this, MessagingCenterMessages.TalhaoSelected);
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel, string>(this, MessagingCenterMessages.TalhaoSelected);
        }

        #endregion
    }
}