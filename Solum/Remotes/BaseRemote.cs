using System.Net.Http;

namespace Solum.Remotes
{
    public class BaseRemote
    {
        public HttpClient Client { get; set; }

        public BaseRemote()
        {
            Client = new HttpClient
            {
                BaseAddress = Settings.BaseUri,
                MaxResponseContentBufferSize = 10240000
            };
        }
    }
}