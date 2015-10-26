namespace Tokiota.Store.Domain.Catalog.Services
{
    using Model;
    using System.Collections.Generic;

    public interface ITopListService
    {
        IEnumerable<TopListItem> GetMostBuyed();
        IEnumerable<TopListItem> GetMostViewed();
        IEnumerable<TopListItem> GetNews();
        void Touch(string id);
    }
}