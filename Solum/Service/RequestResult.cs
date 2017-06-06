using System.Net;
using Newtonsoft.Json;

namespace Solum.Models
{
	public class RequestResult
	{
		public string Message { set; get; }

        [JsonIgnoreAttribute]
		public HttpStatusCode StatusCode { set; get; }

        [JsonIgnoreAttribute]
        public bool IsSuccess
        {
            get => StatusCode == HttpStatusCode.OK;
        }
	}

	public class RequestResult<TData> : RequestResult
	{
        [JsonIgnoreAttribute]
		public TData Data { set; get; }
	}
}
