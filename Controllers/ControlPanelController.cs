namespace OnlineShop.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Models.ViewModels;
    using Models;
    using Utils;

    [Authorize(Roles = "Admin")]
    public class ControlPanelController : BaseController
    {
        public ActionResult Index()
        {
            var products = this.Data.Products.ToList();

            return this.View(products);
        }

        public ActionResult Create()
        {
            return this.View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, Quantity, Price, Picture")]CreateProductViewModel product)
        {
            byte[] picture = FileConverter.ConvertFileToByteArray(product.Picture);

            if (this.ModelState.IsValid && picture != null)
            {

                var newProduct = new Product()
                {
                    Name = product.Name,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    Picture = picture
                };

                this.Data.Products.Add(newProduct);
                this.Data.SaveChanges();

                return this.RedirectToAction("Index");
            }

            return this.View(product);
        }

        public ActionResult Edit(int? id)
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

            var editProduct = new EditProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity,
                Price = product.Price,
                Picture = null,
                PictureAsBytes = product.Picture
            };

            return this.View(editProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Quantity,Price,Picture,PictureAsBytes")] EditProductViewModel editedProduct)
        {
            if (this.ModelState.IsValid)
            {
                var product = this.Data.Products.Find(editedProduct.Id);
                if (product == null)
                {
                    return this.HttpNotFound();
                }

                product.Name = editedProduct.Name;
                product.Quantity = editedProduct.Quantity;
                product.Price = editedProduct.Price;
                product.Picture = editedProduct.Picture != null ? FileConverter.ConvertFileToByteArray(editedProduct.Picture) : editedProduct.PictureAsBytes;


                this.Data.Entry(product).State = EntityState.Modified;
                this.Data.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(editedProduct);
        }


        public ActionResult Delete(int? id)
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
            return this.View(product);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = this.Data.Products.Find(id);
            if (product != null)
            {
                this.Data.Products.Remove(product);
            }

            this.Data.SaveChanges();
            return this.RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Data.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
