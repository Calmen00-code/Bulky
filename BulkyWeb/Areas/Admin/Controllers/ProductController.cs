using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }

        public IActionResult Upsert(int? id)
        {

            // creating a dropdown category list using ViewBag
            // ViewBag.CategoryList = CategoryList;

            // creating a dropdown category list using model view
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.CategoryRepository
                    .GetAll().Select(u=> new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                Product = new Product()
            };

            // Create path
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                // Update path
                productVM.Product = _unitOfWork.ProductRepository.Get(u=>u.Id == id);
                System.Diagnostics.Debug.WriteLine(productVM.Product.ImageUrl);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (productVM == null || !ModelState.IsValid)
            {
                return View();
            }

            if (productVM.Product.Title == null || productVM.Product.Title == "")
            {
                ModelState.AddModelError("Name", "Display order cannot be the same as Name");
                return View();
            }

            if (productVM.Product.Title != null && productVM.Product.Title.ToLower() == "test")
            {
                ModelState.AddModelError("", "test is not a valid name");
                return View();
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // updating new image, deleting old image
                        // removing the first forward slash
                        var oldImagePath =
                            Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    // Create path
                    _unitOfWork.ProductRepository.Add(productVM.Product);
                }
                else
                {
                    // Update path
                    _unitOfWork.ProductRepository.Update(productVM.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.CategoryRepository
                    .GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
                return View(productVM);
            }
        }

        public IActionResult Delete(int? id)
        {
            Product? product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            Product? productDelete = _unitOfWork.ProductRepository.Get(u => u.Id == product.Id);

            if (productDelete == null)
            {
                System.Diagnostics.Debug.WriteLine("not found");
                return NotFound();
            }
            _unitOfWork.ProductRepository.Remove(productDelete);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction("Index", "Product");
        }
    }
}
