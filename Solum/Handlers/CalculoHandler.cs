using System;

namespace Solum.Handler
{
	static class CalculoHandler
	{
		/// <summary>
		/// Esse método calcula o valor de SB
		/// </summary>
		/// <param name="KEntry">Valor de entrada de Potássio</param>
		/// <param name="CaEntry">Valor de entrada de Sódio</param>
		/// <param name="MgEntry">Valor de entrada de Magnésio</param>
		/// <returns>Retorna o valor de SB</returns>
		public static float CalcularSB(float KEntry, float CaEntry, float MgEntry)
		{
			var result = KEntry/390 + CaEntry + MgEntry;
			return result;
		}

		/// <summary>
		/// Esse método calcula o valor de CTC
		/// </summary>
		/// <param name="SBResult">Valor calculado com base nos valores de K, Ca e Mg</param>
		/// <param name="HEntry">Valor de entrada de Hidrogênio</param>
		/// <param name="AlEntry">Valor de entrada de Alumínio</param>
		/// <returns>Retorna o valor de CTC</returns>
		public static float CalcularCTC(float SBResult, float HEntry, float AlEntry)
		{
			var result = SBResult + HEntry + AlEntry;
			return result;
		}

		/// <summary>
		/// Calcula o valor de V em %
		/// </summary>
		/// <param name="SBResult">Valor calculado com base nos valores de K, Ca e Mg</param>
		/// <param name="CTCResult">Valor calculado com base nos valores de SB + H + Al</param>
		/// <returns>Retorna o valor de V em %</returns>
		public static float CalcularV(float SBResult, float CTCResult)
		{
			var result = SBResult / CTCResult * 100;
			return result;
		}

		/// <summary>
		/// Calcula o valor de m em %
		/// </summary>
		/// <param name="AlEntry">Valor de entrada de Alumínio</param>
		/// <param name="SBResult">Valor calculado com base nos valores de K, Ca e Mg</param>
		/// <returns>Retorna o valor de m em %</returns>
		public static float CalcularM(float AlEntry, float SBResult)
		{
			var result = AlEntry / (AlEntry + SBResult) * 100;
			return result;
		}

		/// <summary>
		/// Calcula o valor de Ca/Mg
		/// </summary>
		/// <param name="CaEntry">Valor de entrada de Sódio</param>
		/// <param name="MgEntry">Valor de entrada de Magnésio</param>
		/// <returns>Retorna o valor de Ca/Mg</returns>
		public static float CalcularCaMg(float CaEntry, float MgEntry)
		{
			var result = CaEntry / MgEntry;
			return result;
		}

		/// <summary>
		/// Calcula o valor de Ca/K
		/// </summary>
		/// <param name="CaEntry">Valor de entrada de Sódio</param>
		/// <param name="KEntry">Valor de entrada de Potássio</param>
		/// <returns>Retorna o valor de Ca/K</returns>
		public static float CalcularCaK(float CaEntry, float KEntry)
		{
			var result = CaEntry / (KEntry / 390);
			return result;
		}

		/// <summary>
		/// Calcula o valor de Mg/K
		/// </summary>
		/// <param name="MgEntry">Valor de entrada de Magnésio</param>
		/// <param name="KEntry">Valor de entrada de Potássio</param>
		/// <returns></returns>
		public static float CalcularMgK(float MgEntry, float KEntry)
		{
			var result = MgEntry / (KEntry / 390);
			return result;
		}
	}

}


