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

    public async Task Criar(Corrida novaCorrida) =>
    await _corridaCollection.InsertOneAsync(novaCorrida);

    public async Task AtulizarEstado(string corridaId, string estado)
    {
        var update = Builders<Corrida>.Update
            .Set(c => c.Estado, estado);
        await _corridaCollection.UpdateOneAsync(c => c.Id == corridaId, update);       
    }
    public async Task PegarCorrida(string corridaId, int mototaxistaId)
    {
        var update = Builders<Corrida>.Update
             .Set(c => c.MotoTaxistaId, mototaxistaId);
        await _corridaCollection.UpdateOneAsync(c => c.Id == corridaId, update);
    }

    public async Task ObterTodasCorridaBairro(string bairro) =>
        await _corridaCollection.FindAsync(c => c.Origem.Bairro.Equals(bairro));


    public async Task ObterTodasCorridaCidade(string cidade) =>
        await _corridaCollection.FindAsync(c => c.Origem.Cidade.Equals(cidade));

    public async Task ObterTodasCorridaMototaxista(int mototaxistaId) =>
        await _corridaCollection.FindAsync(c => c.MotoTaxistaId == mototaxistaId);

    public async Task ObterTodasCorridaPassageiro(int passageiroId) => 
       await _corridaCollection.FindAsync(c => c.PassageiroId == passageiroId);

}
