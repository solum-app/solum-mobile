using System;
using Realms;
using Solum.Handlers;

namespace Solum.Models
{
    public class Analise : RealmObject
    {
        // testesgaysgaysag
        public Analise()
        {
            Id = Guid.NewGuid().ToString();
        }

        #region Identificação

        [PrimaryKey]
        public string Id { get; set; }

        public string TalhaoId { get; set; }
        public string Identificacao { get; set; }
        public DateTimeOffset DataRegistro { get; set; }
        public DateTimeOffset DataInterpretacao { get; set; }
        public bool WasInterpreted { get; set; }
        public DateTimeOffset DataCalculoCalagem { get; set; }
        public bool HasCalagem { get; set; }
        public DateTimeOffset DataCalculoCorretiva { get; set; }
        public bool HasCorretiva { get; set; }
        public DateTimeOffset DataCalculoSemeadura { get; set; }
        public bool HasSemeadura { get; set; }
        public DateTimeOffset DataCalculoCobertura { get; set; }
        public bool HasCobertura { get; set; }

        public Talhao Talhao { get; set; }

        #endregion

        #region Analise Quimica

        public float PotencialHidrogenico { get; set; }
        public float Fosforo { get; set; }
        public float Potassio { get; set; }
        public float Calcio { get; set; }
        public float Magnesio { get; set; }
        public float Aluminio { get; set; }
        public float Hidrogenio { get; set; }
        public float MateriaOrganica { get; set; }

        #endregion

        #region Analise Fisica

        public float Areia { get; set; }
        public float Silte { get; set; }
        public float Argila { get; set; }

        #endregion

        #region Calagem

        public float V2 { get; set; }
        public float Prnt { get; set; }
        public int Profundidade { get; set; }

        #endregion

        #region Semeadura

        public string Cultura { get; set; }
        public int Expectativa { get; set; }

        #endregion

        #region Calculated Properites

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

        #endregion
    }
}