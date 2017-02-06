using Realms;

namespace Solum.Models
{
    public class Semeadura : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string AnaliseId { get; set; }

        public string Cultura { get; set; }

        public int Expectativa { get; set; }

        public Analise Analise { get; set; }
    }
}