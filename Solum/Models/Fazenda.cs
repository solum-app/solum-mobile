using Realms;

namespace Solum.Models
{
    public class Fazenda : RealmObject
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string CidadeId { get; set; }

        public Cidade Cidade { get; set; }

        public Usuario Usuario { get; set; }

        public override string ToString()
        {
            return $"{Nome}";
        }
    }
}