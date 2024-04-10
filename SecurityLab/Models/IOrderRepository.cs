namespace SecurityLab.Models
{

    public interface IOrderRepository
    {

        IQueryable<Purchase> Purchases { get; }
        void SaveOrder(Purchase order);
    }
}
