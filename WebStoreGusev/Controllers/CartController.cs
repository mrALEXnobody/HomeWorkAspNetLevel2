using Microsoft.AspNetCore.Mvc;
using WebStoreGusev.Infrastructure.Interfaces;
using WebStoreGusev.Models;

namespace WebStoreGusev.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IOrdersService ordersService;

        public CartController(ICartService cartService, IOrdersService ordersService)
        {
            this.cartService = cartService;
            this.ordersService = ordersService;
        }

        public IActionResult Details()
        {
            var model = new OrderDetailsViewModel()
            {
                CartViewModel = cartService.TransformCart(),
                OrderViewModel = new OrderViewModel()
            };

            return View(model);
        }

        public IActionResult DecrementFromCart(int id)
        {
            cartService.DecrementFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveFromCart(int id)
        {
            cartService.RemoveFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveAll()
        {
            cartService.RemoveAll();
            return RedirectToAction("Details");
        }

        public IActionResult AddToCart(int id, string returnUrl)
        {
            cartService.AddToCart(id);
            return Redirect(returnUrl);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var orderResult = ordersService.CreateOrder(model, cartService.TransformCart(), User.Identity.Name);
                cartService.RemoveAll();
                return RedirectToAction("OrderConfirmed", new { id = orderResult.Id });
            }

            var detailsModel = new OrderDetailsViewModel()
            {
                CartViewModel = cartService.TransformCart(),
                OrderViewModel = model
            };

            return View("Details", detailsModel);
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}