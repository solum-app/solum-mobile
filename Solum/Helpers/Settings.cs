// Helpers/Settings.cs

using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Solum.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        private const string BaseUriKey = "BaseUri";
        private static readonly string BaseUriValue = "https://solumtest.azurewebsites.net";

        private const string UserIdKey = "UserId";
        private static readonly string UserIdValue = string.Empty;

        private const string TokenKey = "UserToken";
        private static readonly string TokenValue = string.Empty;

        private const string ProviderKey = "CustomProvider";
        private static readonly string ProviderValue = "Identity";

        private const string EstadosLoadedKey = "EstadosLoaded";
        private static readonly bool EstadosLoadedValue = false;

        private const string CidadesLoadedKey = "CidadesLoaded";
        private static readonly bool CidadesLoadedValue = false;

		private const string DBPathKey = "DbPath_key";
		private static readonly string DBPathValue;

        #endregion
        

        public static string BaseUri
        {
            get { return AppSettings.GetValueOrDefault(BaseUriKey, BaseUriValue); }
            set { AppSettings.AddOrUpdateValue(BaseUriKey, value); }
        }

        public static string UserId
        {
            get { return AppSettings.GetValueOrDefault(UserIdKey, UserIdValue); }
            set { AppSettings.AddOrUpdateValue(UserIdKey, value); }
        }

        public static string Token
        {
            get { return AppSettings.GetValueOrDefault(TokenKey, TokenValue); }
            set { AppSettings.AddOrUpdateValue(TokenKey, value); }
        }

        public static string AuthProvider
        {
            get { return AppSettings.GetValueOrDefault(ProviderKey, ProviderValue); }
            set { AppSettings.AddOrUpdateValue(ProviderKey, value); }
        }

        public static bool EstadosLoaded
        {
            get { return AppSettings.GetValueOrDefault(EstadosLoadedKey, EstadosLoadedValue); }
            set { AppSettings.AddOrUpdateValue(EstadosLoadedKey, value); }
        }

        public static bool CidadesLoaded
        {
            get { return AppSettings.GetValueOrDefault(CidadesLoadedKey, CidadesLoadedValue); }
            set { AppSettings.AddOrUpdateValue(CidadesLoadedKey, value); }
        }

		public static string DBPath
		{
			get { return AppSettings.GetValueOrDefault(DBPathKey, DBPathValue); }
			set { AppSettings.AddOrUpdateValue(DBPathKey, value); }
		}

        public static string CidadeUri { get; set; }
        public static string EstadoUri { get; set; }
        public static object AccountRegisterUri { get; set; }
        public static object AccountLoginUri { get; set; }
        public static object TokenUri { get; set; }
        public static object AccountLogoutUri { get; set; }
    }
}