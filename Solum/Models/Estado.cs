using System.Collections.Generic;

namespace Solum.Models
{
    public class Estado : DataTable
    {
        public string Nome { get; set; }
        public string Uf { get; set; }

        public virtual ICollection<Cidade> Cidades { get; set; }
        public override string ToString()
        {
            return $"{Nome} - {Uf}";
        }
    }
}