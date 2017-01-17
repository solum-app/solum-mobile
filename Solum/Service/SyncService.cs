using System.Linq;
using System.Threading.Tasks;
using Realms;
using Solum.Models;
using Solum.Remotes;

namespace Solum.Service
{
    public class SyncService
    {
        public static bool InSync { get; set; } = false;
        public static void CidadeEstadoSync()
        {
            InSync = true;
            var dataService = Realm.GetInstance();
            if (!dataService.All<Estado>().Any())
            {
                var estadoRemote = new EstadoRemote();
                var estados = estadoRemote.GetEstados();
                using (var trans = dataService.BeginWrite())
                {
                    foreach (var e in estados)
                        dataService.Add(e, true);
                    trans.Commit();
                }
                foreach (var e in estados)
                {
                    var cidades = estadoRemote.GetCidades(e.Id);
                    using (var tsc = dataService.BeginWrite())
                    {
                        foreach (var c in cidades)
                        {
                            c.Estado = e;
                            dataService.Add(c, true);
                        }
                        tsc.Commit();
                    }
                }
            }
            InSync = false;
        }
    }
}