using System;
using Xamarin.Forms;
using Solum.Models;
using System.Threading.Tasks;
using Solum.Pages;
using System.Globalization;

namespace Solum.ViewModel
{
	public class AnaliseViewModel : BaseViewModel
	{
		Analise realmAnalise;

		public AnaliseViewModel (INavigation navigation):base(navigation)
		{
			Title = "Nova Análise";
		}

		public AnaliseViewModel (INavigation navigation, Analise analise) : base (navigation)
		{
			Title = "Edição de Análise";

			FazendaEntry = analise.Fazenda;
			TalhaoEntry = analise.Talhao;
			DataEntry = analise.Data;
			PhEntry = analise.Ph.ToString();
			PEntry = analise.P.ToString();
			KEntry = analise.K.ToString();
			CaEntry = analise.Ca.ToString();
			MgEntry = analise.Mg.ToString();
			AlEntry = analise.Al.ToString();
			HEntry = analise.H.ToString();
			MateriaOrganicaEntry = analise.MateriaOrganica.ToString();
			AreiaEntry = analise.Areia.ToString();
			SilteEntry = analise.Silte.ToString();
			ArgilaEntry = analise.Argila.ToString();

			realmAnalise = analise;
		}

		string _title;
		public string Title {
			get {
				return _title;
			}
			set {
				SetPropertyChanged (ref _title, value);
			}
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

		string _phEntry;
		public string PhEntry
		{
			get
			{
				return _phEntry;
			}
			set
			{
				SetPropertyChanged(ref _phEntry, string.Format("{0:0.00}", value));
			}
		}

		string _pEntry;
		public string PEntry
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

		string _kEntry;
		public string KEntry
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

		string _caEntry;
		public string CaEntry
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

		string _mgEntry;
		public string MgEntry
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

		string _alEntry;
		public string AlEntry
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

		string _hEntry;
		public string HEntry
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

		string _materiaOrganicaEntry;
		public string MateriaOrganicaEntry
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

		string _areiaEntry;
		public string AreiaEntry
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

		string _silteEntry;
		public string SilteEntry
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

		string _argilaEntry;
		public string ArgilaEntry
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

		Command _dateSelectedCommand;
		public Command DateSelectedCommand {
			get {
				return _dateSelectedCommand ?? (_dateSelectedCommand = new Command ((obj) => ExecuteDateSelectedCommand (obj)));
			}
		}

		protected void ExecuteDateSelectedCommand (object parameter)
		{
			_data = (DateTime)parameter;
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
			if (string.IsNullOrEmpty(FazendaEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um nome para a Fazenda", "OK");
				return;
			}
			if (string.IsNullOrEmpty(TalhaoEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira uma identificação para o Talhão", "OK");
				return;
			}
			if (DataEntry == default(DateTime))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para a Data", "OK");
				return;
			}
			if (string.IsNullOrEmpty(PhEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o pH", "OK");
				return;
			}
			if (string.IsNullOrEmpty(PEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o P", "OK");
				return;
			}
			if (string.IsNullOrEmpty(KEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o K", "OK");
				return;
			}
			if (string.IsNullOrEmpty(CaEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o Ca", "OK");
				return;
			}
			if (string.IsNullOrEmpty(MgEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o Mg", "OK");
				return;
			}
			if (string.IsNullOrEmpty(AlEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o Al", "OK");
				return;
			}
			if (string.IsNullOrEmpty(HEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para H", "OK");
				return;
			}
			if (string.IsNullOrEmpty(MateriaOrganicaEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para a Materia Orgânica", "OK");
				return;
			}
			if (string.IsNullOrEmpty(AreiaEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para a Areia", "OK");
				return;
			}
			if (string.IsNullOrEmpty(SilteEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para o Silte", "OK");
				return;
			}
			if (string.IsNullOrEmpty(ArgilaEntry))
			{
				await Application.Current.MainPage.DisplayAlert("Campo obrigatório não preenchido", "Insira um valor válido para a Argila", "OK");
				return;
			}

		
			var analise = new Analise () {
				Fazenda = FazendaEntry.Trim (),
				Talhao = TalhaoEntry,
				Data = DataEntry,
				Ph = float.Parse("0" + PhEntry.Replace(',', '.'), CultureInfo.InvariantCulture),
				P = float.Parse("0" + PEntry.Replace(',', '.'), CultureInfo.InvariantCulture),
				K = float.Parse("0" + KEntry.Replace(',', '.'), CultureInfo.InvariantCulture),
				Ca = float.Parse("0" + CaEntry.Replace(',', '.'), CultureInfo.InvariantCulture),
				Mg = float.Parse("0" + MgEntry.Replace(',', '.'), CultureInfo.InvariantCulture),
				Al = float.Parse("0" + AlEntry.Replace(',', '.'), CultureInfo.InvariantCulture),
				H = float.Parse("0" + HEntry.Replace(',', '.'), CultureInfo.InvariantCulture),
				MateriaOrganica = float.Parse("0" + MateriaOrganicaEntry.Replace(',', '.'), CultureInfo.InvariantCulture),
				Areia = float.Parse("0" + AreiaEntry.Replace(',', '.'), CultureInfo.InvariantCulture),
				Silte = float.Parse("0" + SilteEntry.Replace(',', '.'), CultureInfo.InvariantCulture),
				Argila = float.Parse("0" + ArgilaEntry.Replace(',', '.'), CultureInfo.InvariantCulture)
			};

			if (realmAnalise == default(Analise))
			{
				await Navigation.PushAsync (new InterpretacaoPage (analise, false));
			} else {
				await Navigation.PushAsync (new InterpretacaoPage (analise, realmAnalise, false));
			}
		}
	}
}

