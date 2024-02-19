
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UkukhulaAPI.Data;
using UkukhulaAPI.Data.Models.ViewModels;
using UkukhulaAPI.Data.Services;

namespace UkukhulaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Bbdadministrator: ControllerBase
    {
        BbdadministratorService _service;
        public Bbdadministrator(BbdadministratorService service){
            _service = service;
        }
        // [Authorize(Roles ="Admin")]
        [HttpPost]
        [Route("bbd-admin/fund")]
        public IActionResult allocateFunding(vFunding fund)
        {
            if(_service.AllocateFunding(fund))
            {
                return Ok();
            }
            else
            {
                return BadRequest("allocation failed");
            }
            

            // return Ok (_ukukhulaContext.Bbdadministrators.ToArray());
        }
    }
}