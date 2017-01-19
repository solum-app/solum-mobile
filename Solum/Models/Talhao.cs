using Realms;

namespace Solum.Models
{
    public class Talhao : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string FazendaId { get; set; }
        public string Nome { get; set; }
        public double? Area { get; set; }
        public Fazenda Fazenda { get; set; }

        public bool IsValido => !string.IsNullOrEmpty(Nome);
        public override string ToString()
        {
            return Area.HasValue ? Nome : $"{Nome} - {Area}m²";
        }
    }
}