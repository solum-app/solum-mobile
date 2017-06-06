using System;
using Solum.Handlers;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class InterpretacaoViewModel : BaseViewModel
    {
        private DateTimeOffset _date;
        private string _fazendaName;
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
        private Nivel _nivelCa;
        private Nivel _nivelCaK;
        private Nivel _nivelCtc;
        private Nivel _nivelK;
        private Nivel _nivelM;
        private Nivel _nivelMg;
        private Nivel _nivelMgK;
        private Nivel _nivelMo;
        private Nivel _nivelP;
        private Nivel _nivelPh;
        private Nivel _nivelV;
        private string _talhaoName;
        private Textura _textura;
        private Analise _analise;

        public InterpretacaoViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
            AzureService.Instance.FindAnaliseAsync(analiseId)
	            .ContinueWith(async (task) =>
                {
					var analise = task.Result;
					var talhao = await AzureService.Instance.FindTalhaoAsync(analise.TalhaoId);
					var fazenda = await AzureService.Instance.FindFazendaAsync(talhao.FazendaId);

					InitializeAnalise(analise, talhao, fazenda);	

					if (!analise.WasInterpreted)
					{
						analise.DataInterpretacao = DateTimeOffset.Now;
						analise.WasInterpreted = true;
					await AzureService.Instance.AddOrUpdateAnaliseAsync(analise);
					}
                });
        }

		public void InitializeAnalise(Analise analise, Talhao talhao, Fazenda fazenda)
		{
			FazendaName = fazenda.Nome;
			TalhaoName = talhao.Nome;
            Analise = analise;
			Date = analise.DataRegistro;
	        Textura = Interpretador.Textura(analise.Argila, analise.Silte);
	        NivelPh = Interpretador.NivelPotencialHidrogenico(analise.PotencialHidrogenico);
	        NivelP = Interpretador.NiveFosforo(analise.Fosforo, Textura);
	        NivelK = Interpretador.NivelPotassio(analise.Potassio, analise.CTC);
	        NivelCa = Interpretador.NivelCalcio(analise.Calcio);
	        NivelMg = Interpretador.NivelMagnesio(analise.Magnesio);
	        NivelMo = Interpretador.NivelMateriaOrganica(analise.MateriaOrganica, Textura);
	        NivelCtc = Interpretador.NivelCtc(analise.CTC, Textura);
	        NivelV = Interpretador.NivelV(analise.V);
	        NivelM = Interpretador.NivelM(analise.M);
	        NivelCaK = Interpretador.NivelCalcioPotassio(analise.CaK);
	        NivelMgk = Interpretador.NivelMagnesioPotassio(analise.MgK);
	        InterpretacaoTextura = Interpretador.TexturaConverter(Textura);
			InterpretacaoPh = Interpretador.NivelPhConverter(NivelPh);
			InterpretacaoP = Interpretador.NivelConverter(NivelP);
			InterpretacaoK = Interpretador.NivelConverter(NivelK);
			InterpretacaoCa = Interpretador.NivelConverter(NivelCa);
			InterpretacaoMg = Interpretador.NivelConverter(NivelMg);
			InterpretacaoCaK = Interpretador.NivelConverter(NivelCaK);
			InterpretacaoMgK = Interpretador.NivelConverter(NivelMgk);
			InterpretacaoM = Interpretador.NivelConverter(NivelM);
			InterpretacaoV = Interpretador.NivelConverter(NivelV);
			InterpretacaoCtc = Interpretador.NivelConverter(NivelCtc);
			InterpretacaoMo = Interpretador.NivelConverter(NivelMo);
		}

        public Analise Analise
		{
            get { return _analise; }
            set { SetPropertyChanged(ref _analise, value); }
		}

        public Nivel NivelCa
        {
			get { return _nivelCa; }
			set { SetPropertyChanged(ref _nivelCa, value); }
        }

        public Nivel NivelCaK
        {
			get { return _nivelCaK; }
			set { SetPropertyChanged(ref _nivelCaK, value); }
        }

        public Nivel NivelCtc
        {
			get { return _nivelCtc; }
			set { SetPropertyChanged(ref _nivelCtc, value); }
        }

        public Nivel NivelK
        {
			get { return _nivelK; }
			set { SetPropertyChanged(ref _nivelK, value); }
        }

        public Nivel NivelM
        {
			get { return _nivelM; }
			set { SetPropertyChanged(ref _nivelM, value); }
        }

        public Nivel NivelMg
        {
			get { return _nivelMg; }
			set { SetPropertyChanged(ref _nivelMg, value); }
        }

        public Nivel NivelMgk
        {
			get { return _nivelMgK; }
			set { SetPropertyChanged(ref _nivelMgK, value); }
        }

        public Nivel NivelMo
        {
			get { return _nivelMo; }
			set { SetPropertyChanged(ref _nivelMo, value); }
        }

        public Nivel NivelP
        {
			get { return _nivelP; }
			set { SetPropertyChanged(ref _nivelP, value); }
        }

        public Nivel NivelPh
        {
			get { return _nivelPh; }
			set { SetPropertyChanged(ref _nivelPh, value); }
        }

        public Nivel NivelV
        {
			get { return _nivelV; }
			set { SetPropertyChanged(ref _nivelV, value); }
        }

        public Textura Textura
        {
			get { return _textura; }
			set { SetPropertyChanged(ref _textura, value); }
        }

        public string FazendaName
        {
			get { return _fazendaName; }
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
    }
}