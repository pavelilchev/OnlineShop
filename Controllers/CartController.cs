namespace OnlineShop.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Models;
    using Models.ViewModels;

    public class CartController : BaseController
    {
        public ActionResult Index()
        {
            var products = this.Data.Products.Where(p => p.Quantity > 0).ToList();

            return this.View(products);
        }

        [Authorize]
        public ActionResult AddToCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = this.Data.Products.Find(id);
            if (product == null)
            {
                return this.HttpNotFound();
            }

            var cartProduct = new CartProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Picture = product.Picture,
                Quantity = product.Quantity
            };

            return this.View(cartProduct);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart([Bind(Include = "Id,Quantity,")]CartProductViewModel cartProduct)
        {
            int id = cartProduct.Id;

            Product product = this.Data.Products.Find(id);
            if (product == null)
            {
                return this.HttpNotFound();
            }

            int quantity = cartProduct.Quantity;
            if (quantity > 0 || quantity <= product.Quantity)
            {
                string currentUserId = this.User.Identity.GetUserId();
                Cart cart = this.Data.Carts.FirstOrDefault(x => x.User.Id == currentUserId && x.IsActive);
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        User = this.Data.Users.FirstOrDefault(u => u.Id == currentUserId),
                        IsActive = true
                    };

                    this.Data.Carts.Add(cart);
                }

                var newCartProduct = new CartProduct()
                {
                    Product = product,
                    Quantity = quantity
                };

                cart.Products.Add(newCartProduct);

                this.Data.SaveChanges();

                return this.RedirectToAction("Index");
            }

            return this.View(product);
        }

        [Authorize]
        public ActionResult MyCart()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var cart = this.Data.Carts.Include(c => c.Products).FirstOrDefault(x => x.User.Id == currentUserId && x.IsActive);

            if (cart == null || cart.Products.Count == 0)
            {
                return this.View("EmptyCart");
            }

            var products = cart.Products.ToList();
            this.ViewData["productsCount"] = products.Sum(p => p.Quantity);
            this.ViewData["productsPrice"] = products.Sum(x => x.Product.Price);

            return this.View(products);
        }

        [Authorize]
        public ActionResult RemoveFromCart(int? cartProductId)
        {
            if (cartProductId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = this.Data.CartProducts.Find(cartProductId);
            if (product == null)
            {
                return this.HttpNotFound();
            }

            this.Data.CartProducts.Remove(product);
            this.Data.SaveChanges();

            return this.RedirectToAction("MyCart");
        }

        [Authorize]
        public ActionResult Finalizepurchase()
        {
            string currentUserId = this.User.Identity.GetUserId();
            Cart cart = this.Data.Carts.Include(c => c.Products).FirstOrDefault(x => x.User.Id == currentUserId && x.IsActive);
            if (cart == null)
            {
                return this.HttpNotFound();
            }

            foreach (var product in cart.Products)
            {
                if (product.Quantity > product.Product.Quantity)
                {
                    return this.RedirectToAction("EditCart", new { cartId = cart.Id, cartProductId = product.Id });
                }

                product.Product.Quantity -= product.Quantity;
            }

            cart.IsActive = false;
            this.Data.SaveChanges();

            return this.View();
        }

        [Authorize]
        public ActionResult EditCart(int cartId, int cartProductId)
        {
            var cartProduct = this.Data.CartProducts.Find(cartProductId);
            this.ViewData["quantity"] = cartProduct.Product.Quantity;

            return this.View(cartProduct);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditCart([Bind(Include = "Id,Quantity,")]CartProduct cartProduct)
        {
            int id = cartProduct.Id;
            string currentUserId = this.User.Identity.GetUserId();
            Cart cart = this.Data.Carts.FirstOrDefault(x => x.User.Id == currentUserId && x.IsActive);
            if (cart == null)
            {
                return this.HttpNotFound();
            }

            int quantity = cartProduct.Quantity;
            if (quantity > 0 || quantity <= cartProduct.Product.Quantity)
            {
                var cp = this.Data.CartProducts.Find(id);
                cp.Quantity = quantity;
                this.Data.SaveChanges();

                return this.RedirectToAction("MyCart");
            }

            this.ViewData["quantity"] = cartProduct.Product.Quantity;
            return this.View(cartProduct);
        }
    }
}