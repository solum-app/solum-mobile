using System;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class RootPage : MasterDetailPage
    {
        private readonly NavigationPage _navigationPage;
        private Page _currentPage;
        public string User { get; }
        public string Username { get; }

        public RootPage()
        {
            InitializeComponent();
            _currentPage = new AnalisesPage();
            _navigationPage = new NavigationPage(_currentPage)
            {
                BarBackgroundColor = Color.FromHex("#24BE55"),
                BarTextColor = Color.White
            };
            Detail = _navigationPage;

            var analiseGesture = new TapGestureRecognizer();
            analiseGesture.Tapped += OnAnalisesTapped;
            AnalisesLabel.GestureRecognizers.Add(analiseGesture);

            var fazendaGesture = new TapGestureRecognizer();
            fazendaGesture.Tapped += OnFazendasTapped;
            FazendasLabel.GestureRecognizers.Add(fazendaGesture);

            var sobreGesture = new TapGestureRecognizer();
            sobreGesture.Tapped += OnSobreTapped;
            SobreLabel.GestureRecognizers.Add(sobreGesture);

            var sairGesture = new TapGestureRecognizer();
            sairGesture.Tapped += OnSairTapped;
            SairLabel.GestureRecognizers.Add(sairGesture);

            var user = new UserDataService().GetLoggedUser();
            User = user.Nome;
            Username = user.Username;
        }

        public async void OnAnalisesTapped(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                if (_currentPage.GetType() == typeof(AnalisesPage))
                {
                    IsPresented = false;
                }
                else
                {
                    _currentPage = new AnalisesPage();
                    Detail = new NavigationPage(_currentPage)
                    {
                        BarBackgroundColor = Color.FromHex("#24BE55"),
                        BarTextColor = Color.White
                    };
                    IsPresented = false;
                }
            }
            else
            {
                var page = new AnalisesPage();
                await _navigationPage.Navigation.PushAsync(page);
                _navigationPage.Navigation.RemovePage(_currentPage);
                _currentPage = page;
                IsPresented = false;
            }
        }

        public async void OnFazendasTapped(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                if (_currentPage.GetType() == typeof(FazendasPage))
                {
                    IsPresented = false;
                }
                else
                {
                    _currentPage = new FazendasPage();
                    Detail = new NavigationPage(_currentPage)
                    {
                        BarBackgroundColor = Color.FromHex("#24BE55"),
                        BarTextColor = Color.White
                    };
                    IsPresented = false;
                }
            }
            else
            {
                var page = new FazendasPage();
                await _navigationPage.Navigation.PushAsync(page);
                _navigationPage.Navigation.RemovePage(_currentPage);
                _currentPage = page;
                IsPresented = false;
            }
        }

        public async void OnSobreTapped(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                if (_currentPage.GetType() == typeof(SobrePage))
                {
                    IsPresented = false;
                }
                else
                {
                    _currentPage = new SobrePage();
                    Detail = new NavigationPage(_currentPage)
                    {
                        BarBackgroundColor = Color.FromHex("#24BE55"),
                        BarTextColor = Color.White
                    };
                    IsPresented = false;
                }
            }
            else
            {
                var page = new SobrePage();
                await _navigationPage.Navigation.PushAsync(page);
                _navigationPage.Navigation.RemovePage(_currentPage);
                _currentPage = page;
                IsPresented = false;
            }
        }

        public async void OnSairTapped(object sender, EventArgs e)
        {
            var command = await DisplayAlert("Sair", "Você realmente deseja sair do app?", "Sim", "Não");
            if (command)
            {
                Application.Current.MainPage = new LoginPage();
            }
        }
    }
}