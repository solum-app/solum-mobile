using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Solum.Helpers;
using Solum.Models;

namespace Solum.Remotes
{
    public class EstadoRemote : BaseRemote
    {
        public ICollection<Estado> GetEstados()
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception("Sem conexão com Internet");

            var url = Settings.BaseUri + Settings.EstadoUri + "?all=true";
            var response = Client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode) throw new Exception("Houve um erro na obteção dos dados do servidor");
            var content = response.Content.ReadAsStringAsync().Result;
            var collection = JsonConvert.DeserializeObject<ICollection<Estado>>(content);
            return collection;
        }

        public ICollection<Cidade> GetCidades(string estadoId)
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception("Sem conexão com Internet");

            var url = Settings.BaseUri + Settings.CidadeUri + "?estadoid=" + estadoId + "&all=true";
            var response = Client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode) throw new Exception("Houve um erro na obteção dos dados do servidor");
            var content = response.Content.ReadAsStringAsync().Result;
            var collection = JsonConvert.DeserializeObject<ICollection<Cidade>>(content);
            return collection;
        }
    }
}