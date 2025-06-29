using GraduationProject.DTOs;
using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecretaryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SecretaryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetSecretaryByEmail/{secretaryEmail}")]
        public async Task<ActionResult<SecretaryDto>> GetSecretaryByEmail(string secretaryEmail)
        {
            SecretaryModel secretaryModel = await _context.Secretary.Where(s => s.Email == secretaryEmail).FirstOrDefaultAsync<SecretaryModel>();

            if (secretaryModel == null)
                return NotFound("No secretary found");

            SecretaryDto secretary = new SecretaryDto
            {
                Id = secretaryModel.Id,
                FirstName = secretaryModel.FirstName,
                SecondName = secretaryModel.SecondName,
                Email = secretaryModel.Email,
                FacultyId = secretaryModel.FacultyId
            };

            return Ok(secretary);
        }


        [HttpPost("PostDepartment")]
        public async Task<ActionResult> PostDepartment(DepartmentDto departmentDto)
        {
            DepartmentModel departmentModel = new DepartmentModel
            {
                Name = departmentDto.Name,
                FacultyId = departmentDto.FacultyId
            };

            _context.Departments.Add(departmentModel);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("PostSpecialization")]
        public async Task<ActionResult> PostSpecialization(SpecializationDto specializationDto)
        {
            SpecializationModel specializationModel = new SpecializationModel
            {
                Name = specializationDto.Name,
                LevelOfEducation = specializationDto.LevelOfEducation,
                DeparmentId = specializationDto.DepartmentId
            };

            _context.Specializations.Add(specializationModel);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
