namespace OnlineShop.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Security;
    using Microsoft.AspNet.Identity;
    using Models;
    using Models.ViewModels;

    public class CustomerController : BaseController
    {
        public ActionResult Index()
        {
            var products = this.Data.Products.ToList();

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
            if (quantity> 0 || quantity <= product.Quantity)
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
            var cart = this.Data.Carts.FirstOrDefault(x => x.User.Id == currentUserId && x.IsActive);

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
    }
}