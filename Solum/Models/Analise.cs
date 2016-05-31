using System;
using Solum.Handler;


namespace Solum.Models
{
	public class Analise
	{
		public string Fazenda {
			get;
			set;
		}

		public string Talhao {
			get;
			set;
		}

		public DateTime Data {
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

		public float SB { 
			get
			{
				return CalculoHandler.CalcularSB(K, Ca, Mg);
			}
		}

		public float CTC { 
			get
			{
				return CalculoHandler.CalcularCTC(SB, H, Al);
			}
		}

		public float V { 
			get
			{
				return CalculoHandler.CalcularV(SB, CTC);
			}
		}

		public float M { 
			get
			{
				return CalculoHandler.CalcularM(Al, SB);
			}
		}

		public float CaMg {
			get
			{
				return CalculoHandler.CalcularCaMg(Ca, Mg);
			}
		}

		public float CaK {
			get
			{
				return CalculoHandler.CalcularCaK(Ca, K);
			} 
		}

		public float MgK {
			get
			{
				return CalculoHandler.CalcularMgK(Mg, K);
			} 
		}
	}
}

