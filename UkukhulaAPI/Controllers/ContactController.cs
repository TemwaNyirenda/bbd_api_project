
using Microsoft.AspNetCore.Mvc;
using UkukhulaAPI.Data;

[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly UkukhulaContext _ukukhulaContext;

    public ContactsController(UkukhulaContext ukukhulaContext)
    {
        _ukukhulaContext = ukukhulaContext;
    }

    [HttpGet]
    public IActionResult GetContacts()
    {
     
        var contacts = _ukukhulaContext.Contacts.ToList();

        return Ok(contacts);
    }
}