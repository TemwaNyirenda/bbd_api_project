
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UkukhulaAPI.Data;


// ...

[ApiController]
[Route("[controller]")]
public class StudentBursaryApplicationController : ControllerBase
{
    private readonly UkukhulaContext _ukukhulaContext;

    public StudentBursaryApplicationController(UkukhulaContext ukukhulaContext)
    {
        _ukukhulaContext = ukukhulaContext;
    }

    [HttpGet]
    public IActionResult GetStudentBursaries()
    {
     
        var studentBursaryApplication = _ukukhulaContext.StudentBursaryApplications.ToList();

        return Ok(studentBursaryApplication);
    }
}