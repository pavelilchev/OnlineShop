namespace OnlineShop.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
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