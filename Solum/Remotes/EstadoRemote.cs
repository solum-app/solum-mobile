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

        public async Task<IList<Estado>> GetEstados()
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception("Sem conexão com Internet");
            try
            {
                var url = $"{Settings.BaseUri}{Settings.EstadoUri}?pagesize=30";
                var response = await Client.GetAsync(url);
                var jsonData = JsonConvert.DeserializeObject<IList<Estado>>(await response.Content.ReadAsStringAsync());
                return jsonData;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}