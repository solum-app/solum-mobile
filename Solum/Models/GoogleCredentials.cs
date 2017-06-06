using System;
using Newtonsoft.Json;

namespace Solum.Models
{
    public class GoogleCredentials
    {
        public string IdToken
        {
            get;
            set;
        }

        public string AuthorizationCode
		{
			get;
			set;
		}

        public string Nome
        {
            get;
            set;
        }

		public string Email
		{
			get;
			set;
		}
    }
}
