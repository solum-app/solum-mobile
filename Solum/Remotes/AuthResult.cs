namespace Solum.Remotes
{
    public enum RegisterResult
    {
        RegisterSuccefully,
        RegisterUnsuccessfully,
    }

    public enum AuthResult
    {
        Success,
        
        LoginSuccessFully,
        LoginUnsuccessfully,
        LogoffSuccessfully,
        LogoffUnsuccessfully,

        InvalidName,
        InvalidEmail,
        WeakPassword,
        ShortPassword,
    }
}