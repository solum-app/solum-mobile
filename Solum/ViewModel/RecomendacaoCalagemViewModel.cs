using System.Collections.Generic;
using Realms;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class RecomendacaoCalagemViewModel : BaseViewModel
    {
        public RecomendacaoCalagemViewModel(INavigation navigation, string calagemId) : base(navigation)
        {
            _realm = Realm.GetInstance();
            Calagem = _realm.Find<Calagem>(calagemId);
            var f = 100 / Calagem.Prnt;
            var v = Calagem.Analise.V;
            var ctc = Calagem.Analise.CTC;
            QuantidadeCal = ((Calagem.V2 - v) * ctc / 100 * f) * _values[Calagem.Profundidade];
        }

        #region private properties

        private readonly Realm _realm;
        private Calagem _calagem;
        private readonly IDictionary<int, float> _values = new Dictionary<int, float>()
        {
            { 5, 0.25f },
            { 10, 0.5f },
            { 20, 1.0f },
            { 40, 2.0f }
        };

        private float _quantidade;
        #endregion

        #region Binding Properties

        public string PageTitle => $"{Calagem.Analise.Talhao.Fazenda.Nome} - {Calagem.Analise.Talhao.Nome}";

        public float QuantidadeCal
        {
            get { return _quantidade; }
            set { SetPropertyChanged(ref _quantidade, value); }
        }

        public Calagem Calagem
        {
            get { return _calagem;}
            set { SetPropertyChanged(ref _calagem, value); }
        }

        #endregion
    }
}