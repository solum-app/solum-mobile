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
		Analise analise;

		public AnaliseViewModel (INavigation navigation):base(navigation)
		{
			
		}

		public AnaliseViewModel (INavigation navigation, Analise analise) : base (navigation)
		{
			FazendaEntry = analise.Fazenda;
			TalhaoEntry = analise.Talhao;
			DataEntry = analise.Data;
			PhEntry = analise.Ph;
			PEntry = analise.P;
			KEntry = analise.K;
			CaEntry = analise.Ca;
			MgEntry = analise.Mg;
			AlEntry = analise.Al;
			HEntry = analise.H;
			MateriaOrganicaEntry = analise.MateriaOrganica;
			AreiaEntry = analise.Areia;
			SilteEntry = analise.Silte;
			ArgilaEntry = analise.Argila;

			this.analise = analise;
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
		DateTimeOffset _data = DateTimeOffset.Now;
		public DateTimeOffset DataEntry {
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
			//if (string.IsNullOrEmpty(FazendaEntry))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um nome para a Fazenda", "OK");
			//	return;
			//}
			//if (string.IsNullOrEmpty(TalhaoEntry))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira uma identificação para o Talhão", "OK");
			//	return;
			//}
			//if (DataEntry == default(DateTime))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para a Data", "OK");
			//	return;
			//}
			//if (PhEntry == default(float))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o pH", "OK");
			//	return;
			//}
			//if (PEntry == default(float))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o P", "OK");
			//	return;
			//}
			//if (KEntry == default(float))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o K", "OK");
			//	return;
			//}
			//if (CaEntry == default(float))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o Ca", "OK");
			//	return;
			//}
			//if (MgEntry == default(float))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o Mg", "OK");
			//	return;
			//}
			//if (AlEntry == default(float))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o Al", "OK");
			//	return;
			//}
			//if (HEntry == default(float))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para H", "OK");
			//	return;
			//}
			//if (MateriaOrganicaEntry == default(float))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para a Materia Orgânica", "OK");
			//	return;
			//}
			//if (AreiaEntry == default(float))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para a Areia", "OK");
			//	return;
			//}
			//if (SilteEntry == default(float))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o Silte", "OK");
			//	return;
			//}
			//if (ArgilaEntry == default(float))
			//{
			//	await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para a Argila", "OK");
			//	return;
			//}

		
			if (analise == default(Analise)){
				analise = new Analise () {
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
			}

			await Navigation.PushAsync (new InterpretacaoPage (analise));
		}
	}
}

