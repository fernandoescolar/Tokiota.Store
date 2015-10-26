namespace Tokiota.Store.Models
{
    using System.Collections;
    using System.Collections.Generic;

    public class PaginatedListModel<TItem> : IEnumerable<TItem>, IPaginatedListModel
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public IEnumerable<TItem> Items { get; set; }
        public IEnumerator<TItem> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}