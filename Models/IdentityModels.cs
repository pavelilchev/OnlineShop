namespace OnlineShop.Models
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;


    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartProduct> CartProducts { get; set; }

        public ApplicationDbContext()
            : base("OnlineShop", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartProduct>()
                   .HasRequired<Cart>(s => s.Cart)
                   .WithMany(s => s.Products);

            modelBuilder.Entity<CartProduct>()
                 .HasRequired<Product>(s => s.Product);

            base.OnModelCreating(modelBuilder);
        }
    }
}