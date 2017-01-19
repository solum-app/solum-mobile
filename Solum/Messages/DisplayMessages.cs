namespace Solum.Messages
{
    public class RegisterMessages
    {
        public static string RegisterSucessfullTitle = "RegisterSucessfull";
        public static string RegisterSuccessfullMessage = "Seu cadastro foi realizado com sucesso!";

        public static string RegisterUnsuccessTitle = "RegisterUnsuccess";
        public static string RegisterUnsuccessMessage = "Seu cadastro não foi realizado com sucesso!";

        public static string EntryNullValuesTitle = "EntryNullValues";
        public static string EntryNullValuesMessage = "Preencha os campos, selecionando a cidade que reside";

        public static string InvalidEmailTitle = "InvalidEmail";
        public static string InvalidEmailMessage = "Seu e-mail não está na forma correta";

        public static string PasswordIsntMatchTitle = "PasswordIsntMatch";
        public static string PasswordIsntMatchMessage = "Verifique as senhas, elas não estão iguais";

        public static string WeakPasswordTitle = "WeakPassword";
        public static string WeakPasswordMessage = "Sua senha é fraca.Deve conter pelo menos: \n" +
                                                  "- 6 caracteres\n" +
                                                  "- 1 maísculo\n" +
                                                  "- 1 minúsculo\n" +
                                                  "- 1 número\n" +
                                                  "- 1 caracter especial";

        public static string CityIsntSelectedTitle = "CityIsntSelected";
        public static string CityIsntSelectedMessage = "Selecione o Estado e a Cidade que reside";
    }

    public class LoginMessages
    {
        public static string EntryNullValuesTitle = "EntryNullValues";
        public static string EntryNullValuesMessage = "Preencha os campos com email e senha";

        public static string LoginErrorTitle = "CredentialsError";

        public static string LoginErrorMessage =
            "Suas credenciais estão erradas ou sua conta não foi ativada. Por favor, verifique seu e-mail";
    }
}