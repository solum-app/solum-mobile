using System;

namespace Solum.Models
{
    public class EntityData
    {
        public string Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string Version { get; set; }
    }
}