using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class AnalisesViewModel : BaseViewModel
    {
        private ObservableCollection<Analise> _analises;
        private ICommand _deleteCommand;
        private ICommand _editCommand;
        private bool _hasItems;
        private bool _isLoading;
        private ICommand _itemTappedCommand;

        public AnalisesViewModel(INavigation navigation) : base(navigation)
        {
           UpdateAnalisesList();
        }

        public ObservableCollection<Analise> Analises
        {
			get { return _analises; }
			set { SetPropertyChanged(ref _analises, value); }
        }

        public bool HasItems
        {
			get { return _hasItems; }
			set { SetPropertyChanged(ref _hasItems, value); }
        }

        public bool IsLoading
        {
			get { return _isLoading; }
			set { SetPropertyChanged(ref _isLoading, value); }
        }

		public ICommand EditCommand => _editCommand ?? (_editCommand = new Command(async (obj) => await ShowEditAnalisePage(obj as Analise)));

		public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new Command(async (obj) => await DeleteAnalise(obj as Analise)));

		public ICommand ItemTappedCommand => _itemTappedCommand ?? (_itemTappedCommand =  new Command(async (obj) => await ShowGerenciamentoAnalisePage(obj as Analise)));

        public void UpdateAnalisesList()
        {
            if (!IsLoading)
            {
                IsLoading = true;
                HasItems = IsLoading;
                AzureService.Instance.ListAnaliseAsync()
                    .ContinueWith((a) =>
                    {
                        var r = a.Result;
                        Analises = r.Any() ? new ObservableCollection<Analise>(r) : new ObservableCollection<Analise>();
                        HasItems = Analises.Any();
                        IsLoading = false;
                    });
            }
        }

		private async Task DeleteAnalise(Analise analise)
        {
			var confirm = await Application.Current.MainPage.DisplayAlert("Confirmação", "Tem certeza que deseja excluir este item?\nTodos os dados relacionados à essa análise serão exlcuidos!", "Sim", "Não");
			if (confirm && analise != null)
			{
                await AzureService.Instance.DeleteAnaliseAsync(analise);
                Analises.Remove(analise);
                HasItems = Analises.Any();
            }
        }

		private async Task ShowEditAnalisePage(Analise analise)
        {
			if (analise != null)
            {
                await Navigation.PushAsync(new AnalisePage(analise.Id));
            }
        }

		private async Task ShowGerenciamentoAnalisePage(Analise analise)
        {
            if (IsNotBusy)
            {
                IsBusy = true;
				if (analise != null)
                {
                    await Navigation.PushAsync(new GerenciamentoAnalisePage(analise.Id));
                }
                IsBusy = false;
            }
        }
    }
}