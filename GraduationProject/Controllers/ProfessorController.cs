using GraduationProject.DTOs;
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

        //[HttpGet("GetAllProjectRequests")]
        //public async Task<ActionResult<List<ProjectRequestModel>>> GetAllProjectRequestsAsync(int professorId)
        //{
        //    return await _context.ProjectRequests.Where(r => r.ProfessorId == professorId).ToListAsync();
        //}

        [HttpGet("GetAllProjectRequests")]
        public async Task<ActionResult<List<ProjectRequestDto>>> GetAllProjectRequestsAsync(int professorId)
        {
            var projectRequests = await _context.ProjectRequests.Where(r => r.ProfessorId == professorId).ToListAsync();

            List<ProjectRequestDto> projectRequestDtos = new List<ProjectRequestDto>();

            foreach(ProjectRequestModel project in projectRequests)
            {
                ProjectRequestDto p1 = new ProjectRequestDto
                {
                    Title = project.Title,
                    Description = project.Description,
                    LevelOfEducation = project.LevelOfEducation,
                    IsAcceptedByProfessor = project.IsAcceptedByProfessor,
                    StudentId = project.StudentId,
                    ProfessorId = project.ProfessorId
                };

                projectRequestDtos.Add(p1);
            }

            return projectRequestDtos;
        }

        //[HttpGet("GetSpecificProjectRequest/{studentId}")]
        //public async Task<ActionResult<ProjectRequestModel>> GetSpecificProjectRequestAsync(int professorId, int studentId)
        //{
        //    var projectRequest = await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId == studentId).FirstOrDefaultAsync() ?? new ProjectRequestModel();

        //    return projectRequest;
        //}

        [HttpGet("GetSpecificProjectRequest/{studentId}")]
        public async Task<ActionResult<ProjectRequestDto>> GetSpecificProjectRequestAsync(int professorId, int studentId)
        {
            ProjectRequestModel projectRequest = await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId == studentId).FirstOrDefaultAsync();

            ProjectRequestDto projectRequestDto = new ProjectRequestDto
            {
                Title = projectRequest.Title,
                Description = projectRequest.Description,
                LevelOfEducation = projectRequest.LevelOfEducation,
                IsAcceptedByProfessor = projectRequest.IsAcceptedByProfessor,
                StudentId = projectRequest.StudentId,
                ProfessorId = projectRequest.ProfessorId
            };

            return Ok(projectRequestDto);
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

        [HttpPut("UpdateProjectRequest/{projectRequestId}")]
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

                projectRequest.IsAcceptedByProfessor = accepted;
                _context.Entry(projectRequest).Property(p => p.IsAcceptedByProfessor).IsModified = true;

                var professor = await _context.Professors.FindAsync(projectRequest.ProfessorId);
                professor.NumberOfCoordinatedProjects++;
                _context.Entry(professor).Property(p => p.NumberOfCoordinatedProjects).IsModified = true;

                await _context.SaveChangesAsync();

                return Ok("Project request accepted successfully!");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPost("PostNewProjectIdea")]
        public async Task<ActionResult> PostNewProjectIdeaAsync([FromBody] NewProjectIdeaDto newProjectIdeaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(newProjectIdeaDto);
                }

                var newProjectIdeaModel = new ProjectIdeaModel
                {
                    Title = newProjectIdeaDto.Title,
                    Description = newProjectIdeaDto.Description,
                    LevelOfEducation = newProjectIdeaDto.LevelOfEducation,
                    ProfessorId = newProjectIdeaDto.ProfessorId
                };

                _context.ProfessorsProjectIdeas.Add(newProjectIdeaModel);
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
