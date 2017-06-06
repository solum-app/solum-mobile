using System;
using System.Threading.Tasks;
using Solum.Handlers;
using Solum.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserDialogs))]
namespace Solum.Handlers
{
    public class UserDialogs : IUserDialogs
    {
        private readonly IToastNotifier _toaster;

        public UserDialogs() {
            _toaster = DependencyService.Get<IToastNotifier>();
        }

        public void ShowToast(string message, ToastNotificationType type, string title, int duration)
        {
            _toaster.Notify(type, title, message, TimeSpan.FromMilliseconds(duration));
        }

        public async Task DisplayAlert(string message, string title)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }

        public async Task<bool> DisplayAlert(string message, string accept, string cancel, string title)
		{
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
		}
    }
}
