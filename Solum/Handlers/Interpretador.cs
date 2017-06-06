using Solum.Models;

namespace Solum.Handlers
{
    internal static class Interpretador
    {
        public static Textura Textura(float argila, float silte)
        {
            if (argila < 150)
                return Models.Textura.Arenosa;

            if (argila + silte > 150 && argila < 350)
                return Models.Textura.Media;

            return argila < 600 ? Models.Textura.Argilosa : Models.Textura.MuitoArgilosa;
        }

        public static Nivel NivelPotencialHidrogenico(float ph)
        {
            if (ph <= 4.4)
                return Nivel.Alto;

            if (ph > 4.4 && ph <= 4.8)
                return Nivel.Medio;

            if (ph > 4.8 && ph <= 5.5)
                return Nivel.Adequado;

            if (ph > 5.5 && ph <= 5.8)
                return Nivel.Baixo;

            return Nivel.MuitoBaixo;
        }

        public static Nivel NiveFosforo(float fosforo, Textura textura)
        {
            switch (textura)
            {
                case Models.Textura.Arenosa:

                    if (fosforo <= 6)
                        return Nivel.MuitoBaixo;

                    if (fosforo > 6 && fosforo <= 12)
                        return Nivel.Baixo;

                    if (fosforo > 12 && fosforo <= 18)
                        return Nivel.Medio;

                    if (fosforo > 18 && fosforo <= 25)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                case Models.Textura.Media:

                    if (fosforo <= 5)
                        return Nivel.MuitoBaixo;

                    if (fosforo > 5 && fosforo <= 10)
                        return Nivel.Baixo;

                    if (fosforo > 10 && fosforo <= 15)
                        return Nivel.Medio;

                    if (fosforo > 15 && fosforo <= 20)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                case Models.Textura.Argilosa:

                    if (fosforo <= 3)
                        return Nivel.MuitoBaixo;

                    if (fosforo > 3 && fosforo <= 5)
                        return Nivel.Baixo;

                    if (fosforo > 5 && fosforo <= 8)
                        return Nivel.Medio;

                    if (fosforo > 8 && fosforo <= 12)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                case Models.Textura.MuitoArgilosa:

                    if (fosforo <= 2)
                        return Nivel.MuitoBaixo;

                    if (fosforo > 2 && fosforo <= 3)
                        return Nivel.Baixo;

                    if (fosforo > 3 && fosforo <= 4)
                        return Nivel.Medio;

                    if (fosforo > 4 && fosforo <= 6)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                default:
                    return Nivel.Nenhum;
            }
        }

        public static Nivel NivelPotassio(float potassio, float ctc)
        {
            if (ctc < 4)
            {
                if (potassio <= 15)
                    return Nivel.Baixo;

                if (potassio > 15 && potassio <= 30)
                    return Nivel.Medio;

                if (potassio > 30 && potassio <= 40)
                    return Nivel.Adequado;

                return Nivel.Alto;
            }

            if (potassio <= 25)
                return Nivel.Baixo;

            if (potassio > 25 && potassio <= 50)
                return Nivel.Medio;

            if (potassio > 50 && potassio <= 80)
                return Nivel.Adequado;

            return Nivel.Alto;
        }

        public static Nivel NivelCalcio(float calcio)
        {
            if (calcio < 1.5)
                return Nivel.Baixo;

            if (calcio >= 1.5 && calcio <= 7)
                return Nivel.Adequado;

            return Nivel.Alto;
        }

        public static Nivel NivelMagnesio(float magnesio)
        {
            if (magnesio < 0.5)
                return Nivel.Baixo;

            if (magnesio >= 0.5 && magnesio <= 2)
                return Nivel.Adequado;

            return Nivel.Alto;
        }

        public static Nivel NivelCalcioPotassio(float calcioPotassio)
        {
            if (calcioPotassio <= 7)
                return Nivel.Baixo;

            if (calcioPotassio > 7 && calcioPotassio <= 14)
                return Nivel.Medio;

            if (calcioPotassio > 14 && calcioPotassio <= 25)
                return Nivel.Adequado;

            return Nivel.Alto;
        }

        public static Nivel NivelMagnesioPotassio(float magnesioPotassio)
        {
            if (magnesioPotassio <= 2)
                return Nivel.Baixo;

            if (magnesioPotassio > 2 && magnesioPotassio <= 4)
                return Nivel.Medio;

            if (magnesioPotassio > 4 && magnesioPotassio <= 15)
                return Nivel.Adequado;

            return Nivel.Alto;
        }

