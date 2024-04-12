namespace SecurityLab.Models
{
    public interface ILastInterface
    {
        public IQueryable<CustomerPipeline> CustomerPipelines { get; }
    }
}
