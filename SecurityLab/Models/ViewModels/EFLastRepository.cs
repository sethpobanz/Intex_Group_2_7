namespace SecurityLab.Models
{
    public class EFLastRecRepository : ILastInterface
    {
        private readonly PobanzTestDbContext _context;

        public EFLastRecRepository(PobanzTestDbContext context)
        {
            _context = context;
        }

        public IQueryable<CustomerPipeline> CustomerPipelines => _context.CustomerPipelines;
    }
}