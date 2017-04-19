using Solum.Handlers;
using Solum.Models;

namespace Solum.Interfaces
{
    public interface ISemeaduraInterpreter
    {
        float QuanidadeNitrogenio(int expectativa, Nivel nivel);
        float QuantidadeFosforo(int expectativa, Nivel nivel);
        float QuantidadePotassio(int expectativa, Nivel nivel);
    }

    public static class SemeaduraInjector
    {
        public static ISemeaduraInterpreter GetInstance(Cultura cultura)
        {
            switch (cultura)
            {
                case Cultura.Milho:
                    return MilhoSemeaturaInterpreter.Instance;
                default:
                    return null;
            }
        }
    }
}