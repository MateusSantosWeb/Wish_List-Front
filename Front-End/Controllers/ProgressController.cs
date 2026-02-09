using Microsoft.AspNetCore.Mvc;
using WishListWeb.Models;
using WishListWeb.Services;

namespace WishListWeb.Controllers
{
    public class ProgressController : Controller
    {
        private readonly ApiServices _apiService;

        public ProgressController(ApiServices apiService)
        {
            _apiService = apiService;
        }

        private bool IsAuthenticated() => !string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken"));
        
        private bool IsNamorado() => HttpContext.Session.GetString("UserRole") == "Namorado";

        // GET: Progress
        public async Task<IActionResult> Index()
        {
            if (!IsAuthenticated()) 
                return RedirectToAction("Login", "Auth");

            if (!IsNamorado())
            {
                TempData["Error"] = "Acesso negado. Apenas o namorado pode ver esta página.";
                return RedirectToAction("Index", "Wish");
            }

            var progresses = await _apiService.GetAllProgress();
            return View(progresses);
        }

        // POST: Progress/IniciarRealizacao/5
        [HttpPost]
        public async Task<IActionResult> IniciarRealizacao(int wishId)
        {
            if (!IsAuthenticated() || !IsNamorado()) 
                return RedirectToAction("Login", "Auth");

            var result = await _apiService.IniciarRealizacao(wishId);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message ?? "Erro ao iniciar realização";
            }

            return RedirectToAction("Index");
        }

        // POST: Progress/MarcarRealizando/5
        [HttpPost]
        public async Task<IActionResult> MarcarRealizando(int wishId, string notaPrivada)
        {
            if (!IsAuthenticated() || !IsNamorado()) 
                return RedirectToAction("Login", "Auth");

            var result = await _apiService.MarcarRealizando(wishId, notaPrivada);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message ?? "Erro ao marcar como realizando";
            }

            return RedirectToAction("Index");
        }

        // POST: Progress/RealizarWish/5
        [HttpPost]
        public async Task<IActionResult> RealizarWish(int wishId, string notaRealizacao, string? fotosRealizacao)
        {
            if (!IsAuthenticated() || !IsNamorado()) 
                return RedirectToAction("Login", "Auth");

            var result = await _apiService.RealizarWish(wishId, notaRealizacao, fotosRealizacao);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message ?? "Erro ao realizar wish";
            }

            return RedirectToAction("Index");
        }
    }
}
