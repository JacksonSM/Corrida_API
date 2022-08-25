using CorridaAPI.Model.CorridaContext;

namespace CorridaAPI.Services.Contracts;

public interface ICorridaService
{
    Task Criar(Corrida novaCorrida);
    Task AtulizarEstado(string corridaId, string estado);
    Task PegarCorrida(string corridaId, int mototaxistaId);

    Task ObterTodasCorridaMototaxista(int mototaxistaId);
    Task ObterTodasCorridaPassageiro(int passageiroId);
    Task ObterTodasCorridaCidade(string cidade);
    Task ObterTodasCorridaBairro(string bairro);
}
