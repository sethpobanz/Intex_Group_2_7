namespace SecurityLab.Models
{
    public class EFOrder2Repository : IOrder2Repository
    {
        private PobanzTestDbContext _contextOrder;

        public EFOrder2Repository(PobanzTestDbContext contextOrder)
        {
            _contextOrder = contextOrder;
        }
        public IQueryable<Order> Orders => _contextOrder.Orders;
    }
}
