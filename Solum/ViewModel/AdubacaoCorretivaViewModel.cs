using System;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class AdubacaoCorretivaViewModel : BaseViewModel
    {
        public AdubacaoCorretivaViewModel(INavigation navigation, string analiseId, bool enableButton) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _analise = _realm.Find<Analise>(analiseId);
            PageTitle = _analise.Identificacao;
            EnableButton = enableButton;
            Calculate();
        }

        #region Functions

        private void Calculate()
        {
            var argila = _analise.Argila;

            var textura = InterpretaHandler.InterpretaTextura(_analise.Argila, _analise.Silte);
            var pInterpretaded = InterpretaHandler.InterpretaP(_analise.Fosforo, textura).ToUpper();
            var kInterpretaded = InterpretaHandler.InterpretaK(_analise.Potassio, _analise.CTC).ToUpper();

            if (pInterpretaded != "ADEQUADO" && pInterpretaded != "ALTO")
            {
                if (pInterpretaded.Equals("Muito Baixo".ToUpper()))
                    P2O5 = (argila / 10 * 4).ToString("###");
                else if (pInterpretaded.Equals("Baixo".ToUpper()))
                    P2O5 = (argila / 10 * 2).ToString("###");
                else if (pInterpretaded.Equals("Medio".ToUpper()))
                    P2O5 = (argila / 10 * 1).ToString("###");
            }
            else
            {
                P2O5 = 0.ToString();
            }

            if (!kInterpretaded.Equals("Adequado".ToUpper()) && !kInterpretaded.Equals("Alto".ToUpper()))
            {
                var ctc = _analise.CTC;
                if (ctc < 4)
                {
                    if (kInterpretaded.Equals("Baixo".ToUpper()))
                        K2O = 50.0f.ToString("###");
                    else if (kInterpretaded.Equals("Medio".ToUpper()))
                        K2O = 25.0f.ToString("###");
                }
                else
                {
                    if (kInterpretaded.Equals("Baixo".ToUpper()))
                        K2O = 100.0f.ToString("###");
                    else if (kInterpretaded.Equals("Medio".ToUpper()))
                        K2O = 50.0f.ToString("###");
                }
            }
            else
            {
                K2O = 0.ToString();
            }

            //using (var transaction = _realm.BeginWrite())
            //{
            //    _analise.DataCalculoCorretiva = DateTimeOffset.Now;
            //    transaction.Commit();
            //}
        }

        #endregion

        #region Private Properties

        private bool _enableButton;
        private ICommand _salvarCommand;
        private string _p2O5;
        private string _k2O;

        private readonly Analise _analise;
        private readonly Realm _realm;

        #endregion

        #region Binding Properties

        public ICommand SalvarCommand => _salvarCommand ?? (_salvarCommand = new Command(Salvar));
        public string P2O5
        {
            get { return _p2O5; }
            set { SetPropertyChanged(ref _p2O5, value); }
        }

        public string K2O
        {
            get { return _k2O; }
            set { SetPropertyChanged(ref _k2O, value); }
        }

        public bool EnableButton
        {
            get { return _enableButton;}
            set { SetPropertyChanged(ref _enableButton, value); }
        }

        #endregion

        #region Functions

        private async void Salvar()
        {
            using (var transaction = _realm.BeginWrite())
            {
                _analise.DataCalculoCorretiva = DateTimeOffset.Now;
                _analise.HasCorretiva = true;
                transaction.Commit();
            }
            MessagesResource.DadosSalvos.ToToast();
            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PopAsync();
                IsBusy = false;
            }
        }

        #endregion
    }
}