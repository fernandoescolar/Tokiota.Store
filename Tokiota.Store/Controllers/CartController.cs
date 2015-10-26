namespace Tokiota.Store.Controllers
{
    using System.Web.Mvc;
    using Domain.Catalog.Services;
    using Models.Cart;

    public class CartController : Controller
    {
        private readonly ICatalogService catalogService;

        public CartController(ICatalogService storeService)
        {
            this.catalogService = storeService;
        }

        public ActionResult Index()
        {
            var cart = this.GetCurrentCart();
            var model = new CartInfoModel {Count = cart.Count, TotalPrice = cart.TotalPrice};
            return PartialView("Index", model);
        }

        public ActionResult List()
        {
            var model = this.GetCurrentCart();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(string productId)
        {
            var product = this.catalogService.GetProduct(productId);
            var cart = this.GetCurrentCart();
            var item = new CartItem { Id = product.Id, Name = product.Name, Price = product.Price };
            cart.Add(item);

            return this.Index();
        }

        private Cart GetCurrentCart()
        {
            var cart = Session["MyCart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                Session["MyCart"] = cart;
            }

            return cart;
        }
    }
}