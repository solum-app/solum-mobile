using System;
using System.Threading.Tasks;
using Android.Widget;
using Solum.Droid;
using Xamarin.Forms;

[assembly: Dependency (typeof (ToastNotifier))]
namespace Solum.Droid
{
	public class ToastNotifier : IToastNotifier
	{
		public Task<bool> Notify(ToastNotificationType type, string title, string description, TimeSpan duration, object context = null)
		{
			var taskCompletionSource = new TaskCompletionSource<bool>();
			Toast.MakeText(Forms.Context, description, ToastLength.Short).Show();
			return taskCompletionSource.Task;
		}

		public void HideAll()
		{
		}
	}
}
