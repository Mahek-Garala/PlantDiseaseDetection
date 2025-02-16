using Microsoft.AspNetCore.Mvc;

namespace PlantDiseaseDetection.Controllers
{
    public class PlantImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
