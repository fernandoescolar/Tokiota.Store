namespace Tokiota.Store.Domain.Catalog.Services
{
    using Infrastructure.Caching;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class TopListService : ITopListService
    {
        private const string ViewedCountListKey = "ViewedCountListKey";
        private const string MostViewedKey = "MostViewedKey";
        private readonly ICache cache;
        private readonly ICatalogService service;

        public TopListService(ICache cache, ICatalogService service)
        {
            this.cache = cache;
            this.service = service;
        }

        public IEnumerable<TopListItem> GetMostViewed()
        {
            var top = this.GetViewedCountList();
            return top.OrderByDescending(o => o.Weight).Take(5);
        }

        public IEnumerable<TopListItem> GetMostBuyed()
        {
            return this.GetMostViewed();
        }

        public IEnumerable<TopListItem> GetNews()
        {
            return this.GetMostViewed();
        }

        public void Touch(string id)
        {
            var top = this.GetViewedCountList();
            var item = top.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                item.Weight++;
                this.cache.Set(ViewedCountListKey, top, TimeSpan.FromDays(1));
            }
        }

        private List<TopListItem> GetViewedCountList()
        {
            return this.cache.Get(ViewedCountListKey, () =>
            {
                int total;
                var products = this.service.GetProducts(int.MaxValue, 0, out total);
                return products.Select(p => new TopListItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Weight = 0
                }).ToList();
            }, TimeSpan.FromDays(1));
        }

    }
}
