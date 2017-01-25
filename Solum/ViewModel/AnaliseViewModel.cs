using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Messages;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class AnaliseViewModel : BaseViewModel
    {
        #region Private Properties

        private string _title;
        private string _fazendaNome;
        private string _talhaoNome;
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

        private ICommand _selecionarDataCommand;
        private ICommand _selecionarFazendaCommand;
        private ICommand _selecionarTalhaoCommand;
        private ICommand _salvarCommand;

        private Fazenda _fazenda;
        private Talhao _talhao;
        private Analise _analise;
        private readonly Realm _realm;

        #endregion

        public AnaliseViewModel(INavigation navigation) : base(navigation)
        {
            _realm = Realm.GetInstance();
            Title = "Nova Análise";
            FazendaNome = "Selecione uma fazenda";
            TalhaoNome = "Selecione um talhão";

            MessagingCenter.Subscribe<FazendaListViewModel, string>(this, MessagingCenterMessages.FazendaSelected, (model, fazenda) =>
            {
                SelecionarFazenda(fazenda);
            });

            MessagingCenter.Subscribe<FazendaCadastroViewModel, string>(this, MessagingCenterMessages.FazendaSelected, (model, fazenda) =>
            {
                SelecionarFazenda(fazenda);
            });

            MessagingCenter.Subscribe<FazendaDetalhesViewModel, string>(this, MessagingCenterMessages.TalhaoSelected, (model, talhao) =>
             {
                 SelecionarTalhao(talhao);
             });

            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, MessagingCenterMessages.TalhaoSelected, (model, talhao) =>
            {
                SelecionarTalhao(talhao);
            });
        }

        public AnaliseViewModel(INavigation navigation, Analise analise) : base(navigation)
        {
            _realm = Realm.GetInstance();
            Title = $"Atualizar Análise: {analise.Nome}";

            Fazenda = analise.Talhao.Fazenda;
            Talhao = analise.Talhao;
            DataSelecionada = analise.Data;
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
        }

        #region Commands

        public ICommand DataSelecionadaCommand => _selecionarDataCommand ?? (_selecionarDataCommand = new Command(SelecionarData));
        public ICommand SelecionarFazendaCommand => _selecionarFazendaCommand ?? (_selecionarFazendaCommand = new Command(SelecionarFazenda));
        public ICommand SelecionarTalhaoCommand => _selecionarTalhaoCommand ?? (_selecionarTalhaoCommand = new Command(SelecionarTalhao));
        public ICommand SalvarCommand => _salvarCommand ?? (_salvarCommand = new Command(Salvar));

        #endregion

        #region Binding Properties
        
        public string Title
        {
            get { return _title; }
            set { SetPropertyChanged(ref _title, value); }
        }

        public string FazendaNome
        {
            get { return _fazendaNome; }
            set { SetPropertyChanged(ref _fazendaNome, value); }
        }

        public string TalhaoNome
        {
            get { return _talhaoNome; }
            set { SetPropertyChanged(ref _talhaoNome, value); }
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

        public DateTimeOffset DataSelecionada
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

        private void SelecionarData(object parameter)
        {
            DataSelecionada = (DateTimeOffset) parameter;
        }

        private async void SelecionarFazenda()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new FazendaListPage(true));
                IsBusy = false;
            }
        }

        private void SelecionarFazenda(string id)
        {
            Fazenda = _realm.Find<Fazenda>(id);
            FazendaNome = Fazenda.Nome;
        }

        private async void SelecionarTalhao()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new FazendaDetalhesPage(Fazenda, true));
                IsBusy = false;
            }
        }

        private void SelecionarTalhao(string id)
        {
            Talhao = _realm.Find<Talhao>(id);
            TalhaoNome = Talhao.Nome;
        }

        private async void Salvar()
        {
            if (Talhao == null)
            {
                AnaliseMessages.TalhaoIsntSelected.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (DataSelecionada == default(DateTime))
            {
                AnaliseMessages.InvalidDate.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(PotencialHidrogenico))
            {
                AnaliseMessages.PhNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Fosforo))
            {
                AnaliseMessages.PNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Potassio))
            {
                AnaliseMessages.KNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Calcio))
            {
                AnaliseMessages.CaNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Magnesio))
            {
                AnaliseMessages.MgNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Aluminio))
            {
                AnaliseMessages.AlNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Hidrogenio))
            {
                AnaliseMessages.HNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(MateriaOrganica))
            {
                AnaliseMessages.MoNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Areia))
            {
                AnaliseMessages.AreaiNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Silte))
            {
                AnaliseMessages.SilteNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (string.IsNullOrEmpty(Argila))
            {
                AnaliseMessages.ArgilaNull.ToDisplayAlert(MessageType.Aviso);
                return;
            }


            var analise = new Analise
            {
                Talhao = Talhao,
                TalhaoId = Talhao.Id,
                Data = DataSelecionada,
                PotencialHidrogenico = float.Parse("0" + PotencialHidrogenico.Replace(',', '.'), CultureInfo.InvariantCulture),
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
                Argila = float.Parse("0" + Argila.Replace(',', '.'), CultureInfo.InvariantCulture),
                Id = Guid.NewGuid().ToString()
            };

            using (var transaction = _realm.BeginWrite())
            {
                _realm.Add(analise);
                transaction.Commit();
            }

            var current = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(new GerenciamentoAnalise(analise));
            Navigation.RemovePage(current);
            Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();
            MessagingCenter.Unsubscribe<FazendaListViewModel, string>(this, MessagingCenterMessages.FazendaSelected);
            MessagingCenter.Unsubscribe<FazendaCadastroViewModel, string>(this, MessagingCenterMessages.FazendaSelected);
            MessagingCenter.Unsubscribe<FazendaDetalhesViewModel, string>(this, MessagingCenterMessages.TalhaoSelected);
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel, string>(this, MessagingCenterMessages.TalhaoSelected);
        }
    }
}