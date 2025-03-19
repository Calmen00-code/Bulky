using Bulky.DataAccess.Repository.IRepository;
using Bulky.Utility;
using Microsoft.AspNetCore.Mvc;
using Stripe.Issuing;
using System.Security.Claims;

namespace BulkyWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // retrieve the userID of currently logged in user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            if (claim != null)
            {
                // reflect the cart count if user is logged in
                if (HttpContext.Session.GetInt32(SD.SESSION_CART) == null)
                {
                    // setting session for displaying count in cart icon
                    int updateCount = 0;
                    foreach (var userCart in _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value))
                    {
                        updateCount += userCart.Count; 
                    }

                    HttpContext.Session.SetInt32(SD.SESSION_CART, updateCount);
                }

                return View(HttpContext.Session.GetInt32(SD.SESSION_CART));
            }
            else
            {
                // if user is not logged in
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
