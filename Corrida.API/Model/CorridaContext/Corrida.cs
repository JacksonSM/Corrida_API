using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CorridaAPI.Model.CorridaContext;

public class Corrida
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public int PassageiroId { get; set; }
    public int? MotoTaxistaId { get; set; }
    public Localizacao Origem { get; set; } = new Localizacao();
    public Localizacao Destino { get; set; } = new Localizacao();
    public string Estado { get; set; } = string.Empty;
    public float? Estrelas { get; set; }


}
