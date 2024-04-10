using Microsoft.EntityFrameworkCore;
using SecurityLab.Models;

namespace SecurityLab.Models
{

    public class EFOrderRepository : IOrderRepository
    {
        private PobanzTestDbContext context;

        public EFOrderRepository(PobanzTestDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Purchase> Purchases => context.Purchases
                            .Include(o => o.Lines)
                            .ThenInclude(l => l.Legoproduct);

        public void SaveOrder(Purchase order)
        {
            context.AttachRange(order.Lines.Select(l => l.Legoproduct));
            if (order.OrderId == 0)
            {
                context.Purchases.Add(order);
            }
            context.SaveChanges();
        }
    }
}

