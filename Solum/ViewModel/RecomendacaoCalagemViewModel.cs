using System;
using System.Collections.Generic;
using System.Windows.Input;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class RecomendacaoCalagemViewModel : BaseViewModel
    {
        private Analise _analise;

        private readonly IDictionary<int, float> _values = new Dictionary<int, float>
        {
            {5, 0.25f},
            {10, 0.5f},
            {20, 1.0f},
            {40, 2.0f}
        };


        private bool _enableButton;
        private string _prnt;
        private string _profundidade;
        private string _quantidade;

        private ICommand _salvarCommand;
        private string _v2;

        public RecomendacaoCalagemViewModel(INavigation navigation, string analiseId, float v2, float prnt,
            float profundidade, bool enableButton) : base(navigation)
        {
            AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(t =>
            {
                _analise = t.Result;
                PageTitle = _analise.Identificacao;
                Calculate();
            });
            
            V2 = v2.ToString();
            Prnt = prnt.ToString();
            Profundidade = profundidade.ToString();
            EnableButton = enableButton;
        }

        public ICommand SalvarCommand => _salvarCommand ?? (_salvarCommand = new Command(Salvar));


        public bool EnableButton
        {
            get => _enableButton;
            set => SetPropertyChanged(ref _enableButton, value);
        }

        public string QuantidadeCal
        {
            get => _quantidade;
            set => SetPropertyChanged(ref _quantidade, value);
        }

        public string V2
        {
            get => string.Format(_v2, "###.###");
            set => SetPropertyChanged(ref _v2, value);
        }

        public string Prnt
        {
            get => string.Format(_prnt, "###.###");
            set => SetPropertyChanged(ref _prnt, value);
        }

        public string Profundidade
        {
            get => string.Format(_profundidade, "##");
            set => SetPropertyChanged(ref _profundidade, value);
        }


        private async void Salvar()
        {
            if (!IsNotBusy) return;
            IsBusy = true;
            _analise.V2 = float.Parse(V2);
            _analise.Prnt = float.Parse(Prnt);
            _analise.Profundidade = int.Parse(Profundidade);
            _analise.DataCalculoCalagem = DateTimeOffset.Now;
            _analise.HasCalagem = true;
            await AzureService.Instance.UpdateAnaliseAsync(_analise);
            "Dados salvo com sucesso".ToToast(ToastNotificationType.Sucesso);
            await Navigation.PopAsync();
            IsBusy = false;
        }

        public void Calculate()
        {
            float fPrnt, fV2;
            float.TryParse(Prnt, out fPrnt);
            float.TryParse(V2, out fV2);

            var f = 100f / fPrnt;
            var ctc = _analise.CTC;

            var x = fV2 - _analise.V;
            var s = x * ctc;
            var k = 100f * f;
            var j = s / k;
            if (_values.Keys.Contains(int.Parse(Profundidade)))
                j *= _values[int.Parse(Profundidade)];
            else
                j *= _values[20];
            QuantidadeCal = j.ToString("###.###");
        }
    }
}