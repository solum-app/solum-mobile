using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using Solum.Models;
using System.Linq;
using System.Collections.ObjectModel;
using Realms;
using Solum.Pages;
using Solum.Messages;
using Solum.Handlers;

namespace Solum.ViewModel
{
	public class AnalisesViewModel : BaseViewModel
	{

		IList<Analise> analises;

		public AnalisesViewModel (INavigation navigation) : base(navigation)
		{
			MessagingCenter.Subscribe<UpdateAnalisesMessage> (
				this, UpdateAnalisesMessage.UpdateAnalises, UpdateAnalises
			);

			var realm = Realm.GetInstance ();

			analises = realm.All<Analise> ().OrderBy (e => e.Talhao.Fazenda.Nome).ToList ();

			var groupList =
				analises.GroupBy (a => a.Talhao.Fazenda.Nome.ToUpper ())
					.Select (a => new Grouping<string, Analise> (a.Key, a));

			Analises = new ObservableCollection<Grouping<string, Analise>> (groupList);

			HasItems = Analises.Count > 0;
		}

		void UpdateAnalises (object sender)
		{
			var realm = Realm.GetInstance ();

			analises = realm.All<Analise> ().OrderBy (e => e.Talhao.Fazenda.Nome).ToList ();

			var groupList =
				analises.GroupBy (a => a.Talhao.Fazenda.Nome.ToUpper ())
					.Select (a => new Grouping<string, Analise> (a.Key, a));

			Analises = new ObservableCollection<Grouping<string, Analise>> (groupList);

			HasItems = Analises.Count > 0;
		}

		IList<Grouping<string, Analise>> _analises;
		public IList<Grouping<string, Analise>> Analises {
			get {
				return _analises;
			}
			set {
				SetPropertyChanged (ref _analises, value);
			}
		}

		bool _hasItems;
		public bool HasItems
		{
			get
			{
				return _hasItems;
			}
			set
			{
				SetPropertyChanged(ref _hasItems, value);
			}
		}

		private Command _excluirCommand;

		public Command ExcluirCommand {
			get {
				return _excluirCommand ?? (_excluirCommand = new Command ((obj) => ExecuteExcluirCommand (obj)));
			}
		}

		protected void ExecuteExcluirCommand (object obj)
		{
			var analise = (obj as Analise);

			var realm = Realm.GetInstance ();

			using (var trans = realm.BeginWrite ()) {
				realm.Remove (analise);
				trans.Commit ();
			}

			analises.Remove (analise);

			var groupList =
				analises.GroupBy (a => a.Talhao.Fazenda.Nome.ToUpper ())
					.Select (a => new Grouping<string, Analise> (a.Key, a));

			Analises = new ObservableCollection<Grouping<string, Analise>> (groupList);

			HasItems = Analises.Count > 0;
		}

		private Command _editarCommand;

		public Command EditarCommand {
			get {
				return _editarCommand ?? (_editarCommand = new Command (async (obj) => await ExecuteEditarCommand (obj)));
			}
		}

		protected async Task ExecuteEditarCommand (object obj)
		{
			var analise = (obj as Analise);

			await Navigation.PushAsync (new AnalisePage (analise));
		}

		private Command _itemTappedCommand;

		public Command ItemTappedCommand {
			get {
				return _itemTappedCommand ?? (_itemTappedCommand = new Command (async (obj) => await ExecuteItemTappedCommand (obj)));
			}
		}

		protected async Task ExecuteItemTappedCommand (object obj)
		{
			var analise = (obj as Analise);

			await Navigation.PushAsync (new InterpretacaoPage (analise, true));
		}
	}
}

