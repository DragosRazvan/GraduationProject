using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GraduationProject.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectRequestModel>>> GetAllProjectRequestsAsync(int professorId)
        {
            return await _context.ProjectRequests.Where(r => r.ProfessorId == professorId).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<ProjectRequestModel>> GetSpecificProjectRequestAsync(int professorId, int studentId)
        {
            var projectRequest = await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId == studentId).FirstOrDefaultAsync() ?? new ProjectRequestModel();

            return projectRequest;
        }

        [HttpPut]
        public async Task<IActionResult> AcceptProjectRequestAsync(int professorId, int studentId)
        {
            await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId == studentId).ExecuteUpdateAsync(r => r.SetProperty(
                request => request.IsAcceptedByProfessor, true));

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> RejectProjectRequestAsync(int professorId, int studentId)
        {
            await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId == studentId).ExecuteUpdateAsync(r => r.SetProperty(
                request => request.IsAcceptedByProfessor, false));

            NotificationModel notificationModel = new NotificationModel
            {
                NotificationMessage = $"The project request with the "
            };

            await _context.ProjectRequests.ExecuteDeleteAsync();

            return Ok();
        }
    }
}
