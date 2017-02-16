using System;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Models;
using Xamarin.Forms;
using static Solum.Messages.TalhaoMessages;
using static Solum.Messages.MessagingCenterMessages;

namespace Solum.ViewModel
{
    public class TalhaoCadastroViewModel : BaseViewModel
    {
        public TalhaoCadastroViewModel(INavigation navigation, string fazendaId, bool fromAnalise) : base(navigation)
        {
            PageTitle = "Novo Talhão";
            _realm = Realm.GetInstance();
            _isUpdate = false;
            _fromAnalise = fromAnalise;
            Fazenda = _realm.Find<Fazenda>(fazendaId);
        }

        public TalhaoCadastroViewModel(INavigation navigation, string talhaoId) : base(navigation)
        {
            PageTitle = "Editar Talhão";
            _realm = Realm.GetInstance();
            _isUpdate = true;
            Talhao = _realm.Find<Talhao>(talhaoId);
            Fazenda = _realm.Find<Fazenda>(Talhao.FazendaId);
            TalhaoName = Talhao.Nome;
            TalhaoArea = Talhao.Area;
        }

        #region Commands

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new Command(Save));

        #endregion

        #region Functions

        private async void Save()
        {
            if (!_isUpdate)
            {
                if (string.IsNullOrEmpty(TalhaoName))
                {
                    TalhaoNameNull.ToDisplayAlert(MessageType.Aviso);
                    return;
                }

                var novo = new Talhao
                {
                    Id = Guid.NewGuid().ToString(),
                    FazendaId = Fazenda.Id,
                    Fazenda = _realm.Find<Fazenda>(Fazenda.Id),
                    Area = TalhaoArea,
                    Nome = TalhaoName,
                    HasArea = !string.IsNullOrEmpty(TalhaoArea)
                };

                using (var transaction = _realm.BeginWrite())
                {
                    _realm.Add(novo);
                    transaction.Commit();
                }
                if (!_fromAnalise) Success.ToToast();
                else MessagingCenter.Send(this, TalhaoSelected, novo.Id);
                await Navigation.PopAsync();
            }

            else
            {
                if (string.IsNullOrEmpty(TalhaoName))
                {
                    TalhaoNameNull.ToDisplayAlert(MessageType.Aviso);
                    return;
                }

                using (var transaction = _realm.BeginWrite())
                {
                    Talhao.Nome = TalhaoName;
                    Talhao.Area = TalhaoArea;
                    Talhao.HasArea = !string.IsNullOrEmpty(Talhao.Area);
                    transaction.Commit();
                }

                if (!_fromAnalise) UpdateSuccess.ToToast();
                else MessagingCenter.Send(this, TalhaoSelected, Talhao.Id);
                await Navigation.PopAsync();
            }
        }

        #endregion

        #region Private Properties

        private ICommand _saveCommand;

        private Fazenda _fazenda;
        private Talhao _talhao;

        private string _talhaoName;
        private string _talhaoArea;

        private readonly bool _isUpdate;
        private readonly bool _fromAnalise;

        private readonly Realm _realm;

        #endregion

        #region Binding Properties

        public string TalhaoName
        {
            get { return _talhaoName; }
            set { SetPropertyChanged(ref _talhaoName, value); }
        }

        public string TalhaoArea
        {
            get { return _talhaoArea; }
            set { SetPropertyChanged(ref _talhaoArea, value); }
        }

        public Fazenda Fazenda
        {
            get { return _fazenda; }
            set { SetPropertyChanged(ref _fazenda, value); }
        }

        public Talhao Talhao
        {
            get { return _talhao; }
            set { SetPropertyChanged(ref _talhao, value); }
        }

        #endregion
    }
}