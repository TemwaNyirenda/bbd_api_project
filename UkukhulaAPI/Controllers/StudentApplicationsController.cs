using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UkukhulaAPI.Data;
using UkukhulaAPI.Data.Models;
using UkukhulaAPI.Data.Models.ViewModels;
using UkukhulaAPI.Data.Services;

namespace UkukhulaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentApplicationsController : ControllerBase
    {
       
        private ApplicationService applicationService;

        private readonly IMapper _mapper;

        public StudentApplicationsController(ApplicationService applicationService, IMapper mapper)
        {

            this.applicationService = applicationService;
            this._mapper = mapper;
        }

        // GET: api/Applications
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public ActionResult<ViewStudentApplication> GetStudentBursaryApplications()
        {

            List<ViewStudentApplication> vStudents = new List<ViewStudentApplication>(); ;
            foreach (var StudentApp in applicationService.GetStudentBursaryApplications())
            {
                var studentApplication = _mapper.Map<ViewStudentApplication>(StudentApp);
                studentApplication.ApplicationStatus = new ViewApplicationStatus();
                studentApplication.ApplicationStatus.StatusId = StudentApp.StatusId;
                studentApplication.ApplicationStatus.Status = StudentApp.Status.Status;
                vStudents.Add(studentApplication);
            }
          
            return Ok(vStudents);
        }


        [HttpPost(Name = "applicationRejectOrApproval")]
        public IActionResult DecideStudentApplication([FromBody] ApplicationRequest request)
        {
            if (request.ApplicationID != null && request.Approve != null && request.Comment != null)
            {
                int entriesUpdated = applicationService.ChangeStatusOfStudentApplication((int)request.ApplicationID, (bool)request.Approve, (string)request.Comment);
                if (entriesUpdated > 0)
                {
                    return Ok();
                }
                return BadRequest();

            }
            else if (request.Comment == null)
            {

            }
            return BadRequest();

        }


        [HttpGet("{id}")]
        public IActionResult GetStudentApplicationById(int id)
        {

            ViewStudentApplication vStudents = new ViewStudentApplication(); 
            var StudentApp  = applicationService.GetStudentBursaryApplicationById(id);

            if(StudentApp != null)
            {
                vStudents = _mapper.Map<ViewStudentApplication>(StudentApp);
                vStudents.ApplicationStatus = new ViewApplicationStatus();
                vStudents.ApplicationStatus.Status = StudentApp.Status.Status;
                vStudents.ApplicationStatus.StatusId = StudentApp.StatusId;
                return Ok(vStudents);
            }
            return NotFound();
        }
    }


    public class ApplicationRequest
    {

        public int? ApplicationID { get; set; }

        public bool? Approve { get; set; }


        public string? Comment { get; set; }
    }

}
