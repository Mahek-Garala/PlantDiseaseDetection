using Microsoft.AspNetCore.Mvc;

namespace PlantDiseaseDetection.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
