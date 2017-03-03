using System;
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
            var k = Interpretador.NivelPotassio(_analise.Potassio, _analise.CTC);
            var textura = Interpretador.Textura(_analise.Argila, _analise.Silte);
            var p = Interpretador.NiveFosforo(_analise.Fosforo, textura);
            IsPotassioBaixo = k != Nivel.Adequado && k != Nivel.Alto;
            IsFosforoBaixo = p != Nivel.Adequado && p != Nivel.Alto; 
            PageTitle = _analise.Identificacao;

            Expectativas = new List<DisplayNumber>
            {
                new DisplayNumber("6 t/ha", 6),
                new DisplayNumber("8 t/ha", 8),
                new DisplayNumber("10 t/ha", 10),
                new DisplayNumber("12 t/ha", 12)
            };

            Culturas = new List<DisplayEnum>();
            foreach (Cultura value in Enum.GetValues(typeof(Cultura)))
            {
                Culturas.Add(new DisplayEnum(value.ToString(), value));
            }

            if (_analise.HasSemeadura)
            {
                CulturaSelected = Culturas.FirstOrDefault(x => x.Text.Equals(_analise.Cultura));
                ExpectativaSelected = Expectativas.FirstOrDefault(x => x.Value == _analise.Expectativa);
            }
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

            if (CulturaSelected == null)
            {
                "Selecione a cultura".ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (IsNotBusy)
            {
                IsBusy = true;
                var current = Navigation.NavigationStack.LastOrDefault();
                await Navigation.PushAsync(new RecomendaSemeaduraPage(_analise.Id, (int) ExpectativaSelected.Value, (Cultura) CulturaSelected.Item, true));
                Navigation.RemovePage(current);
                IsBusy = false;
            }
        }

        #endregion

        #region private properties

        private IList<DisplayNumber> _expectativas;
        private DisplayNumber _expectativaSelected;

        private IList<DisplayEnum> _culturas;
        private DisplayEnum _culturaSelected;

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

        public IList<DisplayNumber> Expectativas
        {
            get { return _expectativas; }
            set { SetPropertyChanged(ref _expectativas, value); }
        }

        public DisplayNumber ExpectativaSelected
        {
            get { return _expectativaSelected; }
            set { SetPropertyChanged(ref _expectativaSelected, value); }
        }

        public IList<DisplayEnum> Culturas
        {
            get { return _culturas; }
            set { SetPropertyChanged(ref _culturas, value); }
        }

        public DisplayEnum CulturaSelected
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