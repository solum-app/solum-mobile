using System;
using System.Windows.Input;
using Realms;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class TalhaoCadastroViewModel : BaseViewModel
    {
        private readonly Realm _realm;
        private ICommand _cadastrarTalhaoCommand;
        private Fazenda _fazenda;
        private Talhao _talhao;
        private string _talhaoNome;
        private string _talhaoArea;
        private string _titulo;
        private readonly bool _isUpdate;

        public TalhaoCadastroViewModel(INavigation navigation, Fazenda fazenda) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _isUpdate = false;
            Titulo = "Novo Talhão";
            Fazenda = fazenda;
        }

        public TalhaoCadastroViewModel(INavigation navigation, Fazenda fazenda, Talhao talhao) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _isUpdate = true;
            Fazenda = fazenda;
            Talhao = talhao;
            Titulo = "Atualizar Talhão " + _talhao.Nome;
            TalhaoNome = Talhao.Nome;
            TalhaoArea = Talhao.Area;
        }

        public string TalhaoNome
        {
            get { return _talhaoNome; }
            set { SetPropertyChanged(ref _talhaoNome, value); }
        }

        public string TalhaoArea
        {
            get { return _talhaoArea; }
            set { SetPropertyChanged(ref _talhaoArea, value); }
        }

        public string Titulo
        {
            get { return _titulo; }
            set { SetPropertyChanged(ref _titulo, value); }
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
       

        public ICommand CadastrarTalhaoCommand
            => _cadastrarTalhaoCommand ?? (_cadastrarTalhaoCommand = new Command(Cadastrar));

        private async void Cadastrar()
        {
            if (!_isUpdate)
            {
                if (string.IsNullOrEmpty(TalhaoNome))
                {
                    MessagingCenter.Send(this, "NullEntrys", "Preencha o campo de nome do talhão");
                    return;
                }

                var novo = new Talhao
                {
                    Id = Guid.NewGuid().ToString(),
                    FazendaId = Fazenda.Id,
                    Fazenda = _realm.Find<Fazenda>(Fazenda.Id),
                    Area = TalhaoArea,
                    Nome = TalhaoNome,
                    HasArea = !string.IsNullOrEmpty(TalhaoArea)
                };

                using (var transaction = _realm.BeginWrite())
                {
                    _realm.Add(novo);
                    transaction.Commit();
                }

                MessagingCenter.Send(this, "Success", "Talhão cadastrado com sucesso");
                await Navigation.PopAsync();
            }

            else
            {
                if (string.IsNullOrEmpty(TalhaoNome))
                {
                    MessagingCenter.Send(this, "NullEntrys", "Preencha o campo de nome do talhão");
                    return;
                }

                using (var transaction = _realm.BeginWrite())
                {
                    Talhao.Nome = TalhaoNome;
                    Talhao.Area = TalhaoArea;
                    transaction.Commit();
                }

                MessagingCenter.Send(this, "Success", "Talhão atualizado com sucesso");
                await Navigation.PopAsync();
            }
        }
    }
}