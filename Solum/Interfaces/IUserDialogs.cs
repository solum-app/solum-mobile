using System;
using System.Threading.Tasks;

namespace Solum.Interfaces
{
    public interface IUserDialogs
    {
        void ShowToast(string message, ToastNotificationType type = ToastNotificationType.Info, string title = null, int duration = 2000);
        Task DisplayAlert(string message, string title = null);
        Task<bool> DisplayAlert(string message, string accept, string cancel, string title = null);
    }
}
