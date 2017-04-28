using System;
using System.Collections.Generic;
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
				Titulo = "Titulo",
				Descricao = "Descrição"
			});
			itens.Add(new CarouselViewModel {
				Titulo = "Titulo",
				Descricao = "Descrição"
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
	}
}
