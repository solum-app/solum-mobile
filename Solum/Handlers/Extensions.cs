using System;
using Xamarin.Forms;

namespace Solum
{
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
	}
}
