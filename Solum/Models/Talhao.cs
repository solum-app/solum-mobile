namespace Solum.Models
{
    public class Talhao : EntityData
    {
        public string FazendaId { get; set; }
        public string Nome { get; set; }
        public string Area { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Area) ? Nome : $"{Nome} - {Area} ha";
        }
    }
}