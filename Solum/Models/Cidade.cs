using System.Threading.Tasks;
using Newtonsoft.Json;
using Solum.Service;

namespace Solum.Models
{
    public class Cidade : EntityData
    {
        public string Nome { get; set; }
        public string EstadoId { get; set; }

        Estado _estado;
        [JsonIgnore]
        public Estado Estado { 
            get {
				if (_estado == null)
				{
					Task.Run(async () => _estado = await AzureService.Instance.FindEstadoAsync(EstadoId)).Wait();
				}
				return _estado;
            }
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}