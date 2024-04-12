
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SecurityLab.Models
{
    public class EFCustomerRepository : ICustomerRepository
    {
        private readonly PobanzTestDbContext _dbContext;

        public EFCustomerRepository(PobanzTestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Customer> Customers => _dbContext.Customers;

        public void Add(Customer customer)
        {
            _dbContext.Customers.Add(customer);
        }

        public void Remove(Customer customer)
        {
            _dbContext.Customers.Remove(customer);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Customer customer)
        {
            _dbContext.Customers.Update(customer);
        }
    }
}
