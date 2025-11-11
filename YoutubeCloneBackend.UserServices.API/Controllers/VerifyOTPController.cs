using Microsoft.AspNetCore.Mvc;

namespace YoutubeCloneBackend.UserServices.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerifyOTPController : Controller
    {
        [HttpPost]
        public IActionResult VerifyOTP()
        {
            return View();
        }
    }
}
