
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UkukhulaAPI.Data;


// ...

[ApiController]
[Route("[controller]")]

public class RaceController : ControllerBase
{
    private readonly UkukhulaContext _ukukhulaContext;

    public RaceController(UkukhulaContext ukukhulaContext)
    {
        _ukukhulaContext = ukukhulaContext;
    }

    [HttpGet]
    public IActionResult GetUContacts()
    {
     
        var races = _ukukhulaContext.Races.ToList();

        return Ok(races);
    }
}