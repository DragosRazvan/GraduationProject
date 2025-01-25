using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/[Controller]")]
        public async Task<ActionResult<List<ProfessorModel>>> GetAvailableCoordinatorsAsync(int studentSpecializationId)
        {
            var coordinatorsList = await _context.Professors.Where(p => p.SpecializationId.Contains(studentSpecializationId)).ToListAsync();

            return coordinatorsList;
        }

        [HttpGet]
        [Route("api/[controller]/{studentId}")]
        public async Task<ActionResult<ProjectRequestModel>> GetProjectRequest(int studentId)
        {
            return await _context.ProjectRequests.FindAsync(studentId);
        }

        [HttpPost]
        [Route("api/[controller]/{id}")]
        public async Task<IActionResult> PostProjectRequestAsync(ProjectRequestModel projectRequest)
        { 
            _context.ProjectRequests.Add(projectRequest);
            await _context.SaveChangesAsync();

            return Created();
        }
    }
}
