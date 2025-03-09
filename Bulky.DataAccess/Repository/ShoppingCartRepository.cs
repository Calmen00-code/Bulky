using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base (db)
        {
            _db = db;
        }

        public void Update(ShoppingCart cart)
        {
            var shoppingCartFromDb = _db.ShoppingCarts.FirstOrDefault(u => u.Id == cart.Id);

            if (shoppingCartFromDb != null)
            {
                shoppingCartFromDb.ProductId = cart.ProductId;
                shoppingCartFromDb.Product = cart.Product;
                shoppingCartFromDb.Count = cart.Count;
                shoppingCartFromDb.ApplicationUserId = cart.ApplicationUserId;
                shoppingCartFromDb.ApplicationUser = cart.ApplicationUser;
            }
        }
    }
}
