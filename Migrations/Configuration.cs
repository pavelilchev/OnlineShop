namespace OnlineShop.Migrations
{
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.UI.WebControls;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            this.SeedUsers(context);
            this.SeedProduct(context);
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);
                var userToInsert = new User { UserName = "admin", Email = "admin@gmail.com" };

                userManager.Create(userToInsert, "123456");
                userManager.AddToRole(userToInsert.Id, "Admin");
            }
        }

        private void SeedProduct(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {
                var product1 = new Product()
                {
                    Name = "Bench",
                    Quantity = 120,
                    Price = 120.99m,
                    Picture = File.ReadAllBytes(HttpContext.Current.Server.MapPath(@"~\Content\seed\bench.jpg"))
                };
                context.Products.Add(product1);

                var product2 = new Product()
                {
                    Name = "Extended Bench",
                    Quantity = 131,
                    Price = 150.99m,
                    Picture = File.ReadAllBytes(HttpContext.Current.Server.MapPath(@"~\Content\seed\mobile-bench.jpg"))
                };
                context.Products.Add(product2);

                var product3 = new Product()
                {
                    Name = "Dumbbell",
                    Quantity = 230,
                    Price = 15.99m,
                    Picture = File.ReadAllBytes(HttpContext.Current.Server.MapPath(@"~\Content\seed\dumbbell.png"))
                };
                context.Products.Add(product3);

                var product4 = new Product()
                {
                    Name = "Gladiator",
                    Quantity = 21,
                    Price = 759.99m,
                    Picture = File.ReadAllBytes(HttpContext.Current.Server.MapPath(@"~\Content\seed\gladiator.jpg"))
                };
                context.Products.Add(product4);

                var product5 = new Product()
                {
                    Name = "Leg Press",
                    Quantity = 51,
                    Price = 409.99m,
                    Picture = File.ReadAllBytes(HttpContext.Current.Server.MapPath(@"~\Content\seed\legpress.jpg"))
                };
                context.Products.Add(product5);

                context.SaveChanges();
            }
        }
    }
}
