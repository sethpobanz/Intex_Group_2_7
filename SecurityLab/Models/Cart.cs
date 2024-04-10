namespace SecurityLab.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public virtual void AddItem(Product b, int quantity)
        {
            CartLine? line = Lines
                .Where(x => x.Product.ProductId == b.ProductId).FirstOrDefault();
            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = b,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product b) => Lines.RemoveAll(x => x.Product.ProductId == b.ProductId);

        public virtual void Clear() => Lines.Clear();
        public decimal CalculateTotal()
        {
            var total = Lines.Sum(x => x.Product.Price * x.Quantity);

            return (decimal)total;
        }
        public class CartLine
        {
            public int CartLineId { get; set; }
            public Product Product { get; set; } = new();
            public int Quantity { get; set; }

        }


    }
}
