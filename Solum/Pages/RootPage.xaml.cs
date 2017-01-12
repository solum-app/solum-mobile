using System;
using Solum.Effects;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class RootPage : MasterDetailPage
    {
        private NavigationPage _navigationPage;
        private Page _currentPage;


        public RootPage()
        {
            InitializeComponent();
            var color = Color.FromHex("#1FA549");
            if (Device.OS == TargetPlatform.Android)
                DependencyService.Get<IStatusBarColor>().SetColor(color);
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

            var calagemGesture = new TapGestureRecognizer();
            calagemGesture.Tapped += OnCalagemTapped;
            CalagemLabel.GestureRecognizers.Add(calagemGesture);

            var recomendacaoGesture = new TapGestureRecognizer();
            recomendacaoGesture.Tapped += OnRecomendacaoTapped;
            RecomendacaoLabel.GestureRecognizers.Add(recomendacaoGesture);
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

        public async void OnCalagemTapped(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                if (_currentPage.GetType() == typeof(CalagemPage))
                {
                    IsPresented = false;
                }
                else
                {
                    _currentPage = new CalagemPage();
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
                var page = new CalagemPage();
                await _navigationPage.Navigation.PushAsync(page);
                _navigationPage.Navigation.RemovePage(_currentPage);
                _currentPage = page;
                IsPresented = false;
            }
        }

        public async void OnRecomendacaoTapped(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                if (_currentPage.GetType() == typeof(RecomendaCalagemPage))
                {
                    IsPresented = false;
                }
                else
                {
                    _currentPage = new RecomendaCalagemPage();
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
                var page = new RecomendaCalagemPage();
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
                var color = Color.Black;
                if (Device.OS == TargetPlatform.Android)
                    DependencyService.Get<IStatusBarColor>().SetColor(color);
                _currentPage = new LoginPage();
                _navigationPage = new NavigationPage(_currentPage)
                {
                    BarBackgroundColor = Color.FromHex("#24BE55"),
                    BarTextColor = Color.White
                };
                Application.Current.MainPage = _navigationPage;
            }
        }
    }
}