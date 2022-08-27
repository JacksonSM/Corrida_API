using CorridaAPI.Model;
using CorridaAPI.Model.CorridaContext;
using CorridaAPI.Services.Contracts;
using CorridaAPI.Services.Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CorridaAPI.Services;

public class CorridaService : ICorridaService
{
    private readonly IMongoCollection<Corrida> _corridaCollection;
    public CorridaService(IOptions<CarridaDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

        _corridaCollection = mongoDatabase.GetCollection<Corrida>
            (settings.Value.CorridaCollectionName);
    }

    public async Task Abrir(Corrida novaCorrida) =>
    await _corridaCollection.InsertOneAsync(novaCorrida);
    public async Task AtulizarEstado(string corridaId, string estado, float? estrela)
    {
        var corrida = await _corridaCollection.Find(x => x.Id == corridaId).FirstOrDefaultAsync();
        if (corrida is null) throw new Exception("Corrida não existe.");

        if (estado == "fechado")
        {
            var atualizacoes = Builders<Corrida>.Update
                .Set(c => c.Estado, estado)
                .Set(c => c.Estrelas, estrela);
            await _corridaCollection.UpdateOneAsync(c => c.Id == corridaId, atualizacoes);
        }
        else
        {
            var atualizacoes = Builders<Corrida>.Update
                .Set(c => c.Estado, estado);
            await _corridaCollection.UpdateOneAsync(c => c.Id == corridaId, atualizacoes);
        }                      
    }
    public async Task PegarCorrida(string corridaId, int mototaxistaId)
    {
        var corrida = await _corridaCollection.FindAsync(c => c.Id == corridaId);
        if (corrida is null) throw new Exception("Corrida não existe");

        var update = Builders<Corrida>.Update
             .Set(c => c.MotoTaxistaId, mototaxistaId);
        await _corridaCollection.UpdateOneAsync(c => c.Id == corridaId, update);
    }
    public async Task<List<Corrida>> ObterTodasCorrida(RequestQuery request)
    {   
        var construtorFiltro = new FilterDefinitionBuilder<Corrida>();
        var listaFiltros = new List<FilterDefinition<Corrida>>();

        if (!string.IsNullOrEmpty(request.cidade))
            listaFiltros.Add(construtorFiltro.Eq(c => c.Origem.Cidade, request.cidade));

        if (!string.IsNullOrEmpty(request.bairro))
            listaFiltros.Add(construtorFiltro.Eq(c => c.Origem.Bairro, request.bairro));

        if(request.mototaxistaId != null)
            listaFiltros.Add(construtorFiltro.Eq(c => c.MotoTaxistaId, request.mototaxistaId));

        if(request.passageiroId != null)
            listaFiltros.Add(construtorFiltro.Eq(c => c.PassageiroId, request.passageiroId));

        if(!string.IsNullOrEmpty(request.estado))
            listaFiltros.Add(construtorFiltro.Eq(c => c.Estado, request.estado));

        if(listaFiltros.Count == 0)
            listaFiltros.Add(construtorFiltro.Empty);

        return await _corridaCollection.Find(construtorFiltro.And(listaFiltros)).ToListAsync(); 
    }
}
