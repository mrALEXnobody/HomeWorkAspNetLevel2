using Microsoft.AspNetCore.Mvc;
using WebStoreGusev.Infrastructure;

namespace WebStoreGusev.Controllers
{
    [SampleActionFilter]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //throw new ApplicationException("Ошибочка вышла...");
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult BlogSingle()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }
    }
}