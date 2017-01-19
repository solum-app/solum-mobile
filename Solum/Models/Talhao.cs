using Realms;

namespace Solum.Models
{
    public class Talhao : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string FazendaId { get; set; }
        public string Nome { get; set; }
        public string Area { get; set; }
        public bool HasArea { get; set; }
        public Fazenda Fazenda { get; set; }
        public bool IsValido => !string.IsNullOrEmpty(Nome);

        public override string ToString()
        {
            return string.IsNullOrEmpty(Area)? Nome : $"{Nome} - {Area}m²";
        }
    }
}