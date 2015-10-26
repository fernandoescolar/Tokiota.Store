namespace Tokiota.Store.Models.Cart
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Cart : IEnumerable<CartItem>
    {
        private readonly List<CartItem> items = new List<CartItem>();

        public decimal TotalPrice { get { return this.items.Any() ? this.items.Sum(i => i.TotalPrice) : 0m; } }

        public int Count { get { return this.items.Any() ? this.items.Sum(i => i.Number) : 0; } }

        public void Add(CartItem item)
        {
            var exists = this.items.FirstOrDefault(i => i.Id == item.Id);
            if (exists == null)
            {
                item.Number = 1;
                this.items.Add(item);
            }
            else
            {
                exists.Number += 1;
            }
        }

        public void Remove(string id)
        {
            var exists = this.items.FirstOrDefault(i => i.Id == id);
            if (exists != null)
            {
                this.items.Remove(exists);
            }
        }

        public void ChangeNumber(string id, int number)
        {
            var exists = this.items.FirstOrDefault(i => i.Id == id);
            if (exists != null)
            {
                exists.Number = number;
            }
        }

        public IEnumerator<CartItem> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }
    }
}