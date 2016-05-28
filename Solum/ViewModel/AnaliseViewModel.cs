using System;
using Xamarin.Forms;
using Solum.Models;
using System.Threading.Tasks;
using Solum.Handler;
using Solum.Pages;

namespace Solum.ViewModel
{
	public class AnaliseViewModel : BaseViewModel
	{
		public AnaliseViewModel (INavigation navigation):base(navigation)
		{
			
		}

		string _fazenda;
		public string FazendaEntry {
			get{
				return _fazenda;
			}
			set{
				SetPropertyChanged(ref _fazenda, value);
			}
		}

		string _talhao;
		public string TalhaoEntry {
			get{
				return _talhao;
			}
			set{
				SetPropertyChanged(ref _talhao, value);
			}
		}
		DateTime _data;
		public DateTime DataEntry {
			get{
				return _data;
			}
			set{
				SetPropertyChanged(ref _data, value);
			}
		}

		float? _phEntry;
		public float? PhEntry
		{
			get
			{
				return _phEntry;
			}
			set
			{
				SetPropertyChanged(ref _phEntry, value);
			}
		}

		float? _pEntry;
		public float? PEntry
		{
			get
			{
				return _pEntry;
			}
			set
			{
				SetPropertyChanged(ref _pEntry, value);
			}
		}

		float? _kEntry;
		public float? KEntry
		{
			get
			{
				return _kEntry;
			}
			set
			{
				SetPropertyChanged(ref _kEntry, value);
			}
		}

		float? _caEntry;
		public float? CaEntry
		{
			get
			{
				return _caEntry;
			}
			set
			{
				SetPropertyChanged(ref _caEntry, value);
			}
		}

		float? _mgEntry;
		public float? MgEntry
		{
			get
			{
				return _mgEntry;
			}
			set
			{
				SetPropertyChanged(ref _mgEntry, value);
			}
		}

		float? _alEntry;
		public float? AlEntry
		{
			get
			{
				return _alEntry;
			}
			set
			{
				SetPropertyChanged(ref _alEntry, value);
			}
		}

		float? _hEntry;
		public float? HEntry
		{
			get
			{
				return _hEntry;
			}
			set
			{
				SetPropertyChanged(ref _hEntry, value);
			}
		}

		float? _materiaOrganicaEntry;
		public float? MateriaOrganicaEntry
		{
			get
			{
				return _materiaOrganicaEntry;
			}
			set
			{
				SetPropertyChanged(ref _materiaOrganicaEntry, value);
			}
		}

		float? _areiaEntry;
		public float? AreiaEntry
		{
			get
			{
				return _areiaEntry;
			}
			set
			{
				SetPropertyChanged(ref _areiaEntry, value);
			}
		}

		float? _siliteEntry;
		public float? SiliteEntry
		{
			get
			{
				return _siliteEntry;
			}
			set
			{
				SetPropertyChanged(ref _siliteEntry, value);
			}
		}

		float? _argilaEntry;
		public float? ArgilaEntry
		{
			get
			{
				return _argilaEntry;
			}
			set
			{
				SetPropertyChanged(ref _argilaEntry, value);
			}
		}

		public float SBResult
		{
			get
			{
				return CalculoHandler.CalcularSB(KEntry.Value, CaEntry.Value, MgEntry.Value);
			}
		}

		public float CTCResult
		{
			get
			{
				return CalculoHandler.CalcularCTC(SBResult, HEntry.Value, AlEntry.Value);
			}
		}

		public float VResult
		{
			get
			{
				return CalculoHandler.CalcularV(SBResult, CTCResult);
			}
		}

		public float MResult
		{
			get
			{
				return CalculoHandler.CalcularM(AlEntry.Value, SBResult);
			}
		}

		float? _t;
		public float? T
		{
			get
			{
				return _t;
			}
			set
			{
				SetPropertyChanged(ref _t, value);
			}
		}

		public float CaMgResult
		{
			get
			{
				return CalculoHandler.CalcularCaMg(CaEntry.Value, MgEntry.Value);
			}
		}

		public float CaKResult
		{
			get
			{
				return CalculoHandler.CalcularCaK(CaEntry.Value, KEntry.Value);
			}
		}

		public float MgKResult
		{
			get
			{
				return CalculoHandler.CalcularMgK(MgEntry.Value, KEntry.Value);
			}
		}

		Command _buttonClickedCommand;
		public Command ButtonClickedCommand
		{
			get
			{
				return _buttonClickedCommand ?? (_buttonClickedCommand = new Command(async () => await ExecuteButtonClickedCommand()));
			}
		}

		protected async Task ExecuteButtonClickedCommand()
		{	
			var analise = new Analise (){
				Fazenda = FazendaEntry,
				Talhao = TalhaoEntry,
				Data = DataEntry,
				Ph = PhEntry.Value,
				P = PEntry.Value,
				K = KEntry.Value,
				Ca = CaEntry.Value,
				Mg = MgEntry.Value,
				Al = AlEntry.Value,
				H = HEntry.Value,
				MateriaOrganica = MateriaOrganicaEntry.Value,
				Areia = AreiaEntry.Value,
				Silite = SiliteEntry.Value,
				Argila = ArgilaEntry.Value,
				SB = SBResult,
				CTC = CTCResult,
				V = VResult,
				M = MResult,
				CaMg = CaMgResult,
				CaK = CaKResult,
				MgK = MgKResult
			};

			await Navigation.PushAsync (new InterpretacaoPage (analise));
		}
	}
}

