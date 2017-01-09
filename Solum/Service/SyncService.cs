using System.Threading.Tasks;
using Realms;
using Solum.Remotes;

namespace Solum.Service
{
    public class SyncService
    {
        public static async Task CidadeEstadoSync()
        {
            var dataService = Realm.GetInstance();
            var estadoRemote = new EstadoRemote();
            var estados = await estadoRemote.GetEstados();
            using (var trans = dataService.BeginWrite())
            {
                foreach (var e in estados)
                    dataService.Add(e, true);
                trans.Commit();
            }
        }
    }
}