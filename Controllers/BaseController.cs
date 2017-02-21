namespace OnlineShop.Controllers
{
    using System.Web.Mvc;
    using Models;

    public class BaseController : Controller
    {
        protected ApplicationDbContext Data { get; private set; }

        protected BaseController()
        {
            this.Data = new ApplicationDbContext();
        }
    }
}