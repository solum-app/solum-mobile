using System;
using System.Threading.Tasks;
using MessageBar;
using Solum.iOS.Handelers;
using Solum.Interfaces;
using Xamarin.Forms;

[assembly: Dependency (typeof (ToastNotifier))]
namespace Solum.iOS.Handelers
{
	public class ToastNotifier : IToastNotifier
	{
		static MessageBarStyleSheet _styleSheet;

		public static void Init()
		{
			_styleSheet = new MessageBarStyleSheet();
		}

		public Task<bool> Notify(ToastNotificationType type, string title, string description, TimeSpan duration, object context = null)
		{
			MessageType msgType = MessageType.Info;

			switch (type)
			{
				case ToastNotificationType.Erro:

				case ToastNotificationType.Aviso:
					msgType = MessageType.Error;
					break;

				case ToastNotificationType.Sucesso:
					msgType = MessageType.Success;
					break;
			}

			var taskCompletionSource = new TaskCompletionSource<bool>();
			MessageBarManager.SharedInstance.ShowMessage(title, description, msgType, b => taskCompletionSource.TrySetResult(b));
			return taskCompletionSource.Task;
		}

		public void HideAll()
		{
			MessageBarManager.SharedInstance.HideAll();
		}
	}
}
