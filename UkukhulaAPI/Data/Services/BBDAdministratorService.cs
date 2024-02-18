

using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using UkukhulaAPI.Data.Models;
using UkukhulaAPI.Data.Models.ViewModels;

namespace UkukhulaAPI.Data.Services
{

    public class BbdadministratorService
    {

        private UkukhulaContext _context;
        public BbdadministratorService(UkukhulaContext context)
        {
            _context = context;
        }
        public void addUniversityApllication(UniversityApplication universityApplication)
        {

        }
        public bool AllocateFunding([FromBody] vFunding funding)
        {
            var _user = _context.Users.FirstOrDefault(e => e.Idnumber == funding.Idnumber);
            if (_user == null)
            {
                return false;
            }

            var _admin = _context.Bbdadministrators.FirstOrDefault(e => e.UserId == _user.UserId);
            if (_admin == null)
            {
                return false;
            }

            var _newBudget = _context.YearlyBursaryDetails.FirstOrDefault(e => e.BursaryDetailsId == DateTime.Now.Year);
            if (_newBudget == null)
            {
                var yearBudget = new YearlyBursaryDetail()
                {
                    CapAmountPerStudent = funding.CapAmountPerStudent,
                    YearBudget = funding.YearBudget
                };
                _context.Add(yearBudget);
                _context.SaveChanges();
                _newBudget = _context.YearlyBursaryDetails.FirstOrDefault(e => e.BursaryDetailsId == DateTime.Now.Year);
            }

            // Retrieve the funded universities
            var fundedUniversities = _context.UniversityApplications
                   .Include(u => u.University)
                   .ThenInclude(e => e.YearlyUniversityAllocations)
                   .ThenInclude(r => r.BursaryDetails)
                   .Where(e => e.Status.StatusId == 2)
                   .ToList();
            int count = fundedUniversities.Count();
            if (count == 0)
            {
                // No universities are eligible for funding
                return false;
            }

            // Calculate the amount per institution
            decimal amountPerInstitution = funding.YearBudget / count;

            foreach (var universityApplication in fundedUniversities)
            {
                // Add a new YearlyUniversityAllocation entry for each funded university
                universityApplication.University.YearlyUniversityAllocations.Add(new YearlyUniversityAllocation()
                {
                    AllocatedAmount = amountPerInstitution,
                    BursaryDetailsId = _newBudget.BursaryDetailsId
                });
            }

            // Save changes to persist the allocations
            _context.SaveChanges();
            return true;
        }

    }
}