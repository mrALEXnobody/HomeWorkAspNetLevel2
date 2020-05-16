using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStoreGusev.DomainNew.Entities;
using WebStoreGusev.Models;

namespace WebStoreGusev.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Проверяем логин/пароль пользователя
            var loginResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            
            // Если не успешно
            if (!loginResult.Succeeded)
            {
                ModelState.AddModelError("", "Вход невозможен");
                return View(model);
            }

            // Если успешно

            // если ReturnUrl - локальный 
            if (Url.IsLocalUrl(model.ReturnUrl))
            {
                // перенаправляем туда откуда пришли
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // создаем сущность пользователя
            var user = new User { UserName = model.UserName, Email = model.Email };
            // используем менеджер для создания
            var createResult = await _userManager.CreateAsync(user, model.Password);

            // выводим ошибки 
            if (!createResult.Succeeded)
            {
                foreach (var identityError in createResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                    return View(model);
                }
            }

            // если успешно, производим логин
            await _signInManager.SignInAsync(user, false);
            // добавляем пользователя к группе Users
            await _userManager.AddToRoleAsync(user, "Users");
            return RedirectToAction("Index", "Home");
        }
    }
}