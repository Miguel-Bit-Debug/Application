namespace Application.Domain.Repositories
{
    public interface IProductRepository<T> where T : class
    {
        IEnumerable<T> List();
        void Add(T obj);
    }
}
