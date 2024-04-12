namespace SecurityLab.Models
{
    public class EFProductRecRepository : IProductRecInterface
    {
        private readonly PobanzTestDbContext _context;

        public EFProductRecRepository(PobanzTestDbContext context)
        {
            _context = context;
        }

        public IQueryable<ProductPipeline> ProductPipelines => _context.ProductPipelines;
    }
}
