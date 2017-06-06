using System.Threading.Tasks;
using Newtonsoft.Json;
using Solum.Service;

namespace Solum.Models
{
    public class Fazenda : EntityData
    {
        public string Nome { get; set; }
        public string CidadeId { get; set; }

        Cidade _cidade;
		[JsonIgnore]
        public Cidade Cidade
        {
			get
			{
				if (_cidade == null)
				{
                    Task.Run(async () => _cidade = await AzureService.Instance.FindCidadeAsync(CidadeId)).Wait();
				}
				return _cidade;
			}
        }
        public override string ToString()
        {
            return Nome;
        }
    }
}