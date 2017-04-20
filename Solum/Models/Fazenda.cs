namespace Solum.Models
{
    public class Fazenda : EntityData
    {
        public string Nome { get; set; }
        public string CidadeId { get; set; }
        public string UsuarioId { get; set; }

        public override string ToString()
        {
            return Nome;
        }
    }
}