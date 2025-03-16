using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        // this allow API to be called by external sources
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "ApplicationUser").ToList();

            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PAYMENT_STATUS_PENDING);
                    break;

                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.ORDER_STATUS_PROCESSING);
                    break;

                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.ORDER_STATUS_SHIPPED);
                    break;

                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.ORDER_STATUS_APPROVED);
                    break;
            }

            return Json(new { data = orderHeaders });
        }

        #endregion
    }
}
