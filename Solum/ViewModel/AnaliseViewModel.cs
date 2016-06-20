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
		DateTime _data = DateTime.Now;
		public DateTime DataEntry {
			get{
				return _data;
			}
			set{
				SetPropertyChanged(ref _data, value);
			}
		}

		float _phEntry;
		public float PhEntry
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

		float _pEntry;
		public float PEntry
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

		float _kEntry;
		public float KEntry
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

		float _caEntry;
		public float CaEntry
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

		float _mgEntry;
		public float MgEntry
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

		float _alEntry;
		public float AlEntry
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

		float _hEntry;
		public float HEntry
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

		float _materiaOrganicaEntry;
		public float MateriaOrganicaEntry
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

		float _areiaEntry;
		public float AreiaEntry
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

		float _silteEntry;
		public float SilteEntry
		{
			get
			{
				return _silteEntry;
			}
			set
			{
				SetPropertyChanged(ref _silteEntry, value);
			}
		}

		float _argilaEntry;
		public float ArgilaEntry
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
			if (PhEntry == default(float))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para pH", "OK");
				return;
			}
			if (PEntry == default(float))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para P", "OK");
				return;
			}
			if (KEntry == default(float))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para K", "OK");
				return;
			}
			if (CaEntry == default(float))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para Ca", "OK");
				return;
			}
			if (MgEntry == default(float))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para Mg", "OK");
				return;
			}
			if (AlEntry == default(float))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para Al", "OK");
				return;
			}
			if (HEntry == default(float))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para H", "OK");
				return;
			}
			if (MateriaOrganicaEntry == default(float))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para Materia Orgânica", "OK");
				return;
			}
			if (AreiaEntry == default(float))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para Areia", "OK");
				return;
			}
			if (SilteEntry == default(float))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para Silte", "OK");
				return;
			}
			if (ArgilaEntry == default(float))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para Argila", "OK");
				return;
			}

			var analise = new Analise (){
				Fazenda = FazendaEntry,
				Talhao = TalhaoEntry,
				Data = DataEntry,
				Ph = PhEntry,
				P = PEntry,
				K = KEntry,
				Ca = CaEntry,
				Mg = MgEntry,
				Al = AlEntry,
				H = HEntry,
				MateriaOrganica = MateriaOrganicaEntry,
				Areia = AreiaEntry,
				Silte = SilteEntry,
				Argila = ArgilaEntry
			};

			await Navigation.PushAsync (new InterpretacaoPage (analise));
		}
	}
}

