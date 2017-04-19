﻿using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        private string _nome;
        private string _email;

        public MenuViewModel(INavigation navigation) : base(navigation)
        {
           // var user = Realm.GetInstance().All<Usuario>().FirstOrDefault();
            Nome = "Teste";
            Email = "Teste@gmail.com";
        }

        public string Nome
        {
            get { return _nome; }
            set { SetPropertyChanged(ref _nome, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetPropertyChanged(ref _email, value); }
        }
    }
}