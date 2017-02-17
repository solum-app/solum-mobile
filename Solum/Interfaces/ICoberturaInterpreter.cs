namespace Solum.Interfaces
{
    public interface ICoberturaInterpreter
    {
        string CalculateN(int expectativa);
        string CalculateP(int expectativa);
        string CalculateK(int expectativa);
    }

    public class MilhoCoberturaInterpreter : ICoberturaInterpreter
    {
        private static MilhoCoberturaInterpreter _instance;
        public static MilhoCoberturaInterpreter Instance => _instance ?? (_instance = new MilhoCoberturaInterpreter());

        private MilhoCoberturaInterpreter()
        {
            
        }

        public string CalculateN(int expectativa)
        {
            switch (expectativa)
            {
                case 6:
                    return 40.0f.ToString();
                case 8:
                    return 70.0f.ToString();
                case 10:
                    return 130.0f.ToString();
                case 12:
                    return 180.0f.ToString();
            }
            return 0.ToString();
        }

        public string CalculateP(int expectativa)
        {
            return 0.ToString();
        }

        public string CalculateK(int expectativa)
        {
            switch (expectativa)
            {
                case 6:
                    return 0.0f.ToString();
                case 8:
                    return 30.0f.ToString();
                case 10:
                    return 60.0f.ToString();
                case 12:
                    return 90.0f.ToString();
            }
            return 0.ToString();
        }
    }

    public static class CoberturaInjector
    {
        public static ICoberturaInterpreter GetInstance(string cultura)
        {
            if (cultura.Equals("MILHO"))
                return MilhoCoberturaInterpreter.Instance;
            return null;
        }
    }
}