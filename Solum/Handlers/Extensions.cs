using System;
using System.Text;
using Solum.Interfaces;
using Xamarin.Forms;

namespace Solum.Handlers
{
    public enum MessageType
    {
        Info,
        Erro,
        Aviso,
        Sucesso,
        Falha
    }

    public static class Extensions
    {
        public static void ToToast(this string message, ToastNotificationType type = ToastNotificationType.Info, string title = null)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var toaster = DependencyService.Get<IToastNotifier>();
                toaster.Notify(type, title ?? type.ToString().ToLowerInvariant(), Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetBytes(message).Length), TimeSpan.FromSeconds(2.5f));
            });
        }

        public static void ToDisplayAlert(this string message, MessageType messageType = MessageType.Info)
        {
            Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert(messageType.ToString().ToLowerInvariant(),Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetBytes(message).Length), "OK");
                });
        }
    }
}