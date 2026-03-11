using Microsoft.AspNetCore.Mvc;
using WishListWeb.Models;
using WishListWeb.Services;

namespace WishListWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiServices _apiService;

        public AuthController(ApiServices apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _apiService.Login(model);

            if (result.Success)
            {
                HttpContext.Session.SetString("JwtToken", result.Data.Token);
                HttpContext.Session.SetString("UserName", result.Data.Nome);
                HttpContext.Session.SetString("UserRole", result.Data.Role);

                return RedirectToAction("Index", "Wish");
            }

            ViewBag.Error = result.Message;
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _apiService.Register(model);

            if (result.Success)
            {
                TempData["Success"] = "Cadastro realizado! Faça login.";
                return RedirectToAction("Login");
            }

            ViewBag.Error = result.Message;
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}