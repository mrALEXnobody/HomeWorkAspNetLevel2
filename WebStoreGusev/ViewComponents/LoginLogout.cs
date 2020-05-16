using Microsoft.AspNetCore.Mvc;

namespace WebStoreGusev.ViewComponents
{
    public class LoginLogout : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
