using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Solum.Models;
using Solum.Service.Interfaces;

namespace Solum.Service
{
    public class FazendaService : AzureService<Fazenda>, IStore<Fazenda>
    {
        private static FazendaService _instance;

        public static FazendaService Instance => _instance ?? (_instance = new FazendaService());

        private FazendaService() { }

        public async Task InsertAsync(Fazenda t)
        {
            await Table.InsertAsync(t);
        }

        public async Task UpdateAsync(Fazenda t)
        {
            await Table.UpdateAsync(t);
        }

        public async Task DeleteAsync(Fazenda t)
        {
            await Table.DeleteAsync(t);
        }

        public async Task<ICollection<Fazenda>> ReadAllAsync()
        {
            return (await Table.ToListAsync()).OrderBy(f => f.Nome).ToList();
        }

        public async Task<Fazenda> FindAsync(string id)
        {
            return await Table.LookupAsync(id);
        }
    }
}