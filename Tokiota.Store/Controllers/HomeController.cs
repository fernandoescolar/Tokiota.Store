namespace Tokiota.Store.Controllers
{
    using Domain.Catalog.Model;
    using Domain.Catalog.Services;
    using Models;
    using Models.Product;
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Helpers;

    public class HomeController : Controller
    {
        private readonly ICatalogService service;
        private readonly ITopListService topService;
        private readonly IMapper<Product, ProductModel> mapper;
        
        public HomeController(ICatalogService service, ITopListService topService, IMapper<Product, ProductModel> mapper)
        {
            this.service = service;
            this.topService = topService;
            this.mapper = mapper;
        }
        public ActionResult Index(int id = 1, int pageSize = 6)
        {
            var model = new PaginatedListModel<ProductModel>();
            var total = 0;
            model.Items = this.service.GetProducts(pageSize, id - 1, out total).Select(this.mapper.Map);
            model.Total = total;
            model.Page = id;
            model.PageSize = pageSize;
            return View(model);
        }

        public ActionResult Search(int id = 1, int pageSize = 9, string category = null, string word = null)
        {
            ViewBag.Searched = word;
            ViewBag.Category = category;
            var model = new PaginatedListModel<ProductModel>();
            var total = 0;
            model.Items = this.service.SearchProducts(category, word, pageSize, id - 1, out total).Select(this.mapper.Map);
            model.Total = total;
            model.Page = id;
            model.PageSize = pageSize;
            return View("Index", model);
        }

        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }

        public ActionResult MostViewed()
        {
            var model = this.topService.GetMostViewed();
            return PartialView(model);
        }

        public ActionResult MostBuyed()
        {
            var model = this.topService.GetMostBuyed();
            return PartialView(model);
        }
    }
}