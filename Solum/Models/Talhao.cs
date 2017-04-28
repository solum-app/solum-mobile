namespace Solum.Models
{
    public class Talhao : EntityData
    {
        public string UsuarioId { get; set; }
        public string FazendaId { get; set; }
        public string Nome { get; set; }
        public string Area { get; set; }
        public bool HasArea { get; set; }

        public override string ToString()
        {
            return HasArea ? Nome : $"{Nome} - {Area} ha";
        }
    }
}