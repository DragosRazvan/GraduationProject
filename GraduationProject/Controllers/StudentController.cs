using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<ActionResult<List<ProfessorModel>>> GetAvailableCoordinatorsAsync(int studentSpecializationId)
        {
            var coordinatorsList = await _context.Professors.Where(p => p.SpecializationId.Equals(studentSpecializationId) && p.NumberOfCoordinatedProjects < 20).ToListAsync();

            return Ok(coordinatorsList);
        }

        public async Task<ActionResult<List<ProjectIdeaModel>>> GetProjectIdeasOfSelectedProfessor(int professsorId)
        {
            var professorProjectList = await _context.ProposedProjectIdeas.Where(pr => pr.ProfessorId.Equals(professsorId)).ToListAsync();

            return Ok(professorProjectList);
        }

        [HttpGet]
        [Route("api/[controller]/{studentId}")]
        public async Task<ActionResult<ProjectRequestModel>> GetProjectRequest(int studentId)
        {
            return await _context.ProjectRequests.FindAsync(studentId);
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> PostProjectRequestAsync(ProjectRequestModel projectRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.ProjectRequests.Add(projectRequest);
                await _context.SaveChangesAsync();

                return Created();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
