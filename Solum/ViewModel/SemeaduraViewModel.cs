using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class SemeaduraViewModel : BaseViewModel
    {
        public SemeaduraViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
            var realm = Realm.GetInstance();
            _analise = realm.Find<Analise>(analiseId);
            var k = InterpretaHandler.InterpretaK(_analise.Potassio, _analise.CTC);
            var textura = InterpretaHandler.InterpretaTextura(_analise.Argila, _analise.Silte);
            var p = InterpretaHandler.InterpretaP(_analise.Fosforo, textura);
            IsPotassioBaixo = k.ToUpper() != "ADEQUADO" & k.ToUpper() != "ALTO";
            IsFosforoBaixo = p.ToUpper() != "ADEQUADO" & p.ToUpper() != "ALTO";
            PageTitle = _analise.Identificacao;
            Expectativas = new List<DisplayItems>
            {
                new DisplayItems("6 t/ha", 6),
                new DisplayItems("8 t/ha", 8),
                new DisplayItems("10 t/ha", 10),
                new DisplayItems("12 t/ha", 12)
            };
            Culturas = new List<string> {"Milho"};
        }

        #region commands

        public ICommand RecomendarCommand => _recomendarCommand ?? (_recomendarCommand = new Command(Recomendar));

        #endregion

        #region functions

        private async void Recomendar()
        {
            if (ExpectativaSelected == null)
            {
                "Selecione a sua expectativa".ToDisplayAlert(MessageType.Aviso);
                return;
            }
            if (string.IsNullOrEmpty(CulturaSelected))
            {
                "Selecione a cultura".ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (IsNotBusy)
            {
                IsBusy = true;
                var current = Navigation.NavigationStack.LastOrDefault();
                await Navigation.PushAsync(new RecomendaSemeaduraPage(_analise.Id, (int) ExpectativaSelected.Value,
                    CulturaSelected, true));
                Navigation.RemovePage(current);
                IsBusy = false;
            }
        }

        #endregion

        #region private properties

        private IList<DisplayItems> _expectativas;
        private DisplayItems _expectativaSelected;

        private IList<string> _culturas;
        private string _culturaSelected;

        private bool _isPotassioBaixo;
        private bool _isFosforoBaixo;

        private readonly Analise _analise;

        private ICommand _recomendarCommand;

        private bool _enableButton;

        #endregion

        #region binding properties

        public bool EnableButton
        {
            get { return _enableButton; }
            set { SetPropertyChanged(ref _enableButton, value); }
        }

        public IList<DisplayItems> Expectativas
        {
            get { return _expectativas; }
            set { SetPropertyChanged(ref _expectativas, value); }
        }

        public DisplayItems ExpectativaSelected
        {
            get { return _expectativaSelected; }
            set { SetPropertyChanged(ref _expectativaSelected, value); }
        }

        public IList<string> Culturas
        {
            get { return _culturas; }
            set { SetPropertyChanged(ref _culturas, value); }
        }

        public string CulturaSelected
        {
            get { return _culturaSelected; }
            set { SetPropertyChanged(ref _culturaSelected, value); }
        }

        public bool IsPotassioBaixo
        {
            get { return _isPotassioBaixo; }
            set { SetPropertyChanged(ref _isPotassioBaixo, value); }
        }

        public bool IsFosforoBaixo
        {
            get { return _isFosforoBaixo; }
            set { SetPropertyChanged(ref _isFosforoBaixo, value); }
        }

        public bool IsPotassioFosforoBaixo => IsPotassioBaixo && IsFosforoBaixo;
        public bool JustPotassioBaixo => IsPotassioBaixo && !IsFosforoBaixo;
        public bool JustFosforoBaixo => !IsPotassioBaixo && IsFosforoBaixo;

        #endregion
    }
}