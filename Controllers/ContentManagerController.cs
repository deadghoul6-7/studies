using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ProjectAspEShop2024.AppIdentity;
using ProjectAspEShop2024.BusinesLogic;
using ProjectAspEShop2024.DataAccessLayer;
using ProjectAspEShop2024.Domains;
using ProjectAspEShop2024.Dto;
using ProjectAspEShop2024.Helpers;
using ProjectAspEShop2024.Models;

namespace ProjectAspEShop2024.Controllers
{
    [Authorize(Roles ="content_manager")]
    public class ContentManagerController : Controller
    {
        private readonly ShopContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger _logger;

        public ContentManagerController(
            ShopContext context, 
            IMapper mapper, 
            UserManager<AppUser> userManager,
            ILogger<ContentManagerController> logger)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

		public IActionResult UploadFileDocumentsView()
		{
            var model = new FileDocumentViewModel();
			return View(model);
		}

        [HttpPost]
        public async Task<IActionResult> UploadFileDocumentsView(FileDocumentViewModel viewModel)
        {
            var allErrors = ModelState
                .Values.SelectMany(v => v.Errors).ToList();

            var model = new FileDocument();
            if (ModelState.IsValid)
            {
                UploadFileDocument(viewModel, model);

                if (model.FileDocumentId == 0)
                {
                    await _context.CreateFileDocumentAsync(_userManager, User, model);
                }
                else
                {
                    await _context.UpdateFileDocumentAsync(_userManager, User, model);
                }

                return new RedirectResult("Index");
            }

            return View(viewModel);
        }

        private void UploadFileDocument(
            FileDocumentViewModel viewModel, 
            FileDocument model)
        {
            if (viewModel.FormFile == null)
            {
                _logger.LogError("image not found");
                return;
            }
            else
            {
                _logger.LogInformation($"image '{viewModel.FormFile.FileName}' found");
            }

            // создадим уникальное имя файла
            string uniqueName = Guid.NewGuid().ToString();
            // сохраним расширение файла
            string ext = Path.GetExtension(viewModel.FormFile.FileName);
            uniqueName += uniqueName + ext;

            string directoryName = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "FileDocuments");

            string filename = Path.Combine(
                directoryName,
                uniqueName);

            Directory.CreateDirectory(directoryName);

            // сохраним файл на сервере, в папке
            using (var stream = System.IO.File.Create(filename))
            {
                viewModel.FormFile.CopyTo(stream);
            }

            model.StorageId = uniqueName;
            model.Name = viewModel.Name;
        }

        public IActionResult EditCategoriesView()
        {
            var categories = _context
                .GetAllCategories()
                .Select(x => _mapper.Map<CategoryViewModel>(x))
                .ToList();

            return View(categories);
        }

        public IActionResult CreateCategory()
        {
            var model = new CategoryViewModel();

            return View("EditCategoryView", model);
        }

        public IActionResult DeleteCategory(long categoryId)
        {
            try
            {
                _context.DeleteCategory(categoryId);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = "Ошибка при удалении категории (возможно - категория не пустая)";
                _logger.LogError(ex.ToString());
                return View("Error");
            }

            return new RedirectResult("EditCategoriesView");
        }

        public IActionResult EditCategoryView(long categoryId)
        {
            var category = _context.Categories
                .FirstOrDefault(c => c.CategoryId == categoryId);

            var model = _mapper.Map<CategoryViewModel>(category);

            return View(model);
        }

        [HttpPost]
        public IActionResult EditCategoryView(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CategoryId == 0)
                {
                    _context.CreateCategory(model);
                }
                else
                {
                    _context.UpdateCategory(model);
                }

                return new RedirectResult("EditCategoriesView");
            }

            return View(model);
        }

        public IActionResult AddProductView(string returnUrl)
        {
            var model = new ProductEditViewModel();
            model.Initialize(_context);
            model.ReturnUrl = returnUrl;

            return View("EditProductView", model);
        }


        public IActionResult DeleteProduct(long productId, string returnUrl)
        {
            try
            {
                _context.DeleteProduct(productId);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ошибка при удалении продукта";
                _logger.LogError(ex.ToString());
                return View("Error");
            }

            return new RedirectResult(returnUrl);
        }

        public IActionResult EditProductView(long productId, string returnUrl)
        {
            var product = _context
                .Products
                .Include(p => p.BrandOfProduct)
                .Include(p => p.CategoryOfProduct)
                .FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                ViewBag.ErrorMessage = $"не найден продукт с id '{productId}'";
                return View("Error");
            }

            var productEditModel = _mapper
                .Map<ProductEditViewModel>(product);

            // инициализация, для создания списков
            // брендов и категорий!
            productEditModel.Initialize(_context, product);
            productEditModel.ReturnUrl = returnUrl;

            return View(productEditModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditProductView(ProductEditViewModel model)
        {
            var allErrors = ModelState
                .Values.SelectMany(v => v.Errors).ToList();

            if (ModelState.IsValid)
            {
                UploadImage(model);

                if (model.ProductId == 0)
                {
                    _context.CreateProduct(_mapper, model);
                }
                else
                {
                    await _context.UpdateProductAsync(_userManager, User, model);
                }

                return new RedirectResult(model.ReturnUrl);
            } 

            return View(model);
        }

        private void UploadImage(ProductEditViewModel model)
        {
            if (model.ImageFile == null)
            {
                _logger.LogError("image not found");
                return;
            }
            else
            {
                _logger.LogInformation($"image '{model.ImageFile.FileName}' found");
            }

            // создадим уникальное имя файла
            string uniqueName = Guid.NewGuid().ToString();
            // сохраним расширение файла
            string ext = Path.GetExtension(model.ImageFile.FileName);
            uniqueName += uniqueName + ext;

            // путь к папке на сервере - где картинки
            string filename = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "ProductImages",
                uniqueName);

            // сохраним файл на сервере, в папке
            using (var stream = System.IO.File.Create(filename))
            {
                model.ImageFile.CopyTo(stream);
            }

            model.PhotoUrl = uniqueName;
        }
    }
}
