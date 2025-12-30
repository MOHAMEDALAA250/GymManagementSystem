using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    //BaseUrl/Home/Index
    public class HomeController : Controller
    {
        //BaseUrl/Home/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}
