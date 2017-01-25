namespace Solum.Messages
{
    public class RegisterMessages
    {
        
    }

    public class LoginMessages
    {
        public static string EntryNullValuesTitle = "EntryNullValues";
        public static string EntryNullValuesMessage = "Preencha os campos com email e senha";

        public static string LoginErrorTitle = "CredentialsError";

        public static string LoginErrorMessage =
            "Suas credenciais estão erradas ou sua conta não foi ativada. Por favor, verifique seu e-mail";
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
        public static string RegisterSuccessfullMessage = "Talhão cadastrado com sucesso!";

        public static string UpdateSuccessfullTitle = "UpdateSuccessfull";
        public static string UpdateSucessfullMessage = "Talhão Atualizado com sucesso!";
    }
}