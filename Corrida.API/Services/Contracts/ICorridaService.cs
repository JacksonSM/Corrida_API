using CorridaAPI.Model.CorridaContext;

namespace CorridaAPI.Services.Contracts;

public interface ICorridaService
{
    Task Abrir(Corrida novaCorrida);
    Task AtulizarEstado(string corridaId, string estado, float? estrela);
    Task PegarCorrida(string corridaId, int mototaxistaId);

    Task<IEnumerable<Corrida>> ObterTodasCorridaMototaxista(int mototaxistaId);
    Task<IEnumerable<Corrida>> ObterTodasCorridaPassageiro(int passageiroId);
    Task<IEnumerable<Corrida>> ObterTodasCorridaCidade(string cidade);
    Task<IEnumerable<Corrida>> ObterTodasCorridaBairro(string bairro);
}
