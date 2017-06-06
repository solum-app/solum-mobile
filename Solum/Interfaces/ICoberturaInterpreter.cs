using Solum.Models;

namespace Solum.Interfaces
{
    public interface ICoberturaInterpreter
    {
        float QuanidadeNitrogenio(int expectativa);
        float QuantidadeFosforo(int expectativa);
        float QuantidadePotassio(int expectativa);
    }

    public class MilhoCoberturaInterpreter : ICoberturaInterpreter
    {
        private static MilhoCoberturaInterpreter _instance;
        public static MilhoCoberturaInterpreter Instance => _instance ?? (_instance = new MilhoCoberturaInterpreter());

        private MilhoCoberturaInterpreter()
        {
            
        }

        public float QuanidadeNitrogenio(int expectativa)
        {
            switch (expectativa)
            {
                case 6:
                    return 40.0f;
                case 8:
                    return 70.0f;
                case 10:
                    return 130.0f;
                case 12:
                    return 180.0f;
            }
            return 0;
        }

        public float QuantidadeFosforo(int expectativa)
        {
            return 0;
        }

        public float QuantidadePotassio(int expectativa)
        {
            switch (expectativa)
            {
                case 6:
                    return 0.0f;
                case 8:
                    return 30.0f;
                case 10:
                    return 60.0f;
                case 12:
                    return 90.0f;
            }
            return 0;
        }
    }

    public static class CoberturaInjector
    {
        public static ICoberturaInterpreter GetInstance(Cultura cultura)
        {
            return cultura == Cultura.Milho ? MilhoCoberturaInterpreter.Instance : null;
        }
    }
}