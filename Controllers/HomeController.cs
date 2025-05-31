using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OllamaWebChat.Models;

namespace OllamaWebChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Página inicial que retorna a view com o modelo ChatRequest vazio
        public IActionResult Index()
        {
            return View(new ChatRequest());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
