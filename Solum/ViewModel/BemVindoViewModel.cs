using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
	public class BemVindoViewModel : BaseViewModel
	{
		private IList<CarouselViewModel> _carouselItens;
		private int _position;

		public BemVindoViewModel(INavigation navigation) : base(navigation)
		{
			var itens = new List<CarouselViewModel>();
			itens.Add(new CarouselViewModel {
				Titulo = "Bem vindo ao Solum!",
				Descricao = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris quis rhoncus enim. Donec aliquet orci at enim dignissim blandit. Maecenas.",
				Imagem = "ic_carousel_app"
			});
			itens.Add(new CarouselViewModel {
				Titulo = "Sincronização das informações",
				Descricao = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris quis rhoncus enim. Donec aliquet orci at enim dignissim blandit. Maecenas.",
				Imagem = "ic_carousel_cloud"
			});
			itens.Add(new CarouselViewModel
			{
				Titulo = "Crie sua conta gratuitamente",
				Descricao = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris quis rhoncus enim. Donec aliquet orci at enim dignissim blandit. Maecenas.",
				Imagem = "ic_carousel_farmer",
				HasCommands = true,
				Navigation = navigation
			});
			CarouselItens = itens;
		}

		public IList<CarouselViewModel> CarouselItens
		{
			get { return _carouselItens; }
			set { SetPropertyChanged(ref _carouselItens, value); }
		}

		public int Position
		{
			get { return _position; }
			set { SetPropertyChanged(ref _position, value); }
		}
	}


	public class CarouselViewModel
	{
		private ICommand _navigateToLoginCommand;
		private ICommand _navigateToRegisterCommand;

		public string Titulo
		{
			get;
			set;
		}

		public string Descricao
		{
			get;
			set;
		}

		public string Imagem
		{
			get;
			set;
		}

		public bool HasCommands
		{
			get;
			set;
		}

		public INavigation Navigation
		{
			get;
			set;
		}

		public ICommand NavigateToLoginCommand
			=> _navigateToLoginCommand ?? (_navigateToLoginCommand = new Command(async () => await NavigateToLogin()));

		public ICommand NavigateToRegisterCommand
			=> _navigateToRegisterCommand ?? (_navigateToRegisterCommand = new Command(async () => await NavigateToRegister()));

		private async Task NavigateToLogin()
		{
			await Navigation.PushAsync(new LoginPage());
			while (Navigation.NavigationStack.Count > 0)
					Navigation.RemovePage(Navigation.NavigationStack[0]);
		}

		private async Task NavigateToRegister() {
			var cadastroPage = new CadastroPage();
			await Navigation.PushAsync(cadastroPage);
			Navigation.InsertPageBefore(new LoginPage(), cadastroPage);
			while (Navigation.NavigationStack.Count > 1)
					Navigation.RemovePage(Navigation.NavigationStack[0]);
		}
	}
}
