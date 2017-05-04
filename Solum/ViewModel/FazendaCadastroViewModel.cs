﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Auth;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class FazendaCadastroViewModel : BaseViewModel
    {
        public FazendaCadastroViewModel(INavigation navigation, bool fromAnalise) : base(navigation)
        {
			Task.Run(LoadEstados);
            _fromAnalise = fromAnalise;
            PageTitle = "Nova Fazenda";
        }

        public FazendaCadastroViewModel(INavigation navigation, string fazendaId, bool fromAnalise) : base(navigation)
        {
            _isUpdate = true;
            _fromAnalise = fromAnalise;
            _fazenda = AzureService.Instance.FindFazendaAsync(fazendaId).Result;
            FazendaName = _fazenda.Nome;
            PageTitle = "Editar Fazenda";
            AzureService.Instance.ListEstadosAsync()
	            .ContinueWith(async(task) =>
                {
                    Estados = task.Result;
					CidadeSelected = await AzureService.Instance.FindCidadeAsync(_fazenda.CidadeId);
                    EstadoSelected = Estados.FirstOrDefault(t => t.Id.Equals(CidadeSelected.EstadoId));
					Cidades = await AzureService.Instance.ListCidadesAsync(EstadoSelected.Id);
                });
        }

        #region Propriedades Privadas

        private ICommand _registerFazendaCommand;
        private ICommand _updateCidadesCommand;
        private IList<Cidade> _cidades;
        private IList<Estado> _estados;
        private Cidade _cidadeSelected;
        private Estado _estadoSelected;
        private string _fazendaName;
        private bool _isEstadosLoaded;
        private bool _isCidadesLoaded;
        private readonly bool _isUpdate;
        private readonly Fazenda _fazenda;
        private readonly bool _fromAnalise;

        #endregion

        #region Propriedade de Binding

        public string FazendaName
        {
			get { return _fazendaName; }
			set { SetPropertyChanged(ref _fazendaName, value); }
        }

        public IList<Estado> Estados
        {
			get { return _estados; }
			set { SetPropertyChanged(ref _estados, value); }
        }

        public Estado EstadoSelected
        {
			get { return _estadoSelected; }
			set { SetPropertyChanged(ref _estadoSelected, value); }
        }

        public bool IsEstadosLoaded
        {
			get { return _isEstadosLoaded; }
			set { SetPropertyChanged(ref _isEstadosLoaded, value); }
        }

        public IList<Cidade> Cidades
        {
			get { return _cidades; }
			set { SetPropertyChanged(ref _cidades, value); }
        }

        public Cidade CidadeSelected
        {
			get { return _cidadeSelected; }
			set { SetPropertyChanged(ref _cidadeSelected, value); }
        }

        public bool IsCidadesLoaded
        {
			get { return _isCidadesLoaded; }
			set { SetPropertyChanged(ref _isCidadesLoaded, value); }
        }

        #endregion

        #region Comandos

        public ICommand UpdateCidadesCommand
			=> _updateCidadesCommand ?? (_updateCidadesCommand = new Command(async () => await LoadCidades()));

        public ICommand RegisterFazendaCommand
			=> _registerFazendaCommand ?? (_registerFazendaCommand = new Command(async () => await RegisterFazenda()));

        #endregion

        #region Funções

        private async Task LoadEstados()
        {
            Estados = await AzureService.Instance.ListEstadosAsync();
            IsEstadosLoaded = true;
        }


		public async Task LoadCidades()
        {
            Cidades = await AzureService.Instance.ListCidadesAsync(EstadoSelected.Id);
            IsCidadesLoaded = true;
        }

		public async Task RegisterFazenda()
        {
            if (!_isUpdate)
            {
                if (string.IsNullOrEmpty(FazendaName))
                {
                    MessagesResource.FazendaCadastroNomeVazio.ToDisplayAlert(MessageType.Info);
                    return;
                }

                if (CidadeSelected == null)
                {
                    MessagesResource.FazendaCadastroCidadeVazia.ToDisplayAlert(MessageType.Info);
                    return;
                }

                var userId = await DependencyService.Get<IAuthentication>().UserId();

                var fazenda = new Fazenda
                {
                    Id = Guid.NewGuid().ToString(),
                    Nome = FazendaName,
                    CidadeId = CidadeSelected.Id,
                    UsuarioId = userId
                };

                await AzureService.Instance.InsertFazendaAsync(fazenda);

                if (!_fromAnalise)
                    MessagesResource.FazendaCadastroSucesso.ToToast();
                else
                    MessagingCenter.Send(this, MessagesResource.McFazendaSelecionada, fazenda.Id);
            }
            else
            {
                if (string.IsNullOrEmpty(FazendaName))
                {
                    MessagesResource.FazendaCadastroNomeVazio.ToDisplayAlert(MessageType.Info);
                    return;
                }
                _fazenda.Nome = FazendaName;
                _fazenda.CidadeId = CidadeSelected.Id;
                await AzureService.Instance.UpdateFazendaAsync(_fazenda);
                MessagingCenter.Send(this, MessagesResource.McFazendaSelecionada, _fazenda.Id);
                MessagesResource.FazendaEdicaoSucesso.ToToast(ToastNotificationType.Sucesso);
            }
            await Navigation.PopAsync();
        }

        #endregion
    }
}