using MongoDB.Driver;

namespace Infra.Data.Data
{
    public interface IDbContext : IDisposable
    {
        IMongoCollection<T> Collection<T>();
        IMongoCollection<T> Collection<T>(string collectionName);
    }
}
