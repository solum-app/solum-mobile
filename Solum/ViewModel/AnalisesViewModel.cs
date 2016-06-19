using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using Solum.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Solum.ViewModel
{
	public class AnalisesViewModel : BaseViewModel
	{
		public AnalisesViewModel (INavigation navigation) : base(navigation)
		{
			var list = new List<Analise> ();

			for (int i = 0; i < 5; i++) {
				var analise = new Analise ();
				analise.Fazenda = "Fazenda Santo Augustinho";
				analise.Talhao = "Talhão " + (i + 1);
				analise.Data = DateTime.Now;

				list.Add (analise);

				var analise2 = new Analise ();
				analise2.Fazenda = "Fazenda Esperança";
				analise2.Talhao = "Talhão " + (i + 1);
				analise2.Data = DateTime.Now;

				list.Add (analise2);
			}

			var groupList =
				list.OrderBy (a => a.Fazenda)
					.GroupBy (a => a.Fazenda).ToList ();

			Analises = new ObservableCollection<IGrouping<string, Analise>> (groupList);
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
				return _excluirCommand ?? (_excluirCommand = new Command (async (obj) => await ExecuteExcluirCommand (obj)));
			}
		}

		protected async Task ExecuteExcluirCommand (object obj)
		{
			Debug.WriteLine ("Excluir");
		}

		private Command _editarCommand;

		public Command EditarCommand {
			get {
				return _editarCommand ?? (_editarCommand = new Command (async (obj) => await ExecuteEditarCommand (obj)));
			}
		}

		protected async Task ExecuteEditarCommand (object obj)
		{
			Debug.WriteLine ("Editar");
		}
	}
}

