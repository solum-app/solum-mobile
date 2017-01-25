using System;
using System.Globalization;
using Solum.Interfaces;
using Xamarin.Forms;

namespace Solum.Handlers
{
    public enum MessageType
    {
        Info, Erro, Aviso, Sucesso, Falha
    }

	public static class Extensions
	{
		public static void ToToast(this string message, ToastNotificationType type = ToastNotificationType.Info, string title = null)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				var toaster = DependencyService.Get<IToastNotifier>();
				toaster.Notify(type, title ?? type.ToString(), message, TimeSpan.FromSeconds(2.5f));
			});
		}

	    public static void ToDisplayAlert(this string message, MessageType messageType = MessageType.Sucesso)
	    {
	        Device.BeginInvokeOnMainThread(() =>
	        {
	            Application.Current.MainPage.DisplayAlert(messageType.ToString().ToUpper(), message, "OK");
	        });
	    }
    }
}
