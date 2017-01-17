using Realms;

namespace Solum.Models
{
    public class Cidade : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Nome { get; set; }
        public string EstadoId { get; set; }
        public Estado Estado { get; set; }
        public override string ToString()
        {
            return Nome;
        }
    }
}