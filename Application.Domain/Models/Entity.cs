using MongoDB.Bson.Serialization.Attributes;

namespace Application.Domain.Models;

public abstract class Entity
{
    [BsonId]
    public Guid Id { get; private set; }
}
