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

        //[HttpPut]
        //public async Task<IActionResult> AcceptProjectRequestAsync(int professorId, int studentId)
        //{
        //    await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId == studentId).ExecuteUpdateAsync(r => r.SetProperty(
        //        request => request.IsAcceptedByProfessor, true));

        //    return Ok();
        //}

        //[HttpPut]
        //public async Task<IActionResult> RejectProjectRequestAsync(int professorId, int studentId)
        //{
        //    await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId == studentId).ExecuteUpdateAsync(r => r.SetProperty(
        //        request => request.IsAcceptedByProfessor, false));

        //    NotificationModel notificationModel = new NotificationModel
        //    {
        //        NotificationMessage = $"The project request with the "
        //    };

        //    await _context.ProjectRequests.ExecuteDeleteAsync();

        //    return Ok();
        //}

        [HttpPut]
        public async Task<ActionResult> UpdateProjectRequestStateAsync(int projectRequestId, bool accepted)
        {
            try
            {
                var projectRequest = await _context.ProjectRequests.FindAsync(projectRequestId);

                if (projectRequest == null)
                {
                    return NotFound("Project request not found.");
                }

                if (!accepted)
                {
                    _context.ProjectRequests.Remove(projectRequest);
                    await _context.SaveChangesAsync();

                    return Ok("Project request deleted successfully!");
                }

                _context.Entry(projectRequest).Property(p => p.IsAcceptedByProfessor).IsModified = accepted;
                await _context.SaveChangesAsync();

                return Ok("Project request accepted successfully!");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostNewProjectIdeaAsync(ProjectIdeaModel projectIdeaModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(projectIdeaModel);
                }

                _context.ProposedProjectIdeas.Add(projectIdeaModel);
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
