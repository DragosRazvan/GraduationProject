using GraduationProject.DTOs;
using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetProjectsByProfessorId/{professorId}")]
        public async Task<ActionResult<ICollection<ProjectRequestDto>>> GetProjectsByProfessorId(int professorId){
            List<ProjectRequestModel> projectRequests = await _context.ProjectRequests.Where(p => (p.StudentId == null && p.ProfessorId == professorId)).ToListAsync<ProjectRequestModel>();

            List<ProjectRequestDto> projectsByProfessorId = new List<ProjectRequestDto>();

            foreach(ProjectRequestModel project in projectRequests)
            {
                ProjectRequestDto newProject = new ProjectRequestDto
                {
                    Id = project.Id,
                    Title = project.Title,
                    Description = project.Description,
                    LevelOfEducation = project.LevelOfEducation,
                    IsAcceptedByProfessor = project.IsAcceptedByProfessor,
                    ProfessorId = project.ProfessorId,
                    StudentId = project.StudentId
                };

                projectsByProfessorId.Add(newProject);
            }
            return Ok(projectsByProfessorId);
        }

    }
}
