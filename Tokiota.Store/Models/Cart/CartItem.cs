namespace Tokiota.Store.Models.Cart
{
    public class CartItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Number { get; set; }

        public decimal TotalPrice { get { return this.Price * this.Number; } }
    }
}