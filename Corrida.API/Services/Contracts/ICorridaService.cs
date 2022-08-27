using CorridaAPI.Model;
using CorridaAPI.Model.CorridaContext;

namespace CorridaAPI.Services.Contracts;

public interface ICorridaService
{
    Task Abrir(Corrida novaCorrida);
    Task AtulizarEstado(string corridaId, string estado, float? estrela);
    Task PegarCorrida(string corridaId, int mototaxistaId);

    Task<List<Corrida>> ObterTodasCorrida(RequestQuery requestQuery);

}
