using Solum.Interfaces;
using Solum.Models;

namespace Solum.Handlers
{
    public class MilhoSemeaturaInterpreter : ISemeaduraInterpreter
    {
        private static MilhoSemeaturaInterpreter _instance;

        private MilhoSemeaturaInterpreter()
        {
        }

        public static MilhoSemeaturaInterpreter Instance => _instance ?? (_instance = new MilhoSemeaturaInterpreter());

        public float QuanidadeNitrogenio(int expectativa, Nivel nivel)
        {
            return expectativa == 6 ? 20.0f : 30.0f;
        }

        public float QuantidadeFosforo(int expectativa, Nivel nivel)
        {
            if (nivel != Nivel.Adequado && nivel != Nivel.Alto)
                nivel = Nivel.Adequado;

            switch (expectativa)
            {
                case 6:
                    return nivel == Nivel.Adequado ? 60.0f : 30.0f;
                case 8:
                    return nivel == Nivel.Adequado ? 80.0f : 40.0f;
                case 10:
                    return nivel == Nivel.Adequado ? 100.0f : 50.0f;
                case 12:
                    return nivel == Nivel.Adequado ? 120.0f : 60.0f;
            }
            return 0.0f;
        }

        public float QuantidadePotassio(int expectativa, Nivel nivel)
        {
            if (nivel != Nivel.Adequado && nivel != Nivel.Alto)
                nivel = Nivel.Adequado;

            switch (expectativa)
            {
                case 6:
                    return nivel == Nivel.Adequado ? 60.0f : 30.0f;
                case 8:
                    return nivel == Nivel.Adequado ? 60.0f : 40.0f;
                case 10:
                    return nivel == Nivel.Adequado ? 60.0f : 50.0f;
                case 12:
                    return 60.0f;
            }
            return 0.0f;
        }
    }
}