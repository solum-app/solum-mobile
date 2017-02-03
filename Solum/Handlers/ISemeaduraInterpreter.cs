namespace Solum.Handlers
{
    public interface ISemeaduraInterpreter
    {
        float CalculateN(int expectativa, string level);
        float CalculateP(int expectativa, string level);
        float CalculateK(int expectativa, string level);
    }

    public static class SemeaduraInjector
    {
        public static ISemeaduraInterpreter GetInstance(string name)
        {
            if (name.ToUpper().Equals("MILHO"))
                return MilhoSemeaturaInterpreter.Instance;
            return null;
        }
    }
}