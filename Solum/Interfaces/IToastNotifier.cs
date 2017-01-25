using System;
using System.Threading.Tasks;

namespace Solum.Interfaces {
	
	public interface IToastNotifier
	{
		Task<bool> Notify(ToastNotificationType type, string title, string description, TimeSpan duration, object context = null);

		void HideAll();
	}

	public enum ToastNotificationType
	{
		Info,
		Sucesso,
		Erro,
		Aviso,
	}
}