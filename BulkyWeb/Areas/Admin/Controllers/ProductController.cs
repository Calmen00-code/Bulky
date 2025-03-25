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
                // System.Diagnostics.Debug.WriteLine(productVM.Product.ImageUrl);
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
                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    //if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    //{
                    //    // updating new image, deleting old image
                    //    // removing the first forward slash
                    //    var oldImagePath =
                    //        Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                    //    if (System.IO.File.Exists(oldImagePath))
                    //    {
                    //        System.IO.File.Delete(oldImagePath);
                    //    }
                    //}

                    //using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    //{
                    //    file.CopyTo(fileStream);
                    //}

                    //productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    // Create path
                    _unitOfWork.ProductRepository.Add(productVM.Product);
                    TempData["success"] = "Product created successfully!";
                }
                else
                {
                    // Update path
                    _unitOfWork.ProductRepository.Update(productVM.Product);
                    TempData["success"] = "Product updated successfully!";
                }

                _unitOfWork.Save();
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

        // this allow API to be called by external sources
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = products});
        }

        // remove this HttpDelete tag when u are not using SweetAlert
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            // var oldImagePath = 
            //     Path.Combine(wwwRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

            //if (System.IO.File.Exists(oldImagePath))
            //{
            //    System.IO.File.Delete(oldImagePath);
            //}

            _unitOfWork.ProductRepository.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new {success = true, message = "Deleted Successful"});
        }

        #endregion
    }
}
