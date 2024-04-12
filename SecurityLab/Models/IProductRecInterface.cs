namespace SecurityLab.Models
{
    public interface IProductRecInterface
    {
        public IQueryable<ProductPipeline> ProductPipelines { get; }
    }
}
