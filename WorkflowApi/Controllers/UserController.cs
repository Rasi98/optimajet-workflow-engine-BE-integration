using Microsoft.AspNetCore.Mvc;
using WorkflowLib.Model;

namespace WorkflowApi.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(Users.Data);
        }

        [HttpGet]
        [Route("inspectors")]
        public async Task<IActionResult> GetInspectors()
        {
            return Ok(Inspectors.Data);
        }
    }
}
