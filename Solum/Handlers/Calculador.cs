using Solum.Models;

namespace Solum.Handlers
{
    internal static class Calculador
    {
        /// <summary>
        ///   Esse método calcula o valor de SB
        /// </summary>
        /// <param name="potassio">Valor de entrada de Potássio</param>
        /// <param name="calcio">Valor de entrada de Sódio</param>
        /// <param name="magnesio">Valor de entrada de Magnésio</param>
        /// <returns>Retorna o valor de SB</returns>
        public static float CalcularSb(float potassio, float calcio, float magnesio)
        {
            var result = potassio / 390 + calcio + magnesio;
            return result;
        }

        /// <summary>
        ///   Esse método calcula o valor de CTC
        /// </summary>
        /// <param name="sb">Valor calculado com base nos valores de K, Ca e Mg</param>
        /// <param name="hidrogenio">Valor de entrada de Hidrogênio</param>
        /// <param name="aluminio">Valor de entrada de Alumínio</param>
        /// <returns>Retorna o valor de CTC</returns>
        public static float CalcularCtc(float sb, float hidrogenio, float aluminio)
        {
            var result = sb + hidrogenio + aluminio;
            return result;
        }

        /// <summary>
        ///   Calcula o valor de V em %
        /// </summary>
        /// <param name="sb">Valor calculado com base nos valores de K, Ca e Mg</param>
        /// <param name="ctc">Valor calculado com base nos valores de SB + H + Al</param>
        /// <returns>Retorna o valor de V em %</returns>
        public static float CalcularV(float sb, float ctc)
        {
            var result = sb / ctc * 100;
            return result;
        }

        /// <summary>
        ///   Calcula o valor de m em %
        /// </summary>
        /// <param name="aluminio">Valor de entrada de Alumínio</param>
        /// <param name="sb">Valor calculado com base nos valores de K, Ca e Mg</param>
        /// <returns>Retorna o valor de m em %</returns>
        public static float CalcularM(float aluminio, float sb)
        {
            var result = aluminio / (aluminio + sb) * 100;
            return result;
        }

        /// <summary>
        ///   Calcula o valor de Ca/Mg
        /// </summary>
        /// <param name="calcio">Valor de entrada de Sódio</param>
        /// <param name="magnesio">Valor de entrada de Magnésio</param>
        /// <returns>Retorna o valor de Ca/Mg</returns>
        public static float CalcularCaMg(float calcio, float magnesio)
        {
            var result = calcio / magnesio;
            return result;
        }

        /// <summary>
        ///   Calcula o valor de Ca/K
        /// </summary>
        /// <param name="calcio">Valor de entrada de Sódio</param>
        /// <param name="potassio">Valor de entrada de Potássio</param>
        /// <returns>Retorna o valor de Ca/K</returns>
        public static float CalcularCaK(float calcio, float potassio)
        {
            var result = calcio / (potassio / 390);
            return result;
        }

        /// <summary>
        ///   Calcula o valor de Mg/K
        /// </summary>
        /// <param name="magnesio">Valor de entrada de Magnésio</param>
        /// <param name="potassio">Valor de entrada de Potássio</param>
        /// <returns></returns>
        public static float CalcularMgK(float magnesio, float potassio)
        {
            var result = magnesio / (potassio / 390);
            return result;
        }

        /// <summary>
        ///   Calcula a quantidade de calcario
        /// </summary>
        /// <returns></returns>
        public static float CalcularCalcario(float prnt, float v2, float ctc, float v, int profundiade)
        {
            var f = 100f / prnt;
            var x = v2 - v;
            var s = x * ctc;
            var k = 100f * f;
            var j = s / k;
            j *= (profundiade / 20);
            return j;
        }

		/// <summary>
		///   Calcula o P2O5
		/// </summary>
		/// <returns></returns>
		public static float CalcularP2O5(float argila, float silte, float fosforo)
		{
            var textura = Interpretador.Textura(argila, silte);
            var pInterpretaded = Interpretador.NiveFosforo(fosforo, textura);
                                                             
            switch (pInterpretaded)
            {
                case Nivel.MuitoBaixo:
                    return (argila / 10 * 4);
                case Nivel.Baixo:
                    return (argila / 10 * 2);
                case Nivel.Medio:
                    return (argila / 10 * 1);
                default:
                    return 0;
            }
		}

        /// <summary>
        ///   Calcula o K2O
        /// </summary>
        /// <returns></returns>
        public static float CalcularK2O(float argila, float silte, float potassio, float ctc)
        {
            var textura = Interpretador.Textura(argila, silte);
            var kInterpretaded = Interpretador.NivelPotassio(potassio, ctc);

            if (ctc < 4)
                switch (kInterpretaded)
                {
                    case Nivel.Baixo:
                        return 50.0f;
                    case Nivel.Medio:
                        return 25.0f;
                    default:
                        return 0;
                }
            else
                switch (kInterpretaded)
                {
                    case Nivel.Baixo:
                        return 100.0f;
                    case Nivel.Medio:
                        return 50.0f;
					default:
						return 0;
                }
		}
    }
}