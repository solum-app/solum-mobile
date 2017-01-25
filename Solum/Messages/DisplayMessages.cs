namespace Solum.Messages
{
    public class RegisterMessages
    {
        public static string Success = "Seu cadastro foi realizado com sucesso!";
        public static string Unsucces = "Seu cadastro n�o foi realizado";
        public static string NullEntriesMessage = "Preencha todos os campos";
        public static string InvalidEmail = "Seu E-Mail � inv�lido";
        public static string PasswordToShort = "Senha muito curta, deve conter ao menos 6 caracteres";
        public static string PasswordIsntMatch = "As senhas n�o s�o iguais";
        public static string CidadeIsntSelected = "Selecione o estado e a cidade que reside";

    }

    public class LoginMessages
    {
        public static string InvalidCredentialsMessage = "Suas credenciais est�o inv�lidas";
        public static string NullEntriesMessage = "Preencha os campos com E-Mail e Senha";
        public static string LoginSuccessMessage = "Login com sucesso!";

    }

    public class FazendaMessages
    {
        public static string NullEntriesTitle = "NullEntries";
        public static string NullEntriesMessage = "Preencha o campo nome e selecione a cidade que a fazenda resite";

        public static string RegisterSuccessfullTitle = "RegisterSuccesfull";
        public static string RegisterSuccessfullMessage = "Fazenda cadastrada com sucesso!";

        public static string UpdateSuccessfullTitle = "UpdateSuccessfull";
        public static string UpdateSucessfullMessage = "Fazenda atualizada com sucesso!";
    }

    public class TalhaoMessages
    {
        public static string NullEntriesTitle = "NullEntries";
        public static string NullEntriesMessage = "Preencha o campo nome do talhao";

        public static string RegisterSuccessfullTitle = "RegisterSuccesfull";
        public static string RegisterSuccessfullMessage = "Talh�o cadastrado com sucesso!";

        public static string UpdateSuccessfullTitle = "UpdateSuccessfull";
        public static string UpdateSucessfullMessage = "Talh�o Atualizado com sucesso!";
    }

    public class UpdateAnalisesMessage
    {
        public static string UpdateAnalises = "UpdateAnalises";
    }
}