using System;
using System.Collections.Generic;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class RecomendacaoCalagemViewModel : BaseViewModel
    {
        public RecomendacaoCalagemViewModel(INavigation navigation, string analiseId, float v2, float prnt, float profundidade, bool enableButton) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _analise = _realm.Find<Analise>(analiseId);
            PageTitle = _analise.Identificacao;
            V2 = v2.ToString();
            Prnt = prnt.ToString();
            Profundidade = profundidade.ToString();
            Calculate();
            EnableButton = enableButton;
        }

        #region Commands

        public ICommand SalvarCommand => _salvarCommand ?? (_salvarCommand = new Command(Salvar));

        #endregion

        #region Private Properties

        private bool _enableButton;
        private string _quantidade;
        private string _v2;
        private string _prnt;
        private string _profundidade;

        private readonly Analise _analise;
        private readonly Realm _realm;

        private readonly IDictionary<int, float> _values = new Dictionary<int, float>
        {
            {5, 0.25f},
            {10, 0.5f},
            {20, 1.0f},
            {40, 2.0f}
        };

        private ICommand _salvarCommand;
        #endregion

        #region Binding Properties

        public bool EnableButton
        {
            get { return _enableButton; }
            set { SetPropertyChanged(ref _enableButton, value); }
        }
        public string QuantidadeCal
        {
            get { return _quantidade; }
            set { SetPropertyChanged(ref _quantidade, value); }
        }

        public string V2
        {
            get { return string.Format(_v2, "###.###"); }
            set { SetPropertyChanged(ref _v2, value); }
        }

        public string Prnt
        {
            get { return string.Format(_prnt, "###.###"); }
            set { SetPropertyChanged(ref _prnt, value); }
        }

        public string Profundidade
        {
            get { return string.Format(_profundidade, "##"); }
            set { SetPropertyChanged(ref _profundidade, value); }
        }

        #endregion

        #region Functions

        private async void Salvar()
        {
            using (var transaction = _realm.BeginWrite())
            {
                _analise.V2 = float.Parse(V2);
                _analise.Prnt = float.Parse(Prnt);
                _analise.Profundidade = int.Parse(Profundidade);
                _analise.DataCalculoCalagem = DateTimeOffset.Now;
                _analise.HasCalagem = true;
                transaction.Commit();
            }

            if (IsNotBusy)
            {
                IsBusy = true;
                "Dados salvo com sucesso".ToToast(ToastNotificationType.Sucesso);
                await Navigation.PopAsync();
                IsBusy = false;
            }
        }

        public void Calculate()
        {
            float fPrnt, fV2;
            float.TryParse(Prnt, out fPrnt);
            float.TryParse(V2, out fV2);

            var f = 100f / fPrnt;
            var ctc = _analise.CTC;

            var x = fV2 - _analise.V;
            var s = (x) * ctc;
            var k = 100f * f;
            var j = (s / k);
            if (_values.Keys.Contains(int.Parse(Profundidade)))
                j *= _values[int.Parse(Profundidade)];
            else
                j *= _values[20];
            QuantidadeCal = j.ToString("###.###");
        }



        #endregion
    }
}