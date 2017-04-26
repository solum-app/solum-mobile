using System.Collections.ObjectModel;
using System.Linq;
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
            IsLoading = true;
            AzureService.Instance.ListAnaliseAsync().ContinueWith((a) =>
            {
                var r = a.Result;
                Analises = r.Any() ? new ObservableCollection<Analise>(r) : new ObservableCollection<Analise>();
                HasItems = Analises.Any();
                IsLoading = false;
            });

        }


        public ObservableCollection<Analise> Analises
        {
            get => _analises;
            set => SetPropertyChanged(ref _analises, value);
        }

        public bool HasItems
        {
            get => _hasItems;
            set => SetPropertyChanged(ref _hasItems, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetPropertyChanged(ref _isLoading, value);
        }


        public ICommand EditCommand => _editCommand ?? (_editCommand = new Command(ShowEditAnalisePage));

        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new Command(DeleteAnalise));

        public ICommand ItemTappedCommand
            => _itemTappedCommand ?? (_itemTappedCommand = new Command(ShowGerenciamentoAnalisePage));


        //public async void UpdateAnalisesList()
        //{
        //    Analises = await AnaliseService.Instance.ReadAllAsync();
        //    HasItems = Analises.Any();
        //}

        private async void DeleteAnalise(object obj)
        {
            if (obj != null)
            {
                var analise = (Analise)obj;
                await AzureService.Instance.DeleteAnaliseAsync(analise);
                Analises.Remove(analise);
                HasItems = Analises.Any();
            }
        }

        private async void ShowEditAnalisePage(object obj)
        {
            if (obj != null)
            {
                var analise = (Analise)obj;
                await Navigation.PushAsync(new AnalisePage(analise.Id));
            }
        }

        private async void ShowGerenciamentoAnalisePage(object obj)
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                if (obj != null)
                {
                    var analise = (Analise)obj;
                    await Navigation.PushAsync(new GerenciamentoAnalisePage(analise.Id));
                }
                IsBusy = false;
            }
        }
    }
}