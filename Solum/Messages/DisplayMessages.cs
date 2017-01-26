namespace Solum.Messages
{
    public class RegisterMessages
    {
        public static string Success = "Seu cadastro foi realizado com sucesso!";
        public static string Unsucces = "Seu cadastro não foi realizado";
        public static string NullEntriesMessage = "Preencha todos os campos";
        public static string InvalidEmail = "Seu E-Mail é inválido";
        public static string PasswordToShort = "Senha muito curta, deve conter ao menos 6 caracteres";
        public static string PasswordIsntMatch = "As senhas não são iguais";
        public static string CidadeIsntSelected = "Selecione o estado e a cidade que reside";
    }

    public class LoginMessages
    {
        public static string InvalidCredentialsMessage = "Suas credenciais estão inválidas";
        public static string NullEntriesMessage = "Preencha os campos com E-Mail e Senha";
        public static string LoginSuccessMessage = "Login com sucesso!";
        public static string ServerUnrecheable = "Servidor indisponível no momento";

    }

    public class FazendaMessages
    {
        public static string FazendaNameNull = "Preencha o campo de nome da fazenda";
        public static string CidadeIsntSelected = "Selecione o estado e cidade do endereço da fazenda";
        public static string Success = "Fazenda cadastrada";
        public static string Updated = "Fazenda atualizada";
        public static string Deleted = "Fazenda removida";
    }

    public class TalhaoMessages
    {
        public static string TalhaoNameNull = "Preencha o campo de nome do talhão";
        public static string Success = "Talhão cadastrado com sucesso!";
        public static string UpdateSuccess = "Talhão atualizado com sucesso!";
        public static string Deleted = "Talhao removido com sucesso!";
    }

    public class UpdateAnalisesMessage
    {
        public static string UpdateAnalises = "UpdateAnalises";
    }

    public class AnaliseMessages
    {
        public static string TalhaoIsntSelected = "Selecione um talhao. Para selecionar um talhão, selecione uma fazenda";
        public static string InvalidDate = "Selecione uma data válida";
        public static string PhNull = "Preencha o campo pH";
        public static string PNull = "Preencha o campo P";
        public static string KNull = "Preencha o campo K";
        public static string CaNull = "Preencha o campo Ca";
        public static string MgNull = "Preencha o campo Mg";
        public static string AlNull = "Preencha o campo Al";
        public static string HNull = "Preencha o campo H";
        public static string MoNull = "Preencha o campo Materia Orgânica";
        public static string AreaiNull = "Preencha o campo Areia";
        public static string SilteNull = "Preencha o campo Silte";
        public static string ArgilaNull = "Preencha o campo Argila";
        public static string Success = "Dados da análise salvos";

    }
}