using System;
using Solum.Interfaces;
using Xamarin.Forms;

namespace Solum.Handlers
{
    public enum MessageType
    {
        Info, Error, Warning, Success, Fail
    }

	public static class Extensions
	{
		public static void ToToast(this string message, ToastNotificationType type = ToastNotificationType.Info, string title = null)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				var toaster = DependencyService.Get<IToastNotifier>();
				toaster.Notify(type, title ?? type.ToString().ToUpper(), message, TimeSpan.FromSeconds(2.5f));
			});
		}

	    public static void ToDisplayAlert(this string message, string title = null, MessageType messageType = MessageType.Success)
	    {
	        Device.BeginInvokeOnMainThread(() =>
	        {
	            Application.Current.MainPage.DisplayAlert(title ?? messageType.ToString().ToUpper(), message, "OK");
	        });
	    }
	}
}
