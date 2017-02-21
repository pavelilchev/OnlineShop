using Microsoft.Owin;

[assembly: OwinStartup(typeof(OnlineShop.Startup))]
namespace OnlineShop
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
