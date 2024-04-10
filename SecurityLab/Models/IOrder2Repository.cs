namespace SecurityLab.Models
{
    public interface IOrder2Repository
    {
        public IQueryable<Order> Orders { get; }

    }
}
