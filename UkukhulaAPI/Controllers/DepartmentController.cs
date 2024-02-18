
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UkukhulaAPI.Data;


// ...

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly UkukhulaContext _ukukhulaContext;

    public DepartmentController(UkukhulaContext ukukhulaContext)
    {
        _ukukhulaContext = ukukhulaContext;
    }

    [HttpGet]
    public IActionResult GetDepartments()
    {
     
        var departments = _ukukhulaContext.Departments.ToList();

        return Ok(departments);
    }
}