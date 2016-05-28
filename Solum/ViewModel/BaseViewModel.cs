using System;
using Xamarin.Forms;

namespace Solum.ViewModel
{
	public class BaseViewModel : BaseNotify
	{
		public BaseViewModel(INavigation navigation)
		{
			this.Navigation = navigation;
		}
		public INavigation Navigation
		{
			get;
			set;
		}
		#region Properties
		bool _isBusy = false;
		public bool IsBusy
		{
			get
			{
				return _isBusy;
			}
			set
			{
				SetPropertyChanged(ref _isBusy, value);
				SetPropertyChanged("IsNotBusy");
			}
		}
		public bool IsNotBusy
		{
			get
			{
				return !IsBusy;
			}
		}
		#endregion
		public virtual void NotifyPropertiesChanged()
		{
		}
	}
	#region Helper Classes
	/// <summary>
	/// Helper class that enforces the flag will always get set to false
	/// </summary>
	public class Busy : IDisposable
	{
		readonly object _sync = new object();
		readonly BaseViewModel _viewModel;
		public Busy(BaseViewModel viewModel)
		{
			_viewModel = viewModel;
			lock (_sync)
			{
				_viewModel.IsBusy = true;
			}
		}
		public void Dispose()
		{
			lock (_sync)
			{
				_viewModel.IsBusy = false;
			}
		}
	}
	#endregion
}