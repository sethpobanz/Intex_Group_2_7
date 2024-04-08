namespace SecurityLab.Models
{
    public class EFProductRepository : IProductInterface
    {
        private PobanzTestDbContext _context;

        public EFProductRepository(PobanzTestDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> Products => _context.Products;
    }
}