        public static Nivel NivelM(float m)
        {
            if (m < 20)
                return Nivel.Baixo;

            if (m >= 20 && m <= 60)
                return Nivel.Alto;

            return Nivel.MuitoAlto;
        }

        public static Nivel NivelV(float v)
        {
            if (v <= 20)
                return Nivel.Baixo;

            if (v > 20 && v <= 35)
                return Nivel.Medio;

            if (v > 35 && v <= 60)
                return Nivel.Adequado;

            if (v > 60 && v <= 70)
                return Nivel.Alto;

            return Nivel.MuitoAlto;
        }

        public static Nivel NivelCtc(float ctc, Textura textura)
        {
            switch (textura)
            {
                case Models.Textura.Arenosa:

                    if (ctc < 3.2)
                        return Nivel.Baixo;

                    if (ctc >= 3.2 && ctc <= 4)
                        return Nivel.Medio;

                    if (ctc > 4 && ctc <= 6)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                case Models.Textura.Media:

                    if (ctc < 4.8)
                        return Nivel.Baixo;

                    if (ctc >= 4.8 && ctc <= 6)
                        return Nivel.Medio;

                    if (ctc > 6 && ctc <= 9)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                case Models.Textura.Argilosa:

                    if (ctc < 7.2)
                        return Nivel.Baixo;

                    if (ctc >= 7.2 && ctc <= 9)
                        return Nivel.Medio;

                    if (ctc > 9 && ctc <= 13.5)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                case Models.Textura.MuitoArgilosa:

                    if (ctc < 9.6)
                        return Nivel.Baixo;

                    if (ctc >= 9.6 && ctc <= 12)
                        return Nivel.Medio;

                    if (ctc > 12 && ctc <= 18)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                default:
                    return Nivel.Nenhum;
            }
        }

        public static Nivel NivelMateriaOrganica(float materiaOrganica, Textura textura)
        {
            switch (textura)
            {
                case Models.Textura.Arenosa:

                    if (materiaOrganica < 8)
                        return Nivel.Baixo;

                    if (materiaOrganica >= 8 && materiaOrganica <= 10)
                        return Nivel.Medio;

                    if (materiaOrganica > 10 && materiaOrganica <= 15)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                case Models.Textura.Media:

                    if (materiaOrganica < 16)
                        return Nivel.Baixo;

                    if (materiaOrganica >= 16 && materiaOrganica <= 20)
                        return Nivel.Medio;

                    if (materiaOrganica > 20 && materiaOrganica <= 30)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                case Models.Textura.Argilosa:
                    if (materiaOrganica < 24)
                        return Nivel.Baixo;

                    if (materiaOrganica >= 24 && materiaOrganica <= 30)
                        return Nivel.Medio;

                    if (materiaOrganica > 30 && materiaOrganica <= 45)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                case Models.Textura.MuitoArgilosa:
                    if (materiaOrganica < 28)
                        return Nivel.Baixo;

                    if (materiaOrganica >= 8 && materiaOrganica <= 35)
                        return Nivel.Medio;

                    if (materiaOrganica > 35 && materiaOrganica <= 52)
                        return Nivel.Adequado;

                    return Nivel.Alto;

                default:
                    return Nivel.Nenhum;
            }
        }

		public static string NivelPhConverter(Nivel nivel)
		{
			switch (nivel)
			{
				case Nivel.MuitoBaixo:
					return "Acidez Muito Baixa";
				case Nivel.Baixo:
					return "Acidez Baixa";
				case Nivel.Medio:
					return "Acidez Média";
				case Nivel.Adequado:
					return "Acidez Adequeada";
				case Nivel.Alto:
					return "Acidez Alta";
				default:
					return "";
			}
		}

		public static string NivelConverter(Nivel nivel)
		{
			switch (nivel)
			{
				case Nivel.MuitoBaixo:
					return "Muito Baixo";
				case Nivel.Baixo:
					return "Baixo";
				case Nivel.Adequado:
					return "Adequado";
				case Nivel.Medio:
					return "Médio";
				case Nivel.Alto:
					return "Alto";
				case Nivel.MuitoAlto:
					return "Muito Alto";
				case Nivel.Nenhum:
					return "";
				default:
					return "";
			}
		}

		public static string TexturaConverter(Textura textura)
		{
			switch (textura)
			{
				case Models.Textura.Arenosa:
					return "Arenosa";
				case Models.Textura.Media:
					return "Média";
				case Models.Textura.Argilosa:
					return "Argilosa";
				case Models.Textura.MuitoArgilosa:
					return "Muito Argilosa";
				default:
					return "";
			}
		}
    }
}