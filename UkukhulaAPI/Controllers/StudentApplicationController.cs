using UkukhulaAPI.Data.Models.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using UkukhulaAPI.Data.Services;
using UkukhulaAPI.Data;

using UkukhulaAPI.Controllers.Request;


namespace UkukhulaAPI;
[Route("api/[controller]")]
[ApiController]
public class StudentApplicationController : ControllerBase
{

    private readonly NewApplicationService _service;



    public StudentApplicationController(NewApplicationService service)
    {
        _service = service;
    }

    
    [HttpPost]
    [Route("new/student-application")]
    public IActionResult insertStudentApplication([FromBody] AddStudentApplicationRequest addStudentApplicationRequest)
    {

        if (addStudentApplicationRequest != null)
        {
            _service.InsertStudentApplication(addStudentApplicationRequest);
        }

        return Ok();

    }
    [HttpPost]
    [Route("/student-application-update")]
    public IActionResult UpdateStudentApplication([FromBody] UpdateStudentApplicationRequest updateStudent)
    {

        if (updateStudent != null)
        {
            _service.updateStudentApplication(updateStudent);
        }

        return Ok();

    }


}