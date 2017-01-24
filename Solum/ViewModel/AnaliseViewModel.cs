using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Realms;
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
        private Realm _realm;

        #endregion

        public AnaliseViewModel(INavigation navigation) : base(navigation)
        {
            _realm = Realm.GetInstance();
            Title = "Nova Análise";
            FazendaNome = "Selecione uma fazenda";
            MessagingCenter.Subscribe<FazendaListViewModel, Fazenda>(this, "FazendaSelecionada", (model, fazenda) =>
            {
                SelecionarFazenda(fazenda);
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

        private void SelecionarFazenda(Fazenda fazenda)
        {
            Fazenda = _realm.Find<Fazenda>(fazenda.Id);
            FazendaNome = Fazenda.Nome;
        }
        private void SelecionarTalhao() { }
        private void Salvar() { }



        protected async Task ExecuteButtonClickedCommand()
        {
            //if (string.IsNullOrEmpty(Fazenda))
            //{
            //    await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
            //        "Insira um nome para a Fazenda", "OK");
            //    return;
            //}
            //if (string.IsNullOrEmpty(Talhao))
            //{
            //    await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
            //        "Insira uma identificação para o Talhão", "OK");
            //    return;
            //}
            if (DataSelecionada == default(DateTime))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para a Data", "OK");
                return;
            }
            if (string.IsNullOrEmpty(PotencialHidrogenico))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para o pH", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Fosforo))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para o P", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Potassio))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para o K", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Calcio))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para o Ca", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Magnesio))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para o Mg", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Aluminio))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para o Al", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Hidrogenio))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para H", "OK");
                return;
            }
            if (string.IsNullOrEmpty(MateriaOrganica))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para a Materia Orgânica", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Areia))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para a Areia", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Silte))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para o Silte", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Argila))
            {
                await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido",
                    "Insira um valor válido para a Argila", "OK");
                return;
            }


            var analise = new Analise
            {
                //Fazenda = FazendaEntry.Trim (),
                //Talhao = TalhaoEntry,
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
                Argila = float.Parse("0" + Argila.Replace(',', '.'), CultureInfo.InvariantCulture)
            };

            //if (realmAnalise == default(Analise))
            //    await Navigation.PushAsync(new InterpretacaoPage(analise, false));
            //else await Navigation.PushAsync(new InterpretacaoPage(analise, realmAnalise, false));
        }
    }
}