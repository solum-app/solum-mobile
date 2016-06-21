using System;
using Solum.Handler;
using Realms;


namespace Solum.Models
{
	public class Analise : RealmObject
	{

		[ObjectId]
		public int Id {
			get;
			set;
		}

		public string Fazenda {
			get;
			set;
		}

		public string Talhao {
			get;
			set;
		}

		public DateTimeOffset Data {
			get;
			set;
		}

		public float Ph {
			get;
			set;
		}

		public float P {
			get; 
			set; 
		}

		public float K { 
			get; 
			set; 
		}

		public float Ca { 
			get; 
			set; 
		}

		public float Mg { 
			get; 
			set; 
		}

		public float Al { 
			get; 
			set; 
		}

		public float H { 
			get; set; 
		}

		public float MateriaOrganica { 
			get; 
			set; 
		}

		public float Areia { 
			get; 
			set; 
		}

		public float Silte { 
			get; 
			set; 
		}

		public float Argila { 
			get; 
			set; 
		}

		[Ignored]
		public float SB { 
			get
			{
				return CalculoHandler.CalcularSB(K, Ca, Mg);
			}
		}

		[Ignored]
		public float CTC { 
			get
			{
				return CalculoHandler.CalcularCTC(SB, H, Al);
			}
		}

		[Ignored]
		public float V { 
			get
			{
				return CalculoHandler.CalcularV(SB, CTC);
			}
		}

		[Ignored]
		public float M { 
			get
			{
				return CalculoHandler.CalcularM(Al, SB);
			}
		}

		[Ignored]
		public float CaMg {
			get
			{
				return CalculoHandler.CalcularCaMg(Ca, Mg);
			}
		}

		[Ignored]
		public float CaK {
			get
			{
				return CalculoHandler.CalcularCaK(Ca, K);
			} 
		}

		[Ignored]
		public float MgK {
			get
			{
				return CalculoHandler.CalcularMgK(Mg, K);
			} 
		}
	}
}

