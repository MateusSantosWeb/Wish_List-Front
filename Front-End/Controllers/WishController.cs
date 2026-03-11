using Microsoft.AspNetCore.Mvc;
using WishListWeb.Models;
using WishListWeb.Services;

namespace WishListWeb.Controllers
{
    public class WishController : Controller
    {
        private readonly ApiServices _apiService;
        private readonly ILogger<WishController> _logger;

        public WishController(ApiServices apiService, ILogger<WishController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        private bool IsAuthenticated() => !string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken"));
        private bool IsNamorado() => HttpContext.Session.GetString("UserRole") == "Namorado";

        // GET: Wish
        public async Task<IActionResult> Index()
        {
            if (!IsAuthenticated()) 
                return RedirectToAction("Login", "Auth");

            var wishes = await _apiService.GetWishes();
            return View(wishes);
        }

        // GET: Wish/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAuthenticated()) 
                return RedirectToAction("Login", "Auth");

            return View();
        }

        // POST: Wish/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WishViewModel model)
        {
            _logger.LogInformation("=== CREATE POST CHAMADO ===");
            _logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");
            _logger.LogInformation($"Titulo: {model.Titulo}");
            _logger.LogInformation($"Categoria: {model.Categoria}");
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"Erro: {error.ErrorMessage}");
                }
                return View(model);
            }

            _logger.LogInformation("Chamando API...");
            var result = await _apiService.CreateWish(model);
            _logger.LogInformation($"Resultado API - Success: {result.Success}, Message: {result.Message}");

            if (result.Success)
            {
                TempData["Success"] = "Wish criado com sucesso!";
                return RedirectToAction("Index");
            }

            ViewBag.Error = result.Message ?? "Erro ao criar desejo";
            return View(model);
        }

        // GET: Wish/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAuthenticated()) 
                return RedirectToAction("Login", "Auth");

            var wish = await _apiService.GetWish(id);
            
            if (wish == null)
            {
                TempData["Error"] = "Wish não encontrado";
                return RedirectToAction("Index");
            }

            return View(wish);
        }

        // POST: Wish/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WishViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _apiService.UpdateWish(id, model);

            if (result.Success)
            {
                TempData["Success"] = "Wish atualizado com sucesso!";
                return RedirectToAction("Index");
            }

            ViewBag.Error = result.Message ?? "Erro ao atualizar desejo";
            return View(model);
        }

        // POST: Wish/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _apiService.DeleteWish(id);

            if (result.Success)
            {
                TempData["Success"] = "Wish removido com sucesso!";
            }
            else
            {
                TempData["Error"] = result.Message ?? "Erro ao deletar";
            }

            return RedirectToAction("Index");
        }

        // POST: Wish/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            if (!IsAuthenticated() || !IsNamorado())
                return RedirectToAction("Login", "Auth");

            var result = await _apiService.UpdateWishStatus(id, status);

            if (result.Success)
            {
                TempData["Success"] = result.Message ?? "Status atualizado com sucesso!";
            }
            else
            {
                TempData["Error"] = result.Message ?? "Erro ao atualizar status";
            }

            return RedirectToAction("Index");
        }

        // GET: Wish/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!IsAuthenticated()) 
                return RedirectToAction("Login", "Auth");

            var wish = await _apiService.GetWish(id);
            
            if (wish == null)
            {
                TempData["Error"] = "Wish não encontrado";
                return RedirectToAction("Index");
            }

            return View(wish);
        }
    }
}
