using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class CalagemViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private Analise _analise;
        private string _prnt;
        private DisplayNumber _profundidadeItem;
        private IList<DisplayNumber> _profundidadeList;
        private ICommand _saveCommand;
        private DisplayNumber _v2Item;
        private IList<DisplayNumber> _v2List;

        public CalagemViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
            V2List = new List<DisplayNumber>();
            for (var i = 30; i <= 80; i += 5)
                V2List.Add(new DisplayNumber($"{i} %", i));

            ProfundidadeList = new List<DisplayNumber>
            {
                new DisplayNumber("5 cm", 5),
                new DisplayNumber("10 cm", 10),
                new DisplayNumber("20 cm", 20),
                new DisplayNumber("40 cm", 40)
            };

            AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(t =>
            {
                _analise = t.Result;
                PageTitle = _analise.Identificacao;
                if (_analise.HasCalagem)
                {
                    V2Item = V2List.FirstOrDefault(x => x.Value.Equals(_analise.V2));
                    ProfundidadeItem = ProfundidadeList.FirstOrDefault(x => x.Value.Equals(_analise.Profundidade));
					Prnt = $"{_analise.Prnt} %";
                }
            });

            _userDialogs = DependencyService.Get<IUserDialogs>();

        }


		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new Command(async () => await Save()));


        public DisplayNumber ProfundidadeItem
        {
			get { return _profundidadeItem; }
			set { SetPropertyChanged(ref _profundidadeItem, value); }
        }

        public DisplayNumber V2Item
        {
			get { return _v2Item; }
			set { SetPropertyChanged(ref _v2Item, value); }
        }

        public IList<DisplayNumber> ProfundidadeList
        {
			get { return _profundidadeList; }
			set { SetPropertyChanged(ref _profundidadeList, value); }
        }
        public IList<DisplayNumber> V2List
        {
			get { return _v2List; }
			set { SetPropertyChanged(ref _v2List, value); }
        }

        public string Prnt
        {
			get { return _prnt; }
			set
			{
				if (value.Contains(" %"))
				{
					value = value.Replace(" %", "");
				}
				else if (value.Length > 1)
				{
					value = value.Remove(value.Length - 2);
				}

				if (!string.IsNullOrEmpty(value))
				{
					SetPropertyChanged(ref _prnt, $"{value} %");
				}
				else
				{
					SetPropertyChanged(ref _prnt, value);
				}
			}
		
        }

        private async Task Save()
        {
            if (V2Item == null)
            {
                await _userDialogs.DisplayAlert(MessagesResource.V2Null);
                return;
            }
            if (string.IsNullOrEmpty(Prnt))
            {
                await _userDialogs.DisplayAlert(MessagesResource.PRNTNull);
                return;
            }

            if (ProfundidadeItem == null)
            {
                await _userDialogs.DisplayAlert(MessagesResource.ProfundidadeNull);
                return;
            }

            var prnt = int.Parse(Prnt.Replace(" %", ""));
            if (prnt <= 0 || prnt > 100)
            {
                await _userDialogs.DisplayAlert(MessagesResource.PRNTInvalido);
                return;
            }

            IsBusy = true;

            _analise.DataCalculoCalagem = DateTimeOffset.Now;
            _analise.HasCalagem = true;
            _analise.Prnt = prnt;
            _analise.V2 = V2Item.Value;
            _analise.Profundidade = (int)ProfundidadeItem.Value;
			await AzureService.Instance.AddOrUpdateAnaliseAsync(_analise);
            await Navigation.PushAsync(new RecomendacaoCalagemPage(_analise.Id, _analise.V2, prnt, _analise.Profundidade, false));
            
            IsBusy = false;
        }
    }
}