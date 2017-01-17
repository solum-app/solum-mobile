using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Solum.Models;

namespace Solum.Remotes
{
    public class EstadoRemote : BaseRemote
    {

        public ICollection<Estado> GetEstados()
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception("Sem conexão com Internet");
            try
            {
                var url = Client.BaseAddress+Settings.EstadoUri+"?all=true";
                var response = Client.GetAsync(url).Result;
                var jsonData = JsonConvert.DeserializeObject<ICollection<Estado>>(response.Content.ReadAsStringAsync().Result);
                return jsonData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ICollection<Cidade> GetCidades(string estadoId)
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception("Sem conexão com Internet");
            try
            {
                var url = Client.BaseAddress + Settings.CidadeUri + "?estadoid=" + estadoId + "&all=true";
                var response = Client.GetAsync(url).Result;
                var jsonData = JsonConvert.DeserializeObject<ICollection<Cidade>>(response.Content.ReadAsStringAsync().Result);
                return jsonData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}