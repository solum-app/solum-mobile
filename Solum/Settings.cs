using System;

namespace Solum
{
    public static class Settings
    {
        static Settings()
        {
            //BaseUri = new Uri("http://192.168.0.19/solum/api/");
            BaseUri = new Uri("http://192.168.0.12/solum/api/");
            AccountUri = "account/";
            AccountRegisterUri = "account/register";
            AccountLoginUri = "account/login";
            AccountLogoutUri = "account/logout";
            TokenUri = "token/";
            EstadoUri = "estado/";
        }

        public static Uri BaseUri { get; set; }

        #region Models Uri

        public static string EstadoUri { get; set; }

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