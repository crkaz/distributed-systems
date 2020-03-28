using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly Models.UserContext _context;
        public BaseController(Models.UserContext context)
        {
            _context = context;
        }
    }
}