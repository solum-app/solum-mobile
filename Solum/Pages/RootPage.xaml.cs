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
            if (Device.OS == TargetPlatform.Android)
				DependencyService.Get<IStatusBarColor>().SetColor((Color)Application.Current.Resources["colorPrimaryDark"]);
            _currentPage = new AnalisesPage();
            _navigationPage = new NavigationPage(_currentPage)
            {
                BarBackgroundColor = (Color)Application.Current.Resources["colorPrimary"],
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

			var testeGesture = new TapGestureRecognizer();
            testeGesture.Tapped += OnTesteTapped;
            TesteLabel.GestureRecognizers.Add(testeGesture);
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
						BarBackgroundColor = (Color)Application.Current.Resources["colorPrimary"],
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
                    _currentPage = new FazendaListPage(false);
                    Detail = new NavigationPage(_currentPage)
					{
						BarBackgroundColor = (Color)Application.Current.Resources["colorPrimary"],
						BarTextColor = Color.White
					};
                    IsPresented = false;
                }
            }
            else
            {
                var page = new FazendaListPage(false);
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
						BarBackgroundColor = (Color)Application.Current.Resources["colorPrimary"],
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


		public async void OnTesteTapped(object sender, EventArgs e)
		{
			if (Device.OS == TargetPlatform.iOS)
			{
				if (_currentPage.GetType() == typeof(SemeaduraPage))
				{
					IsPresented = false;
				}
				else
				{
					_currentPage = new SemeaduraPage();
					Detail = new NavigationPage(_currentPage)
					{
						BarBackgroundColor = (Color)Application.Current.Resources["colorPrimary"],
						BarTextColor = Color.White
					};
					IsPresented = false;
				}
			}
			else
			{
				var page = new SemeaduraPage();
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
                var authservice = AuthService.Instance;
                await authservice.Logoff();

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