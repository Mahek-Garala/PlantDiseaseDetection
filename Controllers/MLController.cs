using Microsoft.AspNetCore.Mvc;

namespace PlantDiseaseDetection.Controllers
{
    public class MLController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
