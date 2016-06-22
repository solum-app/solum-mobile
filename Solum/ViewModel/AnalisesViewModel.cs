using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using Solum.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Realms;
using Solum.Pages;
using Solum.Messages;

namespace Solum.ViewModel
{
	public class AnalisesViewModel : BaseViewModel
	{

		IList<Analise> analises;

		public AnalisesViewModel (INavigation navigation) : base(navigation)
		{
			var realm = Realm.GetInstance ();

			analises = realm.All<Analise> ().OrderBy (e => e.Fazenda).ToList ();

			Analises = new ObservableCollection<IGrouping<string, Analise>> (analises.GroupBy (e => e.Fazenda));

			MessagingCenter.Subscribe<UpdateAnalisesMessage> (
				this, UpdateAnalisesMessage.UpdateAnalises, UpdateAnalises
			);
		}

		void UpdateAnalises (object sender)
		{
			var realm = Realm.GetInstance ();

			analises = realm.All<Analise> ().OrderBy (e => e.Fazenda).ToList ();

			Analises = new ObservableCollection<IGrouping<string, Analise>> (analises.GroupBy (e => e.Fazenda));
		}

		IList<IGrouping<string, Analise>> _analises;
		public IList<IGrouping<string, Analise>> Analises {
			get {
				return _analises;
			}
			set {
				SetPropertyChanged (ref _analises, value);
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

			Analises = new ObservableCollection<IGrouping<string, Analise>> (analises.GroupBy (e => e.Fazenda));
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

