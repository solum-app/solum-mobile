using Realms;

namespace Solum.Models
{
    public class Cidade : EntityData
    {
        public string Nome { get; set; }
        public string EstadoId { get; set; }
        public override string ToString()
        {
            return Nome;
        }
    }
}