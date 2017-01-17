using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Realms;
using Solum.Models;
using Solum.Remotes;

namespace Solum.Service
{
    public class SyncService
    {
        public static bool InSync { get; set; }
        public static async Task CidadeEstadoSync()
        {
            InSync = true;
            var realm = Realm.GetInstance();
            var estadosLocal = realm.All<Estado>().AsQueryable();
            if (!estadosLocal.Any() || estadosLocal.Count() < 27)
            {
                var remote = new EstadoRemote();

                try
                {
                    var estados = await remote.GetEstados();
                    using (var transaction = realm.BeginWrite())
                    {
                        foreach (var estado in estados)
                            realm.Add(estado, true);
                        transaction.Commit();
                    }

                    foreach (var estado in estados)
                    {
                        var cidades = await remote.GetCidades(estado.Id);
                        using (var transaction = realm.BeginWrite())
                        {
                            foreach (var cidade in cidades)
                            {
                                cidade.Estado = estado;
                                realm.Add(cidade, true);
                            }
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            InSync = false;
        }
    }
}