using Humanizer;
using Microsoft.EntityFrameworkCore;
using SecurityLab.Models;
using System.Collections.Generic;

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

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.AttachRange(order.Lines.Select(l => l.Legoproduct));

                    // Set the Date property to the current date
                    order.Date = DateOnly.FromDateTime(DateTime.Today);
                    order.DayOfWeek = DateTime.Today.DayOfWeek.ToString();
    
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
                        order.Customer = new Customer
                        {
                            FirstName = order.Customer.FirstName,
                            LastName = order.Customer.LastName,
                            CountryOfResidence = order.Customer.CountryOfResidence
                        };
                        context.Customers.Add(order.Customer);
                    }

                    // Ensure IDENTITY_INSERT is ON before inserting
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Orders ON");
                    context.Orders.Add(order);
                    context.SaveChanges();
                    // Ensure IDENTITY_INSERT is OFF after inserting
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Orders OFF");
                    // Commit transaction if no errors occurred
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // Rollback transaction if an error occurred
                    transaction.Rollback();
                    throw; // Re-throw the exception to maintain the error information
                }
            }
            // Add the new Order without checking for TransactionId
        }
    }
}

