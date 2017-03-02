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
    }
}