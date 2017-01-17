using System.Linq;
using Realms;

namespace Solum.Models
{
    public class Estado : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Uf { get; set; }

        [Backlink(nameof(Cidade.Estado))]
        public IQueryable<Cidade> Cidades { get; }

        public override string ToString()
        {
            return $"{Nome} - {Uf}";
        }
    }
}