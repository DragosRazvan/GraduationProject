using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpecializationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{specializationId}/GetDepartmentId")]
        public async Task<ActionResult<int>> GetStudentDepartmentId(int specializationId)
        {
            SpecializationModel specializationModel = await _context.Specializations.FindAsync(specializationId);

            if (specializationModel == null)
                return NotFound();

            return Ok(specializationModel.DeparmentId);
        }
    }
}
