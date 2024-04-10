namespace SecurityLab.Models
{
    public interface IProductInterface
    {
        public IQueryable<Product> Products { get; }
        void Update(Product product);
        void Add(Product product);
        void SaveChanges();
        void Remove(Product product);
    }
}
