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
        public static void CidadeEstadoSync()
        {
            //var realm = Realm.GetInstance();
            //var estadosLocal = realm.All<Estado>().AsQueryable();
            //var cidadesLocal = realm.All<Cidade>().AsQueryable();
            //var remote = new EstadoRemote();
            //if (!estadosLocal.Any() || estadosLocal.Count() < 27)
            //{
                
            //    try
            //    {
            //        var estados = remote.GetEstados();
            //        using (var transaction = realm.BeginWrite())
            //        {
            //            foreach (var estado in estados)
            //                realm.Add(estado, true);
            //            transaction.Commit();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.WriteLine(ex.Message);
            //    }
            //}
            //if (!cidadesLocal.Any() || cidadesLocal.Count() < 5565)
            //{
            //    foreach (var estado in estadosLocal)
            //    {
            //        var cidades = remote.GetCidades(estado.Id);
            //        using (var transaction = realm.BeginWrite())
            //        {
            //            foreach (var cidade in cidades)
            //            {
            //                cidade.Estado = estado;
            //                realm.Add(cidade, true);
            //            }
            //            transaction.Commit();
            //        }
            //    }
            //}
        }
    }
}