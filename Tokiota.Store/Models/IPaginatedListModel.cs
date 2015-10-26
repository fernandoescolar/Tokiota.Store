namespace Tokiota.Store.Models
{
    using System.Collections;

    public interface IPaginatedListModel : IEnumerable
    {
        int PageSize { get; set; }
        int Page { get; set; }
        int Total { get; set; }
    }
}