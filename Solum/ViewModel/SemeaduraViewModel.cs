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
            _realm = Realm.GetInstance();
            _analise = _realm.Find<Analise>(analiseId);
            var p = InterpretaHandler.InterpretaK(_analise.Potassio, _analise.CTC);
            IsPotassioBaixo = !p.ToUpper().Equals("ADEQUADO") || !p.ToUpper().Equals("ALTO");
            PageTitle = _analise.Identificacao;
            Expectativas = new List<int> {6, 8, 10, 12};
            Culturas = new List<string> {"Milho"};
        }

        #region private properties

        private IList<int> _expectativas;
        private int _expectativaSelected;

        private IList<string> _culturas;
        private string _culturaSelected;

        private bool _isPotassioBaixo;
        private readonly Analise _analise;

        private ICommand _recomendarCommand;

        private readonly Realm _realm;

        #endregion

        #region binding properties
        
        public IList<int> Expectativas
        {
            get { return _expectativas; }
            set { SetPropertyChanged(ref _expectativas, value); }
        }

        public int ExpectativaSelected
        {
            get { return _expectativaSelected;}
            set { SetPropertyChanged(ref _expectativaSelected, value); }
        }

        public IList<string> Culturas
        {
            get { return _culturas;}
            set { SetPropertyChanged(ref _culturas, value); }
        }

        public string CulturaSelected
        {
            get { return _culturaSelected;}
            set { SetPropertyChanged(ref _culturaSelected, value); }
        }

        public bool IsPotassioBaixo
        {
            get { return _isPotassioBaixo;}
            set { SetPropertyChanged(ref _isPotassioBaixo, value); }
        }

        #endregion

        #region commands

        public ICommand RecomendarCommand => _recomendarCommand ?? (_recomendarCommand = new Command(Recomendar));

        #endregion

        #region functions

        private async void Recomendar()
        {
            if (ExpectativaSelected == 0 || string.IsNullOrEmpty(CulturaSelected))
            {
                "Selecione a sua expectativa e a cultura".ToDisplayAlert(MessageType.Aviso);
                return;
            }

            Semeadura obj;
            if (_realm.All<Semeadura>().Any(s => s.AnaliseId.Equals(_analise.Id)))
            {
                obj = _realm.All<Semeadura>().FirstOrDefault(s => s.AnaliseId.Equals(_analise.Id));
                using (var transaction = _realm.BeginWrite())
                {
                    obj.Cultura = CulturaSelected;
                    obj.Expectativa = ExpectativaSelected;
                    transaction.Commit();
                }
            }
            else
            {
               obj = new Semeadura()
                {
                    Id = Guid.NewGuid().ToString(),
                    Analise = _analise,
                    AnaliseId = _analise.Id,
                    Cultura = CulturaSelected,
                    Expectativa = ExpectativaSelected
                };

                using (var transaction = _realm.BeginWrite())
                {
                    _realm.Add(obj);
                    transaction.Commit();
                }
            }

            if (!IsBusy)
            {
                IsBusy = true;
                var current = Navigation.NavigationStack.LastOrDefault();
                await Navigation.PushAsync(new RecomendaSemeaduraPage(_analise.Id, ExpectativaSelected, CulturaSelected));
                Navigation.RemovePage(current);
                IsBusy = false;
            }
        }

        #endregion
    }
}