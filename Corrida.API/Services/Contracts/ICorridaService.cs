using CorridaAPI.Model.CorridaContext;

namespace CorridaAPI.Services.Contracts;

public interface ICorridaService
{
    Task Abrir(Corrida novaCorrida);
    Task AtulizarEstado(string corridaId, string estado, float? estrela);
    Task PegarCorrida(string corridaId, int mototaxistaId);

    Task<List<Corrida>> ObterTodasCorridaMototaxista(int mototaxistaId);
    Task<List<Corrida>> ObterTodasCorridaPassageiro(int passageiroId);
    Task<List<Corrida>> ObterTodasCorridaCidade(string cidade);
    Task<List<Corrida>> ObterTodasCorridaBairro(string bairro);
}
