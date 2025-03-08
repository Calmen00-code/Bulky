using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> companies = _unitOfWork.CompanyRepository.GetAll().ToList();
            return View(companies);
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();

            // Create path
            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                // Update path
                company = _unitOfWork.CompanyRepository.Get(u=>u.Id == id);
                return View(company);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (company == null || !ModelState.IsValid)
            {
                return View();
            }

            if (company.Name == null || company.Name == "")
            {
                ModelState.AddModelError("Name", "Display order cannot be the same as Name");
                return View();
            }

            if (company.Name != null && company.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "test is not a valid name");
                return View();
            }

            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    // Create path
                    _unitOfWork.CompanyRepository.Add(company);
                    TempData["success"] = "Company created successfully!";
                }
                else
                {
                    // Update path
                    _unitOfWork.CompanyRepository.Update(company);
                    TempData["success"] = "Company updated successfully!";
                }

                _unitOfWork.Save();
                return RedirectToAction("Index", "Company");
            }
            else
            {
                return View(company);
            }
        }

        // this allow API to be called by external sources
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companies = _unitOfWork.CompanyRepository.GetAll().ToList();
            return Json(new {data = companies});
        }

        // remove this HttpDelete tag when u are not using SweetAlert
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.CompanyRepository.Get(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            _unitOfWork.CompanyRepository.Remove(companyToBeDeleted);
            _unitOfWork.Save();

            return Json(new {success = true, message = "Deleted Successful"});
        }

        #endregion
    }
}
