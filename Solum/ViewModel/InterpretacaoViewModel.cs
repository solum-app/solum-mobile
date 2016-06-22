using System;
using Xamarin.Forms;
using Solum.Models;
using Solum.Handlers;
using System.Threading.Tasks;
using Realms;
using System.Linq;
using Solum.Messages;

namespace Solum.ViewModel
{
	public class InterpretacaoViewModel : BaseViewModel
	{

		Analise realmAnalise;

		public InterpretacaoViewModel (INavigation navigation, Analise analise) : base(navigation)
		{
			Init (analise);
		}

		public InterpretacaoViewModel (INavigation navigation, Analise analise, Analise realmAnalise) : base (navigation)
		{
			this.realmAnalise = realmAnalise;
			Init (analise);
		}

		void Init(Analise analise) {
			Analise = analise;
			InterpretacaoTextura = InterpretaHandler.InterpretaTextura (analise.Argila, analise.Silte);
			InterpretacaoPh = InterpretaHandler.InterpretaPh (analise.Ph);
			InterpretacaoP = InterpretaHandler.InterpretaP (analise.P, InterpretacaoTextura);
			InterpretacaoK = InterpretaHandler.InterpretaK (analise.K, analise.CTC);
			InterpretacaoCa = InterpretaHandler.InterpretaCa (analise.Ca);
			InterpretacaoMg = InterpretaHandler.InterpretaMg (analise.Mg);
			InterpretacaoCaK = InterpretaHandler.InterpretaCaK (analise.CaK);
			InterpretacaoMgK = InterpretaHandler.InterpretaMgK (analise.MgK);
			InterpretacaoM = InterpretaHandler.InterpretaM (analise.M);
			InterpretacaoV = InterpretaHandler.InterpretaV (analise.V);
			InterpretacaoCtc = InterpretaHandler.InterpretaCtc (analise.CTC, InterpretacaoTextura);
			InterpretacaoMo = InterpretaHandler.InterpretaMo (analise.MateriaOrganica, InterpretacaoTextura);
		}

		Analise _analise;
		public Analise Analise
		{
			get
			{
				return _analise;
			}
			set
			{
				SetPropertyChanged(ref _analise, value);
			}
		}

		string _interpretacaoTextura;
		public string InterpretacaoTextura
		{
			get
			{
				return _interpretacaoTextura;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoTextura, value);
			}
		}

		string _interpretacaoPh;
		public string InterpretacaoPh
		{
			get
			{
				return _interpretacaoPh;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoPh, value);
			}
		}

		string _interpretacaoP;
		public string InterpretacaoP
		{
			get
			{
				return _interpretacaoP;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoP, value);
			}
		}

		string _interpretacaoK;
		public string InterpretacaoK
		{
			get
			{
				return _interpretacaoK;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoK, value);
			}
		}

		string _interpretacaoCa;
		public string InterpretacaoCa
		{
			get
			{
				return _interpretacaoCa;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoCa, value);
			}
		}

		string _interpretacaoMg;
		public string InterpretacaoMg
		{
			get
			{
				return _interpretacaoMg;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoMg, value);
			}
		}

		string _interpretacaoCaK;
		public string InterpretacaoCaK
		{
			get
			{
				return _interpretacaoCaK;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoCaK, value);
			}
		}

		string _interpretacaoMgK;
		public string InterpretacaoMgK
		{
			get
			{
				return _interpretacaoMgK;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoMgK, value);
			}
		}
			
		string _interpretacaoM;
		public string InterpretacaoM
		{
			get
			{
				return _interpretacaoM;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoM, value);
			}
		}

		string _interpretacaoV;
		public string InterpretacaoV
		{
			get
			{
				return _interpretacaoV;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoV, value);
			}
		}
			
		string _interpretacaoCtc;
		public string InterpretacaoCtc
		{
			get
			{
				return _interpretacaoCtc;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoCtc, value);
			}
		}

		string _interpretacaoMo;
		public string InterpretacaoMo
		{
			get
			{
				return _interpretacaoMo;
			}
			set
			{
				SetPropertyChanged(ref _interpretacaoMo, value);
			}
		}

		public void SalvarAnalise ()
		{
			var realm = Realm.GetInstance ();

			if (realmAnalise == default(Analise)){
				using (var transaction = realm.BeginWrite ()) {
					realmAnalise = realm.CreateObject<Analise> ();
					realmAnalise.Al = Analise.Al;
					realmAnalise.Areia = Analise.Areia;
					realmAnalise.Argila = Analise.Argila;
					realmAnalise.Ca = Analise.Ca;
					realmAnalise.Data = Analise.Data;
					realmAnalise.Fazenda = Analise.Fazenda;
					realmAnalise.H = Analise.H;
					realmAnalise.K = Analise.K;
					realmAnalise.MateriaOrganica = Analise.MateriaOrganica;
					realmAnalise.Mg = Analise.Mg;
					realmAnalise.P = Analise.P;
					realmAnalise.Ph = Analise.Ph;
					realmAnalise.Silte = Analise.Silte;
					realmAnalise.Talhao = Analise.Talhao;

					transaction.Commit ();
				}
			} else {
				using (var transaction = realm.BeginWrite ()) {
					realmAnalise.Al = Analise.Al;
					realmAnalise.Areia = Analise.Areia;
					realmAnalise.Argila = Analise.Argila;
					realmAnalise.Ca = Analise.Ca;
					realmAnalise.Data = Analise.Data;
					realmAnalise.Fazenda = Analise.Fazenda;
					realmAnalise.H = Analise.H;
					realmAnalise.K = Analise.K;
					realmAnalise.MateriaOrganica = Analise.MateriaOrganica;
					realmAnalise.Mg = Analise.Mg;
					realmAnalise.P = Analise.P;
					realmAnalise.Ph = Analise.Ph;
					realmAnalise.Silte = Analise.Silte;
					realmAnalise.Talhao = Analise.Talhao;

					transaction.Commit ();
				}
			}

			MessagingCenter.Send<UpdateAnalisesMessage> (
				new UpdateAnalisesMessage (), 
				UpdateAnalisesMessage.UpdateAnalises
			);
		}
	}
}

