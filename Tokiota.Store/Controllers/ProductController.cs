namespace Tokiota.Store.Controllers
{
    using Domain;
    using Domain.Catalog.Model;
    using Domain.Catalog.Services;
    using Domain.Identity.Model;
    using Models;
    using Models.Product;
    using System.Linq;
    using System.Web.Mvc;

    public class ProductController : Controller
    {
        private readonly ICatalogService service;
        private readonly IImageStorageService imageService;
        private readonly ITopListService topService;
        private readonly IMapper<Product, ProductModel> listMapper;
        private readonly IMapper<Product, EditProductModel> editMapper;

        public ProductController(ICatalogService service, IImageStorageService imageService, ITopListService topService, IMapper<Product, ProductModel> listMapper, IMapper<Product, EditProductModel> editMapper)
        {
            this.service = service;
            this.imageService = imageService;
            this.topService = topService;
            this.listMapper = listMapper;
            this.editMapper = editMapper;
        }

        public ActionResult Details(string id)
        {
            try
            {
                var product = this.service.GetProduct(id);
                var model = listMapper.Map(product);
                this.topService.Touch(id);
                return View(model);
            }
            catch
            {
                return HttpNotFound();
            }
        }

        public ActionResult List(int id = 1, int pageSize = 10)
        {
            var model = new PaginatedListModel<ProductModel>();
            var total = 0;
            model.Items = this.service.GetProducts(pageSize, id - 1, out total).Select(this.listMapper.Map);
            model.Total = total;
            model.Page = id;
            model.PageSize = pageSize;
            return View(model);
        }

        [Authorize(Roles = TokiotaStoreRoles.Admin)]
        public ActionResult Create()
        {
            var model = new EditProductModel();
            return View("Create", model);
        }

        [HttpPost]
        [Authorize(Roles = TokiotaStoreRoles.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditProductModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    model.ImageUrl = this.imageService.SaveImage(model.Image.FileName, model.Image.InputStream);
                }
                else
                {
                    model.ImageUrl = "~/images/none.jpg";
                }

                var entity = this.editMapper.Map(model);
                var result = this.service.CreateProduct(entity);

                if (!result.Succeeded)
                {
                    this.AddErrors(result);
                }
            }

            return View("Edit", model);
        }

        [Authorize(Roles = TokiotaStoreRoles.Admin)]
        public ActionResult Edit(string id)
        {
            var product = this.service.GetProduct(id);
            var model = this.editMapper.Map(product);
            return View("Edit", model);
        }

        [HttpPost]
        [Authorize(Roles = TokiotaStoreRoles.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditProductModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    model.ImageUrl = this.imageService.SaveImage(model.Image.FileName, model.Image.InputStream);
                }

                var entity = this.editMapper.Map(model);
                var result = this.service.UpdateProduct(entity);

                if (!result.Succeeded)
                {
                    this.AddErrors(result);
                }
            }

            return View("Edit", model);
        }

        private void AddErrors(ICallResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}