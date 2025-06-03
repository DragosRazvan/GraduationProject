using GraduationProject.DTOs;
using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;

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
