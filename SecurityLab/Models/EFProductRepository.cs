using System.Linq;

namespace SecurityLab.Models
{
    public class EFProductRepository : IProductInterface
    {
        private readonly PobanzTestDbContext _context;

        public EFProductRepository(PobanzTestDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Products => _context.Products;

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Remove(Product product)
        {
            _context.Products.Remove(product);
        }
    }
}



