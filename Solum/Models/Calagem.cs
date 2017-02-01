using Realms;

namespace Solum.Models
{
    public class Calagem : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string AnaliseId { get; set; }
        public float V2 { get; set; }
        public float Prnt { get; set; }
        public int Profundidade { get; set; }

        public Analise Analise { get; set; }
    }
}