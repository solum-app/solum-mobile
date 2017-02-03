using Solum.Handlers;
using Xamarin.Forms;

namespace Solum.Handlers
{
    public class MilhoSemeaturaInterpreter : ISemeaduraInterpreter
    {
        private static MilhoSemeaturaInterpreter _instance;
        public static MilhoSemeaturaInterpreter Instance => _instance ?? (_instance = new MilhoSemeaturaInterpreter());
        private MilhoSemeaturaInterpreter()
        {
            
        }
        public float CalculateN(int expectativa, string level)
        {
            if (expectativa == 6)
                return 20.0f;
            return 30.0f;
        }

        public float CalculateP(int expectativa, string level)
        {
            var upper = level.ToUpper();
            if (!upper.Equals("ADEQUADO") || !upper.Equals("ALTO"))
                upper = "ADEQUADO";
            
            if (expectativa == 6) return upper.Equals("ADEQUADO") ? 60.0f : 30.0f;
            if (expectativa == 8) return upper.Equals("ADEQUADO") ? 80.0f : 40.0f;
            if (expectativa == 10) return upper.Equals("ADEQUADO") ? 100.0f : 50.0f;
            if (expectativa == 12) return upper.Equals("ADEQUADO") ? 120.0f : 60.0f;
            return 0.0f;
        }

        public float CalculateK(int expectativa, string level)
        {
            var upper = level.ToUpper();
            if (!upper.Equals("ADEQUADO") || !upper.Equals("ALTO"))
                upper = "ADEQUADO";
            if (expectativa == 6) return upper.Equals("ADEQUADO") ? 60.0f : 30.0f;
            if (expectativa == 8) return upper.Equals("ADEQUADO") ? 60.0f : 40.0f;
            if (expectativa == 10) return upper.Equals("ADEQUADO") ? 60.0f : 50.0f;
            if (expectativa == 12) return 60.0f;
            return 0.0f;
        }
    }
}