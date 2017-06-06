using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Solum.Models;

namespace Solum.Interfaces
{
    public interface IDataService
    {
        void SetCredentials(MobileServiceUser user);
        Task<IList<Estado>> ListEstadosAsync();
        Task<IList<Cidade>> ListCidadesAsync(string estadoId);
        Task<Cidade> FindCidadeAsync(string cidadeId);
        Task AddOrUpdateFazendaAsync(Fazenda fazenda);
        Task DeleteFazendaAsync(Fazenda fazenda);
        Task<Fazenda> FindFazendaAsync(string fazendaid);
        Task<IList<Fazenda>> ListFazendaAsync();
        Task AddOrUpdateTalhaoAsync(Talhao talhao);
        Task DeleteTalhaoAsync(Talhao talhao);
        Task<Talhao> FindTalhaoAsync(string talhaoid);
        Task<IList<Talhao>> ListTalhaoAsync(string fazendaId);
        Task<bool> TalhaoHasAnalisesAsync(string talhaoid);
        Task AddOrUpdateAnaliseAsync(Analise analise);
        Task DeleteAnaliseAsync(Analise analise);
        Task<Analise> FindAnaliseAsync(string analiseid);
        Task<IList<Analise>> ListAnaliseAsync();
        Task SynchronizeAnaliseAsync();
        Task SynchronizeFazendaAsync();
        Task SynchronizeTalhaoAsync();
        Task SynchronizeAllAsync();
    }
}
