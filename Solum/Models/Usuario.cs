using System;
using Realms;

namespace Solum.Models
{
    public class Usuario : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset TokenCreated { get; set; }
        public DateTimeOffset TokenValidate { get; set; }
    }
}