﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class SemeaduraViewModel : BaseViewModel
    {
        public SemeaduraViewModel(INavigation navigation, string analiseId, bool enableButton) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _analise = _realm.Find<Analise>(analiseId);
            var p = InterpretaHandler.InterpretaK(_analise.Potassio, _analise.CTC);
            IsPotassioBaixo = !p.ToUpper().Equals("ADEQUADO") || !p.ToUpper().Equals("ALTO");
            PageTitle = _analise.Identificacao;
            Expectativas = new List<int> {6, 8, 10, 12};
            Culturas = new List<string> {"Milho"};
        }

        #region private properties

        private IList<int> _expectativas;
        private int _expectativaSelected;

        private IList<string> _culturas;
        private string _culturaSelected;

        private bool _isPotassioBaixo;
        private readonly Analise _analise;

        private ICommand _recomendarCommand;

        private readonly Realm _realm;
        private bool _enableButton;

        #endregion

        #region binding properties

        public bool EnableButton
        {
            get { return _enableButton; }
            set { SetPropertyChanged(ref _enableButton, value); }
        }

        public IList<int> Expectativas
        {
            get { return _expectativas; }
            set { SetPropertyChanged(ref _expectativas, value); }
        }

        public int ExpectativaSelected
        {
            get { return _expectativaSelected;}
            set { SetPropertyChanged(ref _expectativaSelected, value); }
        }

        public IList<string> Culturas
        {
            get { return _culturas;}
            set { SetPropertyChanged(ref _culturas, value); }
        }

        public string CulturaSelected
        {
            get { return _culturaSelected;}
            set { SetPropertyChanged(ref _culturaSelected, value); }
        }

        public bool IsPotassioBaixo
        {
            get { return _isPotassioBaixo;}
            set { SetPropertyChanged(ref _isPotassioBaixo, value); }
        }

        #endregion

        #region commands

        public ICommand RecomendarCommand => _recomendarCommand ?? (_recomendarCommand = new Command(Recomendar));

        #endregion

        #region functions

        private async void Recomendar()
        {
            if (ExpectativaSelected == 0)
            {
                "Selecione a sua expectativa".ToDisplayAlert(MessageType.Aviso);
                return;
            }
            if (string.IsNullOrEmpty(CulturaSelected))
            {
                "Selecione a cultura".ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (IsNotBusy)
            {
                IsBusy = true;
                var current = Navigation.NavigationStack.LastOrDefault();
                await Navigation.PushAsync(new RecomendaSemeaduraPage(_analise.Id, ExpectativaSelected, CulturaSelected, true));
                Navigation.RemovePage(current);
                IsBusy = false;
            }
        }

        #endregion
    }
}