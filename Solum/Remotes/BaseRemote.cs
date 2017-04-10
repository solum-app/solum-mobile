using System;
using System.Net.Http;
using Solum.Helpers;

namespace Solum.Remotes
{
    public class BaseRemote
    {
        public HttpClient Client { get; }

        public BaseRemote()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(Settings.BaseUri),
                MaxResponseContentBufferSize = 10240000,
                Timeout = TimeSpan.FromSeconds(20)
            };
        }
    }
}