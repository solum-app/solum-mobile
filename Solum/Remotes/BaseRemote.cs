using System;
using System.Net.Http;

namespace Solum.Remotes
{
    public class BaseRemote
    {
        public HttpClient Client { get; }

        public BaseRemote()
        {
            Client = new HttpClient
            {
                BaseAddress = Settings.BaseUri,
                MaxResponseContentBufferSize = 10240000,
                Timeout = TimeSpan.FromSeconds(10)
            };
        }
    }
}