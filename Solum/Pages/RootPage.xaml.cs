using System;
using Solum.Effects;
using Solum.Service;
using Solum.ViewModel;
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
            var color = Color.FromHex("#2FBC5A");
            if (Device.OS == TargetPlatform.Android)
                DependencyService.Get<IStatusBarColor>().SetColor(Color.FromHex("#1FA549"));
            _currentPage = new AnalisesPage();
            _navigationPage = new NavigationPage(_currentPage)
            {
                BarBackgroundColor = color,
                BarTextColor = Color.White
            };
            Detail = _navigationPage;

            Master.BindingContext = new MenuViewModel(Navigation);

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

            //var calagemGesture = new TapGestureRecognizer();
            //calagemGesture.Tapped += OnCalagemTapped;
            //CalagemLabel.GestureRecognizers.Add(calagemGesture);

            //var recomendacaoGesture = new TapGestureRecognizer();
            //recomendacaoGesture.Tapped += OnRecomendacaoTapped;
            //RecomendacaoLabel.GestureRecognizers.Add(recomendacaoGesture);
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
                        BarBackgroundColor = Color.FromHex("#2FBC5A"),
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
                if (_currentPage.GetType() == typeof(FazendaListPage))
                {
                    IsPresented = false;
                }
                else
                {
                    _currentPage = new FazendaListPage();
                    Detail = new NavigationPage(_currentPage)
                    {
                        BarBackgroundColor = Color.FromHex("#2FBC5A"),
                        BarTextColor = Color.White
                    };
                    IsPresented = false;
                }
            }
            else
            {
                var page = new FazendaListPage();
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
                        BarBackgroundColor = Color.FromHex("#2FBC5A"),
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
                        BarBackgroundColor = Color.FromHex("#2FBC5A"),
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
                        BarBackgroundColor = Color.FromHex("#2FBC5A"),
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
                var authservice = new AuthService();
                await authservice.Logoff();

                var color = Color.Black;
                if (Device.OS == TargetPlatform.Android)
                    DependencyService.Get<IStatusBarColor>().SetColor(color);
                _currentPage = new LoginPage();
                _navigationPage = new NavigationPage(_currentPage)
                {
                    BarBackgroundColor = Color.Transparent,
                    BarTextColor = Color.Black
                };
                Application.Current.MainPage = _navigationPage;
            }
        }
    }
}