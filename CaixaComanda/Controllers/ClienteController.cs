using Microsoft.AspNetCore.Mvc;

namespace CaixaComanda.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}