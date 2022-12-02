using Application.Domain.Models;
using Application.Domain.Repositories;
using Infra.Data.Data;
using MongoDB.Driver;

namespace Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository<Product>
    {
        private readonly IMongoCollection<Product> _collection;

        public ProductRepository(IDbContext context)
        {
            _collection = context.Collection<Product>();
        }

        public void Add(Product obj)
        {
            _collection.InsertOne(obj);
        }

        public IEnumerable<Product> List()
        {
            var products = _collection.Find(obj => true).ToList();
            return products;
        }
    }
}
