using System;
using Realms;
using Solum.Handlers;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class InterpretacaoViewModel : BaseViewModel
    {
        public InterpretacaoViewModel(INavigation navigation, Analise analise) : base(navigation)
        {
            var realm = Realm.GetInstance();
            Analise = realm.Find<Analise>(analise.Id);
            FazendaName = Analise.Talhao.Fazenda.Nome;
            Date = Analise.DataRegistro;
            TalhaoName = Analise.Talhao.Nome;
            InterpretacaoTextura = InterpretaHandler.InterpretaTextura(analise.Argila, analise.Silte);
            InterpretacaoPh = InterpretaHandler.InterpretaPh(analise.PotencialHidrogenico);
            InterpretacaoP = InterpretaHandler.InterpretaP(analise.Fosforo, InterpretacaoTextura);
            InterpretacaoK = InterpretaHandler.InterpretaK(analise.Potassio, analise.CTC);
            InterpretacaoCa = InterpretaHandler.InterpretaCa(analise.Calcio);
            InterpretacaoMg = InterpretaHandler.InterpretaMg(analise.Magnesio);
            InterpretacaoCaK = InterpretaHandler.InterpretaCaK(analise.CaK);
            InterpretacaoMgK = InterpretaHandler.InterpretaMgK(analise.MgK);
            InterpretacaoM = InterpretaHandler.InterpretaM(analise.M);
            InterpretacaoV = InterpretaHandler.InterpretaV(analise.V);
            InterpretacaoCtc = InterpretaHandler.InterpretaCtc(analise.CTC, InterpretacaoTextura);
            InterpretacaoMo = InterpretaHandler.InterpretaMo(analise.MateriaOrganica, InterpretacaoTextura);
            using (var transaction = realm.BeginWrite())
            {
                Analise.DataInterpretacao = DateTimeOffset.Now;
                transaction.Commit();
            }
        }

        #region Private Properties

        private Analise _analise;
        private string _fazendaName;
        private string _talhaoName;
        private DateTimeOffset _date;
        private string _interpretacaoCa;
        private string _interpretacaoCaK;
        private string _interpretacaoCtc;
        private string _interpretacaoK;
        private string _interpretacaoM;
        private string _interpretacaoMg;
        private string _interpretacaoMgK;
        private string _interpretacaoMo;
        private string _interpretacaoP;
        private string _interpretacaoPh;
        private string _interpretacaoTextura;
        private string _interpretacaoV;

        #endregion

        #region Binding Properties

        public Analise Analise
        {
            get { return _analise; }
            set { SetPropertyChanged(ref _analise, value); }
        }

        public string FazendaName
        {
            get { return _fazendaName;}
            set { SetPropertyChanged(ref _fazendaName, value); }
        }

        public string TalhaoName
        {
            get { return _talhaoName; }
            set { SetPropertyChanged(ref _talhaoName, value); }
        }

        public DateTimeOffset Date
        {
            get { return _date; }
            set { SetPropertyChanged(ref _date, value); }
        }

        public string InterpretacaoTextura
        {
            get { return _interpretacaoTextura; }
            set { SetPropertyChanged(ref _interpretacaoTextura, value); }
        }

        public string InterpretacaoPh
        {
            get { return _interpretacaoPh; }
            set { SetPropertyChanged(ref _interpretacaoPh, value); }
        }

        public string InterpretacaoP
        {
            get { return _interpretacaoP; }
            set { SetPropertyChanged(ref _interpretacaoP, value); }
        }

        public string InterpretacaoK
        {
            get { return _interpretacaoK; }
            set { SetPropertyChanged(ref _interpretacaoK, value); }
        }

        public string InterpretacaoCa
        {
            get { return _interpretacaoCa; }
            set { SetPropertyChanged(ref _interpretacaoCa, value); }
        }

        public string InterpretacaoMg
        {
            get { return _interpretacaoMg; }
            set { SetPropertyChanged(ref _interpretacaoMg, value); }
        }

        public string InterpretacaoCaK
        {
            get { return _interpretacaoCaK; }
            set { SetPropertyChanged(ref _interpretacaoCaK, value); }
        }

        public string InterpretacaoMgK
        {
            get { return _interpretacaoMgK; }
            set { SetPropertyChanged(ref _interpretacaoMgK, value); }
        }

        public string InterpretacaoM
        {
            get { return _interpretacaoM; }
            set { SetPropertyChanged(ref _interpretacaoM, value); }
        }

        public string InterpretacaoV
        {
            get { return _interpretacaoV; }
            set { SetPropertyChanged(ref _interpretacaoV, value); }
        }

        public string InterpretacaoCtc
        {
            get { return _interpretacaoCtc; }
            set { SetPropertyChanged(ref _interpretacaoCtc, value); }
        }

        public string InterpretacaoMo
        {
            get { return _interpretacaoMo; }
            set { SetPropertyChanged(ref _interpretacaoMo, value); }
        }

        #endregion
    }
}