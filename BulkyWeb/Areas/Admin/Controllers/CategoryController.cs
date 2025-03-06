using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Bulky.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.ROLE_ADMIN)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.CategoryRepository.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category == null || !ModelState.IsValid)
            {
                return View();
            }

            if (category.Name != null && category.Name.ToLower() == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display order cannot be the same as Name");
                return View();
            }

            if (category.Name != null && category.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "test is not a valid name");
                return View();
            }

            _unitOfWork.CategoryRepository.Add(category);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully!";
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //Category? category = _db.Categories.Find(id);
            Category? category = _unitOfWork.CategoryRepository.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category == null || !ModelState.IsValid)
            {
                return View();
            }

            if (category.Name != null && category.Name.ToLower() == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display order cannot be the same as Name");
                return View();
            }

            if (category.Name != null && category.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "test is not a valid name");
                return View();
            }

            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully!";
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Delete(int? id)
        {
            Category? category = _unitOfWork.CategoryRepository.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            Category? categoryDelete = _unitOfWork.CategoryRepository.Get(u => u.Id == category.Id);

            if (categoryDelete == null)
            {
                System.Diagnostics.Debug.WriteLine("not found");
                return NotFound();
            }
            _unitOfWork.CategoryRepository.Remove(categoryDelete);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index", "Category");
        }
    }
}
