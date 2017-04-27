using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solum.Auth;
using Solum.Interfaces;
using Solum.Models;
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

        public static async Task<IQueryable<Fazenda>> PerUser(this IQueryable<Fazenda> query)
        {
            var userId = await DependencyService.Get<ILoginReader>().UserId();
            if(!string.IsNullOrEmpty(userId))
                return query.Where(f => f.UsuarioId == userId);
            throw new NullReferenceException("User Id is null");
        }

        public static async Task<IQueryable<Analise>> PerUser(this IQueryable<Analise> query)
        {
            var userId = await DependencyService.Get<ILoginReader>().UserId();
            if (!string.IsNullOrEmpty(userId))
                return query.Where(f => f.UsuarioId == userId);
            throw new NullReferenceException("User Id is null");
        }

        public static async Task<IQueryable<Talhao>> PerUser(this IQueryable<Talhao> query)
        {
            var userId = await DependencyService.Get<ILoginReader>().UserId();
            if (!string.IsNullOrEmpty(userId))
                return query.Where(f => f.UsuarioId == userId);
            throw new NullReferenceException("User Id is null");
        }
    }
}