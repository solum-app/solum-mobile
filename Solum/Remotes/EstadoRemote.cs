using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Solum.Models;

namespace Solum.Remotes
{
    public class EstadoRemote : BaseRemote
    {
        public async Task<ICollection<Estado>> GetEstados()
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception("Sem conexão com Internet");

            var url = Settings.BaseUri + Settings.EstadoUri + "?all=true";
            var response = await Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var collection = JsonConvert.DeserializeObject<ICollection<Estado>>(content);
            return collection;
        }

        public async Task<ICollection<Cidade>> GetCidades(string estadoId)
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception("Sem conexão com Internet");

            var url = Settings.BaseUri + Settings.CidadeUri+ "estadoid="+ estadoId + "&all=true";
            var response = await Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var collection = JsonConvert.DeserializeObject<ICollection<Cidade>>(content);
            return collection;
        }
    }
}