using Microsoft.AspNetCore.Mvc;
using DistSysACW.Models;

namespace DistSysACW.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly UserContext _context;

        public BaseController(UserContext context)
        {
            _context = context;
        }
    }
}