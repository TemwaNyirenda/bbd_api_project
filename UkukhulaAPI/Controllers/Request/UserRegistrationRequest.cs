

using UkukhulaAPI.Data.Models.View;

namespace UkukhulaAPI.Controllers.Request;

public class UserRegistrationRequest
    {
        public required UserRegistrationVm User { get; set; }
         public required ContactVm userContact { get; set; }
       
    }