namespace SecurityLab.Models
{
    public interface ICustomerRepository
    {
        public IQueryable<Customer> Customers {  get; }
        void Add(Customer customer);
        void SaveChanges();
        void Remove(Customer customer);
        void Update(Customer customer);
    }
}
