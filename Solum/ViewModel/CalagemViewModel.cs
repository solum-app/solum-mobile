using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class CalagemViewModel : BaseViewModel
    {
        public CalagemViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
            var realm = Realm.GetInstance();
            _analise = realm.Find<Analise>(analiseId);
            PageTitle = _analise.Identificacao;

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

            if (_analise.HasCalagem)
            {
                V2Item = V2List.FirstOrDefault(x => x.Value.Equals(_analise.V2));
                ProfundidadeItem = ProfundidadeList.FirstOrDefault(x => x.Value.Equals(_analise.Profundidade));
                Prnt = $"{_analise.Prnt} %";
            }
        }

        #region Commands

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new Command(Save));

        #endregion

        #region Functions

        private async void Save()
        {
            if (V2Item == null)
            {
                "Você deve selecionar um valor para V2".ToDisplayAlert(MessageType.Aviso);
                return;
            }
            if (string.IsNullOrEmpty(Prnt))
            {
                "Você deve adicionar um valor para PRNT".ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (ProfundidadeItem == null)
            {
                "Você deve selecionar um valor para profundidade".ToDisplayAlert(MessageType.Aviso);
                return;
            }

            Prnt = Prnt.Replace("%", "").Trim();
            if (int.Parse(Prnt) <= 0 || int.Parse(Prnt) > 100)
            {
                "O valor de PRNT deve estar entre 0 e 100".ToDisplayAlert(MessageType.Aviso);
                return;
            }

            //using (var transaction = _realm.BeginWrite())
            //{
            //    _analise.DataCalculoCalagem = DateTimeOffset.Now;
            //    _analise.HasCalagem = true;
            //    _analise.Prnt = int.Parse(Prnt);
            //    _analise.V2 = V2Item.Value;
            //    _analise.Profundidade = (int) ProfundidadeItem.Value;
            //    transaction.Commit();
            //}

            if (IsNotBusy)
            {
                IsBusy = true;
                var current = Navigation.NavigationStack.LastOrDefault();
                await Navigation.PushAsync(new RecomendaCalagemPage(_analise.Id, V2Item.Value, float.Parse(Prnt), ProfundidadeItem.Value, true));
                Navigation.RemovePage(current);
                IsBusy = false;
            }
        }

        #endregion

        #region Private properties

        private string _prnt;

        private DisplayNumber _v2Item;
        private DisplayNumber _profundidadeItem;

        private IList<DisplayNumber> _v2List;
        private IList<DisplayNumber> _profundidadeList;

        private ICommand _saveCommand;

        private readonly Analise _analise;

        #endregion

        #region Binding Properties

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
            set { SetPropertyChanged(ref _prnt, value); }
        }

        #endregion
    }
}