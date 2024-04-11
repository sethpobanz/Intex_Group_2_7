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
            
            // Check if the Customer exists
            var customer = context.Customers.Find(order.CustomerId);

            if (customer != null)
            {
                // Attach the existing Customer
                order.Customer = customer;
            }
            else
            {
                // Create a new Customer and attach it to the Order
                order.Customer = new Customer { /* Set customer properties */ };
                context.Customers.Add(order.Customer);
            }

            context.Orders.Add(order);
            context.SaveChanges();
        }
    }
}

