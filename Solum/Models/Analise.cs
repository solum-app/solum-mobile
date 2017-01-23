using System;
using Solum.Handler;
using Realms;


namespace Solum.Models
{
    public class Analise : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string TalhaoId { get; set; }
        public string Nome { get; set; }
        public DateTimeOffset Data { get; set; }

        public float PotencialHidrogenico { get; set; }
        public float Fosforo { get; set; } 
        public float Potassio { get; set; }
        public float Calcio { get; set; }
        public float Magnesio { get; set; }
        public float Aluminio { get; set; }
        public float Hidrogenio { get; set; }
        public float MateriaOrganica { get; set; }
        public float Areia { get; set; }
        public float Silte { get; set; }
        public float Argila { get; set; }

        public DateTimeOffset? DataCalculoCalagem { get; set; }
        public DateTimeOffset? DataCalculoCorretiva { get; set; }
        public DateTimeOffset? DataCalculoSemeadura { get; set; }
        public DateTimeOffset? DataCalculoCobertura { get; set; }

        public Talhao Talhao { get; set; }

        [Ignored]
        public float SB => CalculoHandler.CalcularSB(Potassio, Calcio, Magnesio);

        [Ignored]
        public float CTC => CalculoHandler.CalcularCTC(SB, Hidrogenio, Aluminio);

        [Ignored]
        public float V => CalculoHandler.CalcularV(SB, CTC);

        [Ignored]
        public float M => CalculoHandler.CalcularM(Aluminio, SB);

        [Ignored]
        public float CaMg => CalculoHandler.CalcularCaMg(Calcio, Magnesio);

        [Ignored]
        public float CaK => CalculoHandler.CalcularCaK(Calcio, Potassio);

        [Ignored]
        public float MgK => CalculoHandler.CalcularMgK(Magnesio, Potassio);
    }
}

