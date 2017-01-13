using System;
using System.Linq;
using Realms;

namespace Solum.Models
{
    public class Usuario : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset TokenCreated { get; set; }
        public DateTimeOffset TokenValidate { get; set; }
        public string CidadeId { get; set; }

        public Cidade Cidade { get; set; }

        public IQueryable<Fazenda> Fazendas { get; set; }
    }
}