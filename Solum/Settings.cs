using System;

namespace Solum
{
    public static class Settings
    {
        public static string BackButtonTitle = "Voltar";
        public static string ButtonTitle = "Ok";
        public static string ErrorMessageTitle = "Ops! :-/";
        public static string SuccessMessageTitle = "Tudo Certo! ;-)";

        static Settings()
        {
            // BaseUri = new Uri("http://192.168.0.23/solum/api/");
            BaseUri = new Uri("http://192.168.0.7/solum/api/");
            AccountUri = "account/";
            AccountRegisterUri = "account/register";
            AccountLoginUri = "account/login";
            AccountLogoutUri = "account/logout";
            TokenUri = "token/";
            EstadoUri = "estado/";
            CidadeUri = "cidade/";
        }

        public static Uri BaseUri { get; set; }

        #region Models Uri

        public static string EstadoUri { get; set; }
        public static string CidadeUri { get; set; }

        #endregion

        #region Account Uris

        public static string AccountUri { get; set; }
        public static string AccountRegisterUri { get; set; }
        public static string AccountLoginUri { get; set; }
        public static string AccountLogoutUri { get; set; }
        public static string TokenUri { get; set; }

        #endregion
    }
}