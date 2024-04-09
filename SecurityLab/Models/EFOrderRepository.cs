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

        public IQueryable<Order> Orders => context.Orders
                            .Include(o => o.Lines)
                            .ThenInclude(l => l.Legoproduct);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Legoproduct));
            if (order.TransactionId == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
