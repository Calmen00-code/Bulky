using Bulky.DataAccess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if ((category == null) || (!ModelState.IsValid))
            {
                return View();
            }

            if ((category.Name != null) && (category.Name.ToLower() == category.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("Name", "Display order cannot be the same as Name");
                return View();
            }

            if ((category.Name != null) && (category.Name.ToLower() == "test"))
            {
                ModelState.AddModelError("", "test is not a valid name");
                return View();
            }

            _db.Categories.Add(category);
            _db.SaveChanges();
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
            Category? category = _db.Categories.FirstOrDefault(u=>u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category) 
        {
            if ((category == null) || (!ModelState.IsValid))
            {
                return View();
            }

            if ((category.Name != null) && (category.Name.ToLower() == category.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("Name", "Display order cannot be the same as Name");
                return View();
            }

            if ((category.Name != null) && (category.Name.ToLower() == "test"))
            {
                ModelState.AddModelError("", "test is not a valid name");
                return View();
            }

            _db.Categories.Update(category);
            _db.SaveChanges();
            TempData["success"] = "Category updated successfully!";
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Delete(int? id)
        {
            Category? category = _db.Categories.FirstOrDefault(u=>u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            Category? categoryDelete = _db.Categories.FirstOrDefault(u=>u.Id == category.Id);

            if (categoryDelete == null)
            {
                System.Diagnostics.Debug.WriteLine("not found");
                return NotFound();
            }
            _db.Categories.Remove(categoryDelete);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index", "Category");
        }
    }
}
