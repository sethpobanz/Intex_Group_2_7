namespace SecurityLab.Models
{
    public interface IProductInterface
    {
        public IQueryable<Product> Products { get; }
    }
}
