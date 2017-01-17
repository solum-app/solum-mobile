using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace Solum.Models
{
    public class Estado : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Nome { get; set; }
        public string Uf { get; set; }
        public IList<Cidade> Cidades { get; }

        public override string ToString()
        {
            return $"{Nome} - {Uf}";
        }
    }
}
